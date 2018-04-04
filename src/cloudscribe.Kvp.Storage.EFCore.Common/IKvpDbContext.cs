
using cloudscribe.Kvp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public interface IKvpDbContext : IDisposable
    {
        DbSet<KvpItem> KvpItems { get; set; }

        ChangeTracker ChangeTracker { get; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
