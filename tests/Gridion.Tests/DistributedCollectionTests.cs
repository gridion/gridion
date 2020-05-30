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

namespace Gridion.Tests
{
    using System;

    using Gridion.Core;
    using Gridion.Core.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents the test for a testing of types from the <see cref="Gridion.Core.Collections" /> namespace.
    /// </summary>
    [TestClass]
    public class DistributedCollectionTests
    {
        /// <summary>
        ///     Tests a creation of a <see cref="Gridion.Core.Collections.IDistributedDictionary{TKey,TValue}" /> instance.
        /// </summary>
        [TestMethod]
        public void DistributedDictionaryNameTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, object> dictionary = gridion.GetDictionary<string, object>("name");

                Assert.IsNotNull(dictionary, "dictionary != null");
                Assert.AreEqual("name", dictionary.Name, "The name is incorrect.");
            }
        }

        /// <summary>
        ///     Tests a creation of a <see cref="Gridion.Core.Collections.IDistributedDictionary{TKey,TValue}" />  instance on two
        ///     <see cref="IGridion" /> nodes.
        /// </summary>
        [TestMethod]
        public void DistributedDictionaryTest2Nodes()
        {
            var configuration = new NodeConfiguration("127.0.0.1", 24000);
            var configuration2 = new NodeConfiguration("127.0.0.1", 24001);
            using (var gridion = GridionFactory.Start(configuration))
            {
                using (var gridion2 = GridionFactory.Start(configuration2))
                {
                    IDistributedDictionary<string, object> dictionary = gridion.GetDictionary<string, object>("name");

                    Assert.IsNotNull(dictionary, "dictionary != null");
                    Assert.AreEqual("name", dictionary.Name, "The name is incorrect.");

                    IDistributedDictionary<string, object> dictionary2 = gridion2.GetDictionary<string, object>("name");

                    Assert.IsNotNull(dictionary2, "dictionary != null");
                    Assert.AreEqual("name", dictionary2.Name, "The name is incorrect.");
                }
            }
        }

        /// <summary>
        ///     Tests a creation of a <see cref="Gridion.Core.Collections.IDistributedList{T}" /> instance.
        /// </summary>
        [TestMethod]
        public void DistributedListNameTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedList<string> list = gridion.GetList<string>("name");

                Assert.IsNotNull(list, "list != null");
                Assert.AreEqual("name", list.Name, "The name is incorrect.");
            }
        }

        /// <summary>
        ///     Tests a creation of <see cref="Gridion.Core.Collections.IDistributedList{T}" /> instance on two
        ///     <see cref="IGridion" /> nodes.
        /// </summary>
        [TestMethod]
        public void DistributedListTest2Nodes()
        {
            var configuration = new NodeConfiguration("127.0.0.1", 24000);
            var configuration2 = new NodeConfiguration("127.0.0.1", 24001);
            using (var gridion = GridionFactory.Start(configuration))
            {
                using (var gridion2 = GridionFactory.Start(configuration2))
                {
                    IDistributedList<string> list = gridion.GetList<string>("name");

                    Assert.IsNotNull(list, "list != null");
                    Assert.AreEqual("name", list.Name, "The name is incorrect.");

                    IDistributedList<string> list2 = gridion2.GetList<string>("name");

                    Assert.IsNotNull(list2, "dictionary != null");
                    Assert.AreEqual("name", list2.Name, "The name is incorrect.");
                }
            }
        }

        /// <summary>
        ///     Tests a creation of a <see cref="Gridion.Core.Collections.IDistributedQueue{T}" /> instance.
        /// </summary>
        [TestMethod]
        public void DistributedQueueNameTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedQueue<string> queue = gridion.GetQueue<string>("name");

                Assert.IsNotNull(queue, "queue != null");
                Assert.AreEqual("name", queue.Name, "The name is incorrect.");
            }
        }

        /// <summary>
        ///     Tests a creation of <see cref="Gridion.Core.Collections.IDistributedQueue{T}" /> instance on two
        ///     <see cref="IGridion" /> nodes.
        /// </summary>
        [TestMethod]
        public void DistributedQueueTest2Nodes()
        {
            var configuration = new NodeConfiguration("127.0.0.1", 24000);
            var configuration2 = new NodeConfiguration("127.0.0.1", 24001);
            using (var gridion = GridionFactory.Start(configuration))
            {
                using (var gridion2 = GridionFactory.Start(configuration2))
                {
                    IDistributedQueue<string> queue = gridion.GetQueue<string>("name");

                    Assert.IsNotNull(queue, "queue != null");
                    Assert.AreEqual("name", queue.Name, "The name is incorrect.");

                    IDistributedQueue<string> queue2 = gridion2.GetQueue<string>("name");

                    Assert.IsNotNull(queue2, "queue != null");
                    Assert.AreEqual("name", queue2.Name, "The name is incorrect.");
                }
            }
        }

        /// <summary>
        ///     Tests a creation of a <see cref="Gridion.Core.Collections.IDistributedSet{T}" /> instance.
        /// </summary>
        [TestMethod]
        public void DistributedSetNameTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedSet<string> set = gridion.GetSet<string>("name");

                Assert.IsNotNull(set, "set != null");
                Assert.AreEqual("name", set.Name, "The name is incorrect.");
            }
        }

        /// <summary>
        ///     Tests a creation of <see cref="Gridion.Core.Collections.IDistributedSet{T}" /> instance on two
        ///     <see cref="IGridion" /> nodes.
        /// </summary>
        [TestMethod]
        public void DistributedSetTest2Nodes()
        {
            var configuration = new NodeConfiguration("127.0.0.1", 24000);
            var configuration2 = new NodeConfiguration("127.0.0.1", 24001);
            using (var gridion = GridionFactory.Start(configuration))
            {
                using (var gridion2 = GridionFactory.Start(configuration2))
                {
                    IDistributedSet<string> set = gridion.GetSet<string>("name");

                    Assert.IsNotNull(set, "set != null");
                    Assert.AreEqual("name", set.Name, "The name is incorrect.");

                    IDistributedSet<string> set2 = gridion2.GetSet<string>("name");

                    Assert.IsNotNull(set2, "set != null");
                    Assert.AreEqual("name", set2.Name, "The name is incorrect.");
                }
            }
        }

        /// <summary>
        ///     Tests collection names assertions.
        /// </summary>
        [TestMethod]
        public void ForbiddenCollectionNamesTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                var grid = gridion;
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetList<string>(null), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetList<string>(string.Empty), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetList<string>("   "), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetList<string>("\t"), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetList<string>("\r\n"), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetSet<string>(null), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetSet<string>(string.Empty), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetSet<string>("   "), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetSet<string>("\t"), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetSet<string>("\r\n"), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetQueue<string>(null), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetQueue<string>(string.Empty), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetQueue<string>("   "), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetQueue<string>("\t"), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetQueue<string>("\r\n"), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetDictionary<string, object>(null), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetDictionary<string, object>(string.Empty), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetDictionary<string, object>("   "), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetDictionary<string, object>("\t"), "The non expected exception has been thrown.");
                Assert.ThrowsException<ArgumentNullException>(() => grid.GetDictionary<string, object>("\r\n"), "The non expected exception has been thrown.");
            }
        }
    }
}