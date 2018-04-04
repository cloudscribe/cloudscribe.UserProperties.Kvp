using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.UserProperties.Models
{
    public interface IProfileOptionsResolver
    {
        Task<UserPropertySet> GetProfileProps();
    }
}
