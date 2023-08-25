// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-08
// Last Modified:			2017-07-13
// 

using cloudscribe.Core.Models;
using cloudscribe.UserProperties.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
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

        public Task<List<UserPropertyDefinition>> GetSearchableProfileProps()
        {
            var result = new List<UserPropertyDefinition>();

            foreach (var s in container.PropertySets)
            {
                if (s.TenantId == currentSite.Id.ToString()) 
                { 
                    foreach (var p in s.Properties)
                    {
                        if (p.Searchable) result.Add(p);
                    }
                    return Task.FromResult(result);
                }
            }

            foreach (var s in container.PropertySets)
            {
                if (s.TenantId == "*") 
                {
                    foreach (var p in s.Properties)
                    {
                        if (p.Searchable) result.Add(p);
                    }
                    return Task.FromResult(result);
                }
            }

            return Task.FromResult(result);
        }

        public Task<List<UserPropertyDefinition>> GetUserListingProfileProps()
        {
            var result = new List<UserPropertyDefinition>();

            foreach (var s in container.PropertySets)
            {
                if (s.TenantId == currentSite.Id.ToString())
                {
                    foreach (var p in s.Properties)
                    {
                        if (p.VisibleOnUserListing) result.Add(p);
                    }
                    return Task.FromResult(result);
                }
            }

            foreach (var s in container.PropertySets)
            {
                if (s.TenantId == "*")
                {
                    foreach (var p in s.Properties)
                    {
                        if (p.VisibleOnUserListing) result.Add(p);
                    }
                    return Task.FromResult(result);
                }
            }

            return Task.FromResult(result);
        }

        public Task<string> GetUserListingViewName()
        {
            if(!string.IsNullOrWhiteSpace(container.UserListingViewName))
                return Task.FromResult(container.UserListingViewName);
            else
                return Task.FromResult("index");
        }
    }
}
