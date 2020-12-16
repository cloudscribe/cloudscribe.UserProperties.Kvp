// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-13
// Last Modified:			2020-12-16 - jk

using cloudscribe.Core.Models;
using cloudscribe.Kvp.Models;
using cloudscribe.Pagination.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Storage.NoDb
{
    public class KvpUserSearchQueries : IKvpUserSearchQueries
    {
        public KvpUserSearchQueries(
           IBasicQueries<SiteUser> userQueries,
           IBasicQueries<KvpItem> kvpQueries
           )
        {
            this.userQueries = userQueries;
            this.kvpQueries  = kvpQueries;
        }

        private IBasicQueries<SiteUser> userQueries;
        private IBasicQueries<KvpItem>  kvpQueries;

        private Dictionary<string, List<string>> kvpMatches = new Dictionary<string, List<string>>();

        public async Task<PagedResult<IUserInfo>> GetUserAdminSearchPage(
                    Guid siteId,
                    int pageNumber,
                    int pageSize,
                    string searchInput,
                    int sortMode,
                    List<string> searchableKvpKeys,
                    CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (searchInput == null) searchInput = string.Empty;

            var projectId = siteId.ToString();

            //allows user to enter multiple words (e.g. to allow full name search)
            var searchTerms = searchInput.Trim().ToUpper().Split(" ");
            
            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            
            var users    = allUsers.ToList().AsQueryable();
            var allKvp   = await kvpQueries.GetAllAsync(projectId).ConfigureAwait(false);

            foreach (var term in searchTerms)
            {
                if (!string.IsNullOrWhiteSpace(term))
                {
                    var usersMatchingTerm = allKvp.ToList().AsQueryable()
                                                  .Where (x => searchableKvpKeys.Contains(x.Key.ToUpper()))
                                                  .Where (x => x.SetId.ToUpper().Equals(projectId.ToUpper()))
                                                  .Where (x => x.Value.ToUpper().Contains(term))
                                                  .Select(x => x.SubSetId.ToUpper()).Distinct().ToList();
                    
                    kvpMatches.Add(term, usersMatchingTerm);
                }
            }

            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            IQueryable<IUserInfo> query = users.Where(x => x.SiteId == siteId); 

            foreach (var term in searchTerms)
            {
                if (!string.IsNullOrWhiteSpace(term))
                {
                    // Note each term is already in upper case
                    query = query.Where(x =>
                                               ((SiteUser)x).NormalizedEmail.Contains(term)
                                            || ((SiteUser)x).NormalizedUserName.Contains(term)
                                            || (x.FirstName != null && x.FirstName.ToUpper().Contains(term))
                                            || (x.LastName  != null && x.LastName .ToUpper().Contains(term))
                                            || x.DisplayName.ToUpper().Contains(term)
                                            || kvpMatches[term].Contains(x.Id.ToString().ToUpper())
                     );
                }
            }

            query = query.Distinct();

            query = query.Select( x => new UserInfo
                { 
                    Id                   = x.Id,
                    AvatarUrl            = x.AvatarUrl,
                    AccountApproved      = x.AccountApproved,
                    CreatedUtc           = x.CreatedUtc,
                    DateOfBirth          = x.DateOfBirth,
                    DisplayInMemberList  = x.DisplayInMemberList,
                    DisplayName          = x.DisplayName,
                    Email                = x.Email,
                    FirstName            = x.FirstName,
                    Gender               = x.Gender,
                    IsLockedOut          = x.IsLockedOut,
                    LastLoginUtc         = x.LastLoginUtc,
                    LastName             = x.LastName,
                    PhoneNumber          = x.PhoneNumber,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                    SiteId               = x.SiteId,
                    TimeZoneId           = x.TimeZoneId,
                    UserName             = x.UserName,
                    WebSiteUrl           = x.WebSiteUrl
                }
            );

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

            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<IUserInfo>()
                ;

            return new PagedResult<IUserInfo>
            {
                Data       = data,
                PageNumber = pageNumber,
                PageSize   = pageSize,
                TotalItems = await CountUsersForAdminSearch(siteId, searchTerms, cancellationToken).ConfigureAwait(false)
            };
        }


        public async Task<int> CountUsersForAdminSearch(Guid siteId, string[] searchTerms, CancellationToken cancellationToken = default)
        {
            var allUsers = await userQueries.GetAllAsync(siteId.ToString(), cancellationToken).ConfigureAwait(false);
            var users    = allUsers.ToList().AsQueryable();

            IQueryable<IUserInfo> query = users.Where(x => x.SiteId == siteId);

            foreach (var term in searchTerms)
            {
                if (!string.IsNullOrWhiteSpace(term))
                {
                    // Note each term is already in upper case
                    query = query.Where(x =>
                                               ((SiteUser)x).NormalizedEmail.Contains(term)
                                            || ((SiteUser)x).NormalizedUserName.Contains(term)
                                            || (x.FirstName != null && x.FirstName.ToUpper().Contains(term))
                                            || (x.LastName  != null && x.LastName .ToUpper().Contains(term))
                                            || x.DisplayName.ToUpper().Contains(term)
                                            || kvpMatches[term].Contains(x.Id.ToString().ToUpper())
                     );
                }
            }

            query = query.Distinct();

            return query.ToList().Count();
        }
    }
}
