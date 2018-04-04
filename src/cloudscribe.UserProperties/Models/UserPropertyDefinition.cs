// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-08
// Last Modified:			2017-07-08
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Web.Common.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.UserProperties.Models
{
    public class UserPropertyDefinition : FormItemDefinition
    {
        public UserPropertyDefinition()
        {
            Options = new List<SelectListItem>();
        }


        public bool VisibleOnRegistration { get; set; }
        
        public bool VisibleToUserOnProfile { get; set; } = true;

        public bool EditableByUserOnProfile { get; set; } = true;

        public bool VisibleOnAdminUserEdit { get; set; } = true;

        public bool EditableOnAdminUserEdit { get; set; } = true;


    }
}
