using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace cloudscribe.UserProperties.Models
{
    public interface IUserPropertyValidator
    {
        bool IsValid(
            UserPropertyDefinition prop,
            string postedValue,
            ModelStateDictionary modelState = null
            );
    }
}
