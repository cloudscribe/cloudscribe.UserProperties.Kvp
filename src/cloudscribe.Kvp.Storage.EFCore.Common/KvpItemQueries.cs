// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-13
// Last Modified:			2017-07-13
// 

using cloudscribe.Kvp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public class KvpItemQueries : IKvpItemQueries
    {
        public KvpItemQueries(IKvpDbContext dbContext)
        {
            _db = dbContext;
        }

        private IKvpDbContext _db;

        public async Task<IKvpItem> FetchById(
            string projectId,
            string itemId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _db.KvpItems
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == itemId, cancellationToken)
                .ConfigureAwait(false)
                ;
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

            return await _db.KvpItems
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

        public async Task<List<IKvpItem>> FetchById(
            string projectId,
            string featureId = "*",
            string setId = "*",
            string subSetId = "*",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _db.KvpItems
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
