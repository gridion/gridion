// <copyright file="MultiNodeCollectionTestBase.cs" company="Gridion">
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

namespace Gridion.InternalTests.DistributedCollections.MultiNode
{
    using Gridion.Core;
    using Gridion.Core.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of test methods for <see cref="IDistributedQueue{T}" /> interface.
    /// </summary>
    [TestClass]
    public abstract class MultiNodeCollectionTestBase
    {
        /// <summary>
        /// The default port.
        /// </summary>
        private int port = 24000;

        /// <summary>
        /// Creates the node configuration for the next port.
        /// </summary>
        /// <returns>
        /// created node configuration.
        /// </returns>
        protected NodeConfiguration CreateNextNodeConfiguration()
        {
            var nextPort = this.GetNextPort();
            return new NodeConfiguration("testNode " + nextPort, "127.0.0.1", nextPort);
        }

        /// <summary>
        /// Gets the next port.
        /// </summary>
        /// <returns>the next port.</returns>
        private int GetNextPort()
        {
            return ++this.port;
        }
    }
}