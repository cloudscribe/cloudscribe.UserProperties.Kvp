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
            this.kvpQueries = kvpQueries;
        }

        private IBasicQueries<SiteUser> userQueries;
        private IBasicQueries<KvpItem> kvpQueries;

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

            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            var users    = allUsers.ToList().AsQueryable();

            var allKvp = await kvpQueries.GetAllAsync(projectId).ConfigureAwait(false);
            var kvpMatches = allKvp.ToList().AsQueryable()
                                            .Where(x => searchableKvpKeys.Contains(x.Key.ToLower()))
                                            .Where(x => x.SetId.ToLower().Equals(projectId.ToLower()))
                                            .Where(x => x.Value.ToLower().Contains(searchInput.ToLower()))
                                            .Select(x => x.SubSetId).Distinct().ToList();


            //sortMode: 0 = DisplayName asc, 1 = JoinDate desc, 2 = Last, First

            int offset = (pageSize * pageNumber) - pageSize;

            string searchInputUpper = searchInput.Trim().ToUpper();

            IQueryable<IUserInfo> query
                = from x in users

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
                        || kvpMatches.Contains(x.Id.ToString())
                        )
                  )
                  select new UserInfo
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
                  };


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
            var result        = new PagedResult<IUserInfo>();
            result.Data       = data;
            result.PageNumber = pageNumber;
            result.PageSize   = pageSize;
            result.TotalItems = await CountUsersForAdminSearch(siteId, searchInput, searchableKvpKeys, cancellationToken).ConfigureAwait(false);
            return result;
        }



        public async Task<int> CountUsersForAdminSearch(
                  Guid siteId,
                  string searchInput,
                  List<string> searchableKvpKeys,
                  CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            var allUsers = await userQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            string searchInputUpper = searchInput.Trim().ToUpper();

            var allKvp = await kvpQueries.GetAllAsync(projectId).ConfigureAwait(false);
            var kvpMatches = allKvp.ToList().AsQueryable()
                                            .Where(x => searchableKvpKeys.Contains(x.Key.ToLower()))
                                            .Where(x => x.SetId.ToLower().Equals(projectId.ToLower()))
                                            .Where(x => x.Value.ToLower().Contains(searchInput.ToLower()))
                                            .Select(x => x.SubSetId).Distinct().ToList();

            return allUsers.Where(
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
                    || kvpMatches.Contains(x.Id.ToString())
                    )
                )
            ).ToList().Count;
        }
    }
}
