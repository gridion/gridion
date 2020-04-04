// <copyright file="IClusterCurator.cs" company="Gridion">
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

namespace Gridion.Core.Interfaces.Internals
{
    using System.Collections.Generic;

    /// <summary>
    ///     Represents a cluster manager.
    /// </summary>
    internal interface IClusterCurator
    {
        /// <summary>
        ///     Gets the data provider.
        /// </summary>
        IDataProvider DataProvider { get; }

        /// <summary>
        ///     Returns a list of nodes in the cluster.
        /// </summary>
        /// <returns>an enumeration of nodes.</returns>
        IEnumerable<INodeInternal> GetNodes();

        /// <summary>
        ///     Join the node into the cluster.
        /// </summary>
        /// <param name="node">The node to add.</param>
        void Join(INodeInternal node);

        /// <summary>
        ///     Remove the node from the list of active nodes.
        /// </summary>
        /// <param name="node">The node to remove.</param>
        void Remove(INodeInternal node);

        /// <summary>
        ///     Find the master node.
        /// </summary>
        /// <returns>the master node.</returns>
        INodeInternal FindTheMasterNode();
    }
}