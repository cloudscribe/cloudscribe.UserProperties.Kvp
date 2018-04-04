// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-08
// Last Modified:			2017-07-13
// 

using cloudscribe.Core.Models;
using cloudscribe.UserProperties.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.UserProperties.Services
{
    public class TenantProfileOptionsResolver : IProfileOptionsResolver
    {
        public TenantProfileOptionsResolver(
            SiteContext currentSite,
            IOptions<ProfilePropertySetContainer> containerAccessor
            )
        {
            this.currentSite = currentSite;
            container = containerAccessor.Value;
        }

        private SiteContext currentSite;
        private ProfilePropertySetContainer container;

        public Task<UserPropertySet> GetProfileProps()
        {
            foreach(var s in container.PropertySets)
            {
                if(s.TenantId == currentSite.Id.ToString()) { return Task.FromResult(s); }
            }

            foreach (var s in container.PropertySets)
            {
                if (s.TenantId =="*") { return Task.FromResult(s); }
            }

            var result = new UserPropertySet();
            return Task.FromResult(result);
        }
    }
}
