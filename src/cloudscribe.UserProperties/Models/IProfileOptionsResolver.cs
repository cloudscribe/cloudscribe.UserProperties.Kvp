using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.UserProperties.Models
{
    public interface IProfileOptionsResolver
    {
        Task<UserPropertySet> GetProfileProps();
        Task<List<UserPropertyDefinition>> GetSearchableProfileProps();
    }
}
