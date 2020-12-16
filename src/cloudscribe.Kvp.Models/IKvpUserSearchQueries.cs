// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-13
// Last Modified:			2018-10-08
// 

using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Models
{
    public interface IKvpUserSearchQueries
    {
        Task<int> CountUsersForAdminSearch(
           Guid siteId,
           string[] searchTerms,
           CancellationToken cancellationToken = default(CancellationToken));


        Task<PagedResult<IUserInfo>> GetUserAdminSearchPage(
            Guid siteId, 
            int pageNumber, 
            int pageSize, 
            string searchInput, 
            int sortMode, 
            List<string> searchableKvpKeys,
            CancellationToken cancellationToken = default);
    }
}