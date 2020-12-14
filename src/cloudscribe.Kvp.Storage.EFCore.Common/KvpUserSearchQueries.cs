// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-13
// Last Modified:			2018-10-08
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Storage.EFCore.Common;
using cloudscribe.Kvp.Models;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Storage.EFCore.Common
{
    public class KvpUserSearchQueries : IKvpUserSearchQueries
    {
        public KvpUserSearchQueries(IKvpDbContextFactory    kvpContextFactory,
                                    ICoreDbContextFactory   coreDbContextFactory)

        {
            _kvpContextFactory    = kvpContextFactory;
            _coreDbContextFactory = coreDbContextFactory;
        }

        private readonly IKvpDbContextFactory    _kvpContextFactory;
        private readonly ICoreDbContextFactory   _coreDbContextFactory;
        private List<string> kvpMatches = new List<string>();

        public async Task<PagedResult<IUserInfo>> GetUserAdminSearchPage(
            Guid siteId,
            int pageNumber,
            int pageSize,
            string searchInput,
            int sortMode,
            List<string> searchableKvpKeys,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            string searchInputUpper = searchInput.Trim().ToUpper();
            

            using (var dbKvpContext = _kvpContextFactory.Create())
            {
                // todo only serchable kvps
                kvpMatches = dbKvpContext.KvpItems
                    .Where(x => searchableKvpKeys.Contains(x.Key.ToLower()))
                    .Where(x => x.SetId.ToLower().Equals(siteId.ToString().ToLower()))
                    .Where(x => x.Value.ToLower().Contains(searchInput.ToLower()))
                    .Select(x => x.SubSetId.ToLower()).Distinct().ToList();
            }

            using (var dbContext = _coreDbContextFactory.CreateContext())
            {
                IQueryable<IUserInfo> query = from x in dbContext.Users
                                              where
                                              (
                                                  x.SiteId == siteId
                                                      && (
                                                      searchInput == string.Empty
                                                      || x.NormalizedEmail.Contains(searchInputUpper)
                                                      || x.NormalizedUserName.Contains(searchInputUpper)
                                                      || (x.FirstName != null && x.FirstName.ToUpper().Contains(searchInputUpper))
                                                      || (x.LastName != null && x.LastName.ToUpper().Contains(searchInputUpper))
                                                      || x.DisplayName.ToUpper().Contains(searchInputUpper)
                                                      || kvpMatches.Contains(x.Id.ToString().ToLower())
                                                      )
                                              )
                                              select x;

                switch (sortMode)
                {
                    case 2:
                        query = query.OrderBy(sl => sl.LastName).ThenBy(s2 => s2.FirstName).AsQueryable();
                        break;
                    case 1:
                        query = query.OrderByDescending(sl => sl.CreatedUtc).AsQueryable();
                        break;
                    case 0:
                    default:
                        query = query.OrderBy(sl => sl.DisplayName).AsQueryable();
                        break;
                }

                var data = await query
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<IUserInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<IUserInfo>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await CountUsersForAdminSearch(siteId, searchInput, searchableKvpKeys, cancellationToken).ConfigureAwait(false);
                return result;
            }
        }


        public async Task<int> CountUsersForAdminSearch(
           Guid siteId,
           string searchInput,
           List<string> searchableKvpKeys,
           CancellationToken cancellationToken = default(CancellationToken))
        {
            string searchInputUpper = searchInput.Trim().ToUpper();

            using (var dbKvpContext = _kvpContextFactory.Create())
            {
                // todo only serchable kvps
                kvpMatches = dbKvpContext.KvpItems
                    .Where(x => searchableKvpKeys.Contains(x.Key.ToLower()))
                    .Where(x => x.SetId.ToLower().Equals(siteId.ToString().ToLower()))
                    .Where(x => x.Value.ToLower().Contains(searchInput.ToLower()))
                    .Select(x => x.SubSetId.ToLower()).Distinct().ToList();
            }

            using (var dbContext = _coreDbContextFactory.CreateContext())
            {
                return await dbContext.Users.CountAsync<SiteUser>(
                x =>
                (
                    x.SiteId == siteId
                    && (
                    searchInput == string.Empty
                    || x.NormalizedEmail.Contains(searchInputUpper)
                    || x.NormalizedUserName.Contains(searchInputUpper)
                    || (x.FirstName != null && x.FirstName.ToUpper().Contains(searchInputUpper))
                    || (x.LastName != null && x.LastName.ToUpper().Contains(searchInputUpper))
                    || x.DisplayName.ToUpper().Contains(searchInputUpper)
                    || kvpMatches.Contains(x.Id.ToString().ToLower())
                    )
                )
                , cancellationToken
                )
                .ConfigureAwait(false);
            }
        }
    }
}
