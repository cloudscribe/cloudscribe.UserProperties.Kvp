// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-12
// Last Modified:			2017-07-12
// 

using cloudscribe.Kvp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public class KvpDbContextBase : DbContext
    {
        public KvpDbContextBase(DbContextOptions options) : base(options)
        {

        }

        public DbSet<KvpItem> KvpItems { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    IKvpTableNames tableNames = this.GetService<IKvpTableNames>();
        //    if (tableNames == null)
        //    {
        //        tableNames = new KvpTableNames();
        //    }

        //    modelBuilder.Entity<KvpItem>(entity =>
        //    {

        //    });

        //}

    }
}
