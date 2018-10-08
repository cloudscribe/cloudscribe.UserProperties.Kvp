// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-13
// Last Modified:			2018-10-08
// 

using cloudscribe.Kvp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public class KvpItemQueries : IKvpItemQueries
    {
        public KvpItemQueries(IKvpDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        private readonly IKvpDbContextFactory _contextFactory;

        public async Task<IKvpItem> FetchById(
            string projectId,
            string itemId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var db = _contextFactory.Create())
            {
                return await db.KvpItems
                   .AsNoTracking()
                   .FirstOrDefaultAsync(p => p.Id == itemId, cancellationToken)
                   .ConfigureAwait(false)
                   ;
            }
   
        }

        public async Task<IKvpItem> FetchByKey(
            string projectId,
            string key,
            string featureId = "*",
            string setId = "*",
            string subSetId = "*",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var db = _contextFactory.Create())
            {
                return await db.KvpItems
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                    x => x.Key == key
                    && (featureId == "*" || x.FeatureId == featureId)
                    && (setId == "*" || x.SetId == setId)
                    && (subSetId == "*" || x.SubSetId == subSetId)
                    , cancellationToken)
                    .ConfigureAwait(false)
                    ;
            }
            
        }

        public async Task<List<IKvpItem>> FetchById(
            string projectId,
            string featureId = "*",
            string setId = "*",
            string subSetId = "*",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var db = _contextFactory.Create())
            {
                var query = db.KvpItems
                    .AsNoTracking()
                    .Where(
                    x =>
                    (featureId == "*" || x.FeatureId == featureId)
                    && (setId == "*" || x.SetId == setId)
                    && (subSetId == "*" || x.SubSetId == subSetId)
                    );

                return await query.ToListAsync<IKvpItem>().ConfigureAwait(false);

            }
            
        }


    }
}
