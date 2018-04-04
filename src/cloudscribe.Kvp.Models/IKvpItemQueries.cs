// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-10
// Last Modified:			2017-07-10
// 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Models
{
    public interface IKvpItemQueries
    {
        Task<IKvpItem> FetchById(
            string projectId,
            string itemId,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        /// <summary>
        /// at least on of the optional id filters should be provided
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="key"></param>
        /// <param name="featureId"></param>
        /// <param name="setId"></param>
        /// <param name="subSetId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IKvpItem> FetchByKey(
            string projectId,
            string key,
            string featureId = "*",
            string setId = "*",
            string subSetId = "*",
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<List<IKvpItem>> FetchById(
            string projectId,
            string featureId = "*",
            string setId = "*",
            string subSetId = "*",
            CancellationToken cancellationToken = default(CancellationToken)
            );

    }
}
