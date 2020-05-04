// <copyright file="DistributedCollectionTests.cs" company="Gridion">
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

namespace Gridion.InternalTests.DistributedCollections
{
    using System;

    using Gridion.Core;
    using Gridion.Core.Collections;
    using Gridion.Core.Configurations;
    using Gridion.Core.Interfaces.Internals;
    using Gridion.Core.Logging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of test methods to check common properties of distributed collection.
    /// </summary>
    [TestClass]
    public class DistributedCollectionTests
    {
        /// <summary>
        ///     Tests a <see cref="DistributedDictionary{TKey,TValue}.Name" />.
        /// </summary>
        [TestMethod]
        public void DistributedDictionaryNameTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DistributedDictionary<string, string>(null, null));
            Assert.ThrowsException<ArgumentNullException>(() => new DistributedDictionary<string, string>(string.Empty, null));
            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    var clusterCurator = NSubstitute.Substitute.For<IClusterCurator>();
                    var logger = NSubstitute.Substitute.For<ILogger>();
                    var parentNode = new Node(new GridionServerConfiguration("127.0.0.1", 1), clusterCurator, logger);
                    return new DistributedDictionary<string, string>(null, parentNode);
                });
        }

        /// <summary>
        ///     Tests a <see cref="DistributedList{T}.Name" />.
        /// </summary>
        [TestMethod]
        public void DistributedListNameTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DistributedList<string>(null, null));
            Assert.ThrowsException<ArgumentNullException>(() => new DistributedList<string>(string.Empty, null));
            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    var node = NSubstitute.Substitute.For<INode>();
                    return new DistributedList<string>(null, node);
                });
        }

        /// <summary>
        ///     Tests a <see cref="DistributedQueue{T}.Name" />.
        /// </summary>
        [TestMethod]
        public void DistributedQueueNameTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DistributedQueue<string>(null, null));
            Assert.ThrowsException<ArgumentNullException>(() => new DistributedQueue<string>(string.Empty, null));
            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    var node = NSubstitute.Substitute.For<INode>();
                    return new DistributedQueue<string>(null, node);
                });
        }

        /// <summary>
        ///     Tests a <see cref="DistributedSet{T}.Name" />.
        /// </summary>
        [TestMethod]
        public void DistributedSetNameTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new DistributedSet<string>(null, null));
            Assert.ThrowsException<ArgumentNullException>(() => new DistributedSet<string>(null, null));
            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    var node = NSubstitute.Substitute.For<INode>();
                    return new DistributedSet<string>(null, node);
                });
        }
    }
}