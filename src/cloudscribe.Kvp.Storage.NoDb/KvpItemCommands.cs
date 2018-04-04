// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-11
// Last Modified:			2017-07-11
// 

using System;
using System.Collections.Generic;
using System.Text;
using NoDb;
using cloudscribe.Kvp.Models;
using System.Threading.Tasks;
using System.Threading;

namespace cloudscribe.Kvp.Storage.NoDb
{
    public class KvpItemCommands : IKvpItemCommands
    {
        public KvpItemCommands(
            IBasicQueries<KvpItem> kvpQueries,
            IBasicCommands<KvpItem> kvpCommands
            )
        {
            _kvpQueries = kvpQueries;
            _kvpCommands = kvpCommands;
        }

        private IBasicQueries<KvpItem> _kvpQueries;
        private IBasicCommands<KvpItem> _kvpCommands;

        public async Task Create(
            string projectId,
            IKvpItem kvp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var item = KvpItem.FromIKvpItem(kvp);

            await _kvpCommands.CreateAsync(
                projectId,
                item.Id,
                item,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task Update(
            string projectId,
            IKvpItem kvp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var item = KvpItem.FromIKvpItem(kvp);

            await _kvpCommands.UpdateAsync(
                projectId,
                item.Id,
                item,
                cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(
            string projectId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await _kvpCommands.DeleteAsync(projectId, id, cancellationToken).ConfigureAwait(false);
        }



    }
}
