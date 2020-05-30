// <copyright file="ClusterTests.cs" company="Gridion">
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

namespace Gridion.InternalTests
{
    using Gridion.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of tests for a testing of <see cref="ICluster" />  functionality.
    /// </summary>
    [TestClass]
    public class ClusterTests
    {
        /// <summary>
        ///     Tests a number of distributed objects.
        /// </summary>
        [TestMethod]
        public void DistributedObjectsNumberTest1()
        {
            using (var gridion = GridionFactory.Start())
            {
                using (var gridion2 = GridionFactory.Start(new NodeConfiguration("Node2", "127.0.0.1", 24001)))
                {
                    gridion.GetDictionary<string, string>("test1");
                    gridion2.GetDictionary<string, string>("test2");
                 
                    Assert.AreEqual(2, ClusterCurator.Instance.NumberOfDistributedCollections);
                }
            }
        }

        /// <summary>
        ///     Tests that total count of collections is correct.
        /// </summary>
        [TestMethod]
        public void DistributedObjectsNumberTest2()
        {
            var configuration1 = new NodeConfiguration("node1", "127.0.0.1", 24000);
            var configuration2 = new NodeConfiguration("node2", "127.0.0.1", 24001);
            using (var gridion1 = GridionFactory.Start(configuration1))
            {
                using (var gridion2 = GridionFactory.Start(configuration2))
                {
                    // add a dict
                    gridion1.GetDictionary<string, int>("testDictionary1");

                    // get a dict
                    gridion2.GetDictionary<string, int>("testDictionary1");

                    Assert.AreEqual(1, ClusterCurator.Instance.NumberOfDistributedCollections);
                }
            }
        }

        /// <summary>
        ///     Tests a communication of group of nodes.
        /// </summary>
        [TestMethod]
        public void GroupNodesTest()
        {
            var endpoint = Endpoint.GetDefault();
            var cfg1 = new NodeConfiguration("node1", endpoint.Host, endpoint.Port);
            var cfg2 = new NodeConfiguration("node2", endpoint.Host, endpoint.Port + 1);
            using (var gridion = GridionFactory.Start(cfg1))
            {
                Assert.AreEqual(1, gridion.Cluster.Nodes.Count);
                using (var gridion2 = GridionFactory.Start(cfg2))
                {
                    Assert.AreEqual(2, gridion.Cluster.Nodes.Count);
                    Assert.AreEqual(2, gridion2.Cluster.Nodes.Count);
                }

                Assert.AreEqual(1, gridion.Cluster.Nodes.Count);
            }
        }

        /// <summary>
        ///     Tests a master node functionality.
        /// </summary>
        [TestMethod]
        public void MasterNodeTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                foreach (var nd in gridion.Cluster.Nodes)
                {
                    var node = (INodeInternal)nd;
                    Assert.IsTrue(node.IsMasterNode);
                }

                using (var gridion2 = GridionFactory.Start(new NodeConfiguration("Node2", "127.0.0.1", 24001)))
                {
                    var number = 0;
                    foreach (var node1 in gridion2.Cluster.Nodes)
                    {
                        var node = (INodeInternal)node1;
                        if (node.IsMasterNode)
                        {
                            number++;
                        }
                    }

                    Assert.AreEqual(1, number);
                }
            }
        }
    }
}