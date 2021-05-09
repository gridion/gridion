// <copyright file="DistributedDictionaryTests.cs" company="Gridion">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Gridion.Core;
    using Gridion.Core.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of test methods for <see cref="IDistributedDictionary{TKey,TValue}" /> interface.
    /// </summary>
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class DistributedDictionaryTests : MultiNodeCollectionTestBase
    {
        /// <summary>
        ///     Tests the "Object Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].AddOrUpdate"
        ///     Object AddOrUpdate(Object key,Func`2 addValueFactory,Func`3 updateValueFactory)
        ///     method.
        /// </summary>
        [TestMethod]
        public void AddOrUpdate1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                using (var gridion2 = GridionFactory.Start(new NodeConfiguration("Node2", "127.0.0.1", 24001)))
                {
                    IDistributedDictionary<string, int> expected = gridion.GetDictionary<string, int>("testDictionary");
                    var addedValue = expected.AddOrUpdate("key", _ => 1, (_, _) => 1);

                    IDistributedDictionary<string, int> actual = gridion2.GetDictionary<string, int>("testDictionary");

                    Assert.AreEqual(expected.Count, actual.Count, "The collection lengths are different.");
                    foreach (KeyValuePair<string, int> pair in expected)
                    {
                        Assert.IsTrue(actual.TryGetValue(pair.Key, out var val), "dictionary2.TryGetValue(pair.Key, out var val) failed.");
                        Assert.AreEqual(addedValue, val, "The values are different.");
                        Assert.IsTrue(expected[pair.Key] == val, "dictionary[pair.Key] is not equal to val.");
                    }
                }
            }
        }

        /// <summary>
        ///     Tests a case when a dictionary has been created with one type and then trying to get it with another (with the same name)
        ///     on other node.
        /// </summary>
        [TestMethod]
        public void TryGetDictionaryWithIncorrectArgs()
        {
            Assert.ThrowsException<InvalidCastException>(
                () =>
                { 
                    using (var gridion = GridionFactory.Start(this.CreateNextNodeConfiguration()))
                    {
                        // create a dict with name 'test1' on the first node
                        gridion.GetDictionary<int, string>("test1");
                        using (var gridion2 = GridionFactory.Start(this.CreateNextNodeConfiguration()))
                        {
                            // try to get the dict by name but with different agrs (int, string) on different node
                            gridion2.GetDictionary<string, string>("test1");
                        }
                    }
                });
        }
    }
}