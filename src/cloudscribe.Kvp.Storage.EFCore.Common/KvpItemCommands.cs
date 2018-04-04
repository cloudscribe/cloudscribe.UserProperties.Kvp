// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-13
// Last Modified:			2017-07-13
// 

using cloudscribe.Kvp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public class KvpItemCommands : IKvpItemCommands
    {
        public KvpItemCommands(IKvpDbContext dbContext)
        {
            _db = dbContext;
        }

        private IKvpDbContext _db;

        public async Task Create(
            string projectId,
            IKvpItem kvp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (kvp == null) { throw new ArgumentException("kvp can't be null"); }
            var kvpItem = KvpItem.FromIKvpItem(kvp);

            _db.KvpItems.Add(kvpItem);

            var rowsAffected =
                await _db.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task Update(
            string projectId,
            IKvpItem kvp,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (kvp == null) { throw new ArgumentException("kvp can't be null"); }
            var kvpItem = KvpItem.FromIKvpItem(kvp);

            bool tracking = _db.ChangeTracker.Entries<KvpItem>().Any(x => x.Entity.Id == kvpItem.Id);
            if (!tracking)
            {
                _db.KvpItems.Update(kvpItem);
            }

            var rowsAffected =
                await _db.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task Delete(
            string projectId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            var itemToRemove = await _db.KvpItems.SingleOrDefaultAsync(
                x => x.Id == id 
                , cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove == null) throw new InvalidOperationException("KvpItem not found");

            _db.KvpItems.Remove(itemToRemove);
            var rowsAffected = await _db.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }


    }
}
