// <copyright file="ClusterCurator.cs" company="Gridion">
//     Copyright (c) 2019-2020, Alex Efremov (https://github.com/alexander-efremov)
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
    using System;
    using System.Collections.Generic;
    using Gridion.Core.Interfaces.Implementations;
    using Gridion.Core.Interfaces.Internals;

    /// <inheritdoc />
    internal class ClusterCurator : IClusterCurator
    {
        /// <summary>
        /// The cluster initialization.
        /// </summary>
        private static readonly Lazy<IClusterCurator> Lazy = new Lazy<IClusterCurator>(() => new ClusterCurator());

        /// <summary>
        /// The list of nodes.
        /// </summary>
        private readonly ISet<INodeInternal> nodes;

        /// <summary>
        ///     Prevents a default instance of the <see cref="ClusterCurator" /> class from being created.
        /// </summary>
        private ClusterCurator()
        {
            this.nodes = new HashSet<INodeInternal>();
            this.DataProvider = new MemoryDataProvider(this);
        }

        /// <summary>
        /// Gets the instance of a <see cref="ClusterCurator"/> class.
        /// </summary>
        public static IClusterCurator Instance => Lazy.Value;

        /// <inheritdoc />
        public IDataProvider DataProvider { get; }

        /// <inheritdoc />
        public void Join(INodeInternal node)
        {
            if (this.nodes.Count == 0)
            {
                node.IsMasterNode = true;
            }

            this.nodes.Add(node);
        }

        /// <inheritdoc />
        public IEnumerable<INodeInternal> GetNodes()
        {
            var set = new HashSet<INodeInternal>();

            foreach (var node in this.nodes)
            {
                set.Add(node);
            }

            return set;
        }

        /// <inheritdoc />
        public void Remove(INodeInternal node)
        {
            this.nodes.Remove(node);
        }
    }
}