// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-11
// Last Modified:			2017-07-11
// 

using cloudscribe.Kvp.Models;
using NoDb;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Storage.NoDb
{
    public class KvpItemQueries : IKvpItemQueries
    {
        public KvpItemQueries(
            IBasicQueries<KvpItem> kvpQueries
            )
        {
            _kvpQueries = kvpQueries;
        }

        private IBasicQueries<KvpItem> _kvpQueries;

        public async Task<IKvpItem> FetchById(
            string projectId,
            string itemId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var item = await _kvpQueries.FetchAsync(
                projectId,
                itemId.ToString(),
                cancellationToken).ConfigureAwait(false);
            
            return item;
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
            var all = await _kvpQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return all.Where(
                x => 
                x.Key == key
                && (featureId == "*" || x.FeatureId == featureId)
                && (setId == "*" || x.SetId == setId)
                && (subSetId == "*" || x.SubSetId == subSetId)
                ).FirstOrDefault();

        }

        public async Task<List<IKvpItem>> FetchById(
            string projectId,
            string featureId = "*",
            string setId = "*",
            string subSetId = "*",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var all = await _kvpQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return all.Where(
                x =>
                (featureId == "*" || x.FeatureId == featureId)
                && (setId == "*" || x.SetId == setId)
                && (subSetId == "*" || x.SubSetId == subSetId)
                ).ToList<IKvpItem>();
        }
    }
}
