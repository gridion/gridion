// <copyright file="INodeInternal.cs" company="Gridion">
//     Copyright (c) 2019-2021, Alex Efremov (https://github.com/alexander-efremov)
// </copyright>
// 
// Licensed to the Apache Software Foundation (ASF) under one or more
// contributor license agreements.  See the NOTICE file distributed with
// this work for additional information regarding copyright ownership.
// The ASF licenses this file to You under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with
// the License.  You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// The latest version of this file can be found at https://github.com/gridion/gridion

namespace Gridion.Core
{
    using Gridion.Core.Collections;
    using Gridion.Services.Messages;

    /// <inheritdoc cref="INode" />
    /// <inheritdoc cref="IDistributedCollectionFactory" />
    internal interface INodeInternal : IDistributedCollectionFactory, INode, ISender, IRecipient
    {
        /// <summary>
        ///     Gets the number of distributed objects.
        /// </summary>
        long DistributedObjectsNumber { get; }
        
        /// <summary>
        ///     Gets a value indicating whether the node is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether the node is a master node.
        /// </summary>
        bool IsMasterNode { get; set; }

        /// <summary>
        ///     Starts the node.
        /// </summary>
        void Start();

        /// <summary>
        ///     Stops the node.
        /// </summary>
        void Stop();

        void AddCollection(object collection);
    }
}