// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-10
// Last Modified:			2017-07-10
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Models
{
    public class KvpStorageService : IKvpStorageService
    {
        public KvpStorageService(
            IKvpItemQueries queries,
            IKvpItemCommands commands
            )
        {
            _queries = queries;
            _commands = commands;
        }

        private IKvpItemQueries _queries;
        private IKvpItemCommands _commands;

        public async Task Create(
            string projectId,
            IKvpItem kvp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await _commands.Create(projectId, kvp, cancellationToken).ConfigureAwait(false);
        }

        public async Task Update(
            string projectId,
            IKvpItem kvp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await _commands.Update(projectId, kvp, cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(
            string projectId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await _commands.Delete(projectId, id, cancellationToken).ConfigureAwait(false);
        }


        public async Task<IKvpItem> FetchById(
            string projectId,
            string itemId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return await _queries.FetchById(projectId, itemId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<IKvpItem>> FetchById(
            string projectId,
            string featureId = "*",
            string setId = "*",
            string subSetId = "*",
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return await _queries.FetchById(
                projectId,
                featureId,
                setId,
                subSetId,
                cancellationToken).ConfigureAwait(false);
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
            return await _queries.FetchByKey(
                projectId,
                key,
                featureId,
                setId,
                subSetId,
                cancellationToken).ConfigureAwait(false);
        }

    }
}
