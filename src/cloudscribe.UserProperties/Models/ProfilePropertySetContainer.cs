// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-08
// Last Modified:			2017-07-08
// 

using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.UserProperties.Models
{
    public class ProfilePropertySetContainer
    {
        public ProfilePropertySetContainer()
        {
            PropertySets = new List<UserPropertySet>();
        }

        public List<UserPropertySet> PropertySets { get; set; }
    }
}
