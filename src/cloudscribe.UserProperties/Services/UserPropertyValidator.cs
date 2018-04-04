// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-10
// Last Modified:			2017-07-10
//

using cloudscribe.UserProperties.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace cloudscribe.UserProperties.Services
{
    public class UserPropertyValidator : IUserPropertyValidator
    {
        public UserPropertyValidator(
            IStringLocalizer<UserPropertyResources> localizer
            )
        {
            _localizer = localizer;
        }

        private IStringLocalizer<UserPropertyResources> _localizer;

        public virtual bool IsValid(
            UserPropertyDefinition prop,
            string postedValue,
            ModelStateDictionary modelState = null
            )
        {
            var result = true;
            if (prop.Required)
            {
                if (string.IsNullOrWhiteSpace(postedValue))
                {
                    result = false;
                    if (modelState != null)
                    {
                        modelState.AddModelError(prop.Key + "Error", _localizer[prop.RequiredErrorMessage]); 
                    }
                }
            }
            if (prop.MaxLength > -1 && !string.IsNullOrWhiteSpace(prop.MaxLengthErrorMessage))
            {
                if (!string.IsNullOrWhiteSpace(postedValue) && (postedValue.Length > prop.MaxLength))
                {
                    result = false;
                    if (modelState != null)
                    {
                        modelState.AddModelError(prop.Key + "Error", _localizer[prop.MaxLengthErrorMessage]); 
                    }
                }
            }
            if (
                !string.IsNullOrWhiteSpace(prop.RegexValidationExpression)
                && !string.IsNullOrWhiteSpace(postedValue)
                )
            {
                var r = new Regex(prop.RegexValidationExpression);
                var isMatch = r.IsMatch(postedValue);
                if (!isMatch)
                {
                    result = false;
                    if (modelState != null)
                    {
                        modelState.AddModelError(prop.Key + "Error", _localizer[prop.RegexErrorMessage]); 
                    }

                }
            }

            return result;
        }

    }
}
