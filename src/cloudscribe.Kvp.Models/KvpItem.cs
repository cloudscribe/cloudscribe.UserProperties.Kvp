// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-07
// Last Modified:			2017-07-10
// 

using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Kvp.Models
{
    /// <summary>
    /// This class is for re-useable key/value storage from various sub systems
    /// with a few extra fields for your use in identifying and querying your custom data in storage
    /// ie I will be using SiteId as setid and if applicable maybe userid, featureid could id the purpose
    /// custom fields can be for whatever you may think of. you could use it for a sort value so I get a set of these and then sort with that using linq
    /// these are just example ideas. 
    /// You may also just consider this as a data object and map the values into a more meaningful class after retrieval from storage
    /// </summary>
    public class KvpItem : IKvpItem
    {
        public KvpItem()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string FeatureId { get; set; }
        public string SetId { get; set; }
        public string SubSetId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;

        public static KvpItem FromIKvpItem(IKvpItem i)
        {
            var k = new KvpItem();
            k.CreatedUtc = i.CreatedUtc;
            k.Custom1 = i.Custom1;
            k.Custom2 = i.Custom2;
            k.FeatureId = i.FeatureId;
            k.Id = i.Id;
            k.Key = i.Key;
            k.ModifiedUtc = i.ModifiedUtc;
            k.SetId = i.SetId;
            k.SubSetId = i.SubSetId;
            k.Value = i.Value;

            return k;
        }


    }
}
