using cloudscribe.Core.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.UserProperties.Models
{
    public interface IUserPropertyService
    {
        Task<SiteUser> GetUser(string userId);
        bool IsNativeUserProperty(string key);
        bool HasAnyNativeProps(List<UserPropertyDefinition> props);
        string GetNativeUserProperty(ISiteUser siteUser, string key);
        void UpdateNativeUserProperty(ISiteUser siteUser, string key, string value);
        Task<List<UserProperty>> FetchByUser(string siteId, string userId);
        Task<List<UserProperty>> FetchForUserListing(string siteId, string userId);
        Task<int> CountNonNativeUserListingProperties();
        Task<string> FetchUserProperty(string userId, string key);
        Task CreateOrUpdate(string siteId, string userId, string key, string value);
    }
}
