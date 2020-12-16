// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-13
// Last Modified:			2020-12-16 - jk
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
        
        private Dictionary<string, List<string>> kvpMatches = new Dictionary<string, List<string>>();

        public async Task<PagedResult<IUserInfo>> GetUserAdminSearchPage(
            Guid              siteId,
            int               pageNumber,
            int               pageSize,
            string            searchInput,
            int               sortMode,
            List<string>      searchableKvpKeys,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            if (searchInput == null) searchInput = string.Empty;

            //allows user to enter multiple words (e.g. to allow full name search)
            var searchTerms = searchInput.Trim().ToUpper().Split(" ");

            int offset = (pageSize * pageNumber) - pageSize;

            string searchInputUpper = searchInput.Trim().ToUpper();
            
            // get a list of user IDs from KVPs that match each search input term
            // provided the KVP is configured searchable
            using (var dbKvpContext = _kvpContextFactory.Create())
            {
                foreach (var term in searchTerms)
                {
                    if (!string.IsNullOrWhiteSpace(term))
                    {
                        var usersMatchingTerm = dbKvpContext.KvpItems
                        .Where (x => searchableKvpKeys.Contains(x.Key.ToUpper()))
                        .Where (x => x.SetId.ToUpper().Equals(siteId.ToString().ToUpper()))
                        .Where (x => x.Value.ToUpper().Contains(term))
                        .Select(x => x.SubSetId.ToUpper()).Distinct().ToList();

                        kvpMatches.Add(term, usersMatchingTerm);
                    }
                }
            }

            using (var dbContext = _coreDbContextFactory.CreateContext())
            {
                var query = dbContext.Users.Where(x => x.SiteId == siteId);
                foreach (var term in searchTerms)
                {
                    if (!string.IsNullOrWhiteSpace(term))  
                    {
                        // Note each term is already in upper case
                        query = query.Where(x =>
                                                    x.NormalizedEmail.Contains(term)
                                                 || x.NormalizedUserName.Contains(term)
                                                 || (x.FirstName != null && x.FirstName.ToUpper().Contains(term))
                                                 || (x.LastName != null && x.LastName.ToUpper().Contains(term))
                                                 || x.DisplayName.ToUpper().Contains(term)
                                                 || kvpMatches[term].Contains(x.Id.ToString().ToUpper())
                         );
                    }
                }

                query = query.Distinct();

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

                return new PagedResult<IUserInfo>
                {
                    Data       = data,
                    PageNumber = pageNumber,
                    PageSize   = pageSize,
                    TotalItems = await CountUsersForAdminSearch(siteId, searchTerms, cancellationToken).ConfigureAwait(false)
                };
            }
        }


        public async Task<int> CountUsersForAdminSearch(
           Guid siteId,
           string[] searchTerms,
           CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var dbContext = _coreDbContextFactory.CreateContext())
            {
                var query = dbContext.Users.Where(x => x.SiteId == siteId);

                foreach (var term in searchTerms)
                {
                    if (!string.IsNullOrWhiteSpace(term))
                    {
                        query = query.Where(x =>
                                                   x.NormalizedEmail.Contains(term)
                                                || x.NormalizedUserName.Contains(term)
                                                || (x.FirstName != null && x.FirstName.ToUpper().Contains(term))
                                                || (x.LastName  != null && x.LastName .ToUpper().Contains(term))
                                                || x.DisplayName.ToUpper().Contains(term)
                                                || kvpMatches[term].Contains(x.Id.ToString().ToUpper())
                         );
                    }
                }

                query = query.Distinct();

                return await query.CountAsync().ConfigureAwait(false);
            }
        }
    }
}
