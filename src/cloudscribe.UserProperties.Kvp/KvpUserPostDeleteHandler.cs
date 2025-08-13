// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2025-08-13
// Last Modified:			2025-08-13
//

using cloudscribe.Core.Models.EventHandlers;
using cloudscribe.Kvp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.UserProperties.Kvp
{
    /// <summary>
    /// Handles cleanup of user KVP data after successful user deletion.
    /// This handler only executes if the user deletion was successful, ensuring transactional safety.
    /// </summary>
    public class KvpUserPostDeleteHandler : IHandleUserPostDelete
    {
        public KvpUserPostDeleteHandler(
            IKvpStorageService kvpStorage,
            ILogger<KvpUserPostDeleteHandler> logger)
        {
            _kvpStorage = kvpStorage;
            _logger = logger;
        }

        private readonly IKvpStorageService _kvpStorage;
        private readonly ILogger<KvpUserPostDeleteHandler> _logger;

        public async Task HandleUserPostDelete(
            Guid siteId, 
            Guid userId, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("Starting KVP cleanup for deleted user {UserId} in site {SiteId}", userId, siteId);
            
            try
            {
                // Fetch all KVP items for this user
                // User KVPs are stored with SubSetId = userId
                var userKvps = await _kvpStorage.FetchById(
                    siteId.ToString(), // projectId
                    "*",               // featureId (all features)
                    siteId.ToString(), // setId (site-scoped)  
                    userId.ToString(), // subSetId (user-scoped)
                    cancellationToken).ConfigureAwait(false);
                    
                if (userKvps.Count == 0)
                {
                    _logger.LogDebug("No KVP data found for user {UserId}", userId);
                    return;
                }

                // Delete each KVP item individually
                int deletedCount = 0;
                foreach (var kvp in userKvps)
                {
                    try
                    {
                        await _kvpStorage.Delete(siteId.ToString(), kvp.Id, cancellationToken).ConfigureAwait(false);
                        deletedCount++;
                        _logger.LogDebug("Deleted KVP item {KvpId} (key: {Key}) for user {UserId}", kvp.Id, kvp.Key, userId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to delete KVP item {KvpId} (key: {Key}) for user {UserId}", kvp.Id, kvp.Key, userId);
                        // Continue with other items even if one fails
                    }
                }
                
                _logger.LogInformation("Successfully cleaned up {DeletedCount} of {TotalCount} KVP items for user {UserId}", 
                    deletedCount, userKvps.Count, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to cleanup KVP data for user {UserId} in site {SiteId}", userId, siteId);
                // Don't throw - let other post-delete handlers continue
                // The user has already been successfully deleted from the main system
            }
        }
    }
}