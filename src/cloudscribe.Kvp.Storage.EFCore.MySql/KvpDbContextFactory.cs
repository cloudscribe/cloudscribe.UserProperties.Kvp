using cloudscribe.Kvp.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Kvp.Storage.EFCore.MySql
{
    public class KvpDbContextFactory : IKvpDbContextFactory
    {
        public KvpDbContextFactory(DbContextOptions<KvpDbContext> options)
        {
            _options = options;
        }

        private readonly DbContextOptions<KvpDbContext> _options;

        public IKvpDbContext Create()
        {
            return new KvpDbContext(_options);
        }

    }
}
