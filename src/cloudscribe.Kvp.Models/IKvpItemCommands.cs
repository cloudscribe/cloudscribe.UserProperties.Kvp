// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-10
// Last Modified:			2017-07-10
// 

using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Kvp.Models
{
    public interface IKvpItemCommands
    {
        Task Create(
            string projectId,
            IKvpItem kvp,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Update(
            string projectId,
            IKvpItem kvp,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task Delete(
            string projectId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
