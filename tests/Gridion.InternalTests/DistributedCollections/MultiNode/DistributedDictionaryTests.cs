// <copyright file="DistributedDictionaryTests.cs" company="Gridion">
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

namespace Gridion.InternalTests.DistributedCollections.MultiNode
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Gridion.Core;
    using Gridion.Core.Collections;
    using Gridion.Core.Configurations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of test methods for <see cref="IDistributedDictionary{TKey,TValue}" /> interface.
    /// </summary>
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class DistributedDictionaryTests
    {
        /// <summary>
        ///     Tests the "Object Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].AddOrUpdate"
        ///     Object AddOrUpdate(Object key,Func`2 addValueFactory,Func`3 updateValueFactory)
        ///     method.
        /// </summary>
        [TestMethod]
        public void AddOrUpdate1Test()
        {
            var configuration1 = new GridionConfiguration("node1", "127.0.0.1", 24000);
            var configuration2 = new GridionConfiguration("node2", "127.0.0.1", 24001);
            using (var gridion1 = GridionFactory.Start(configuration1))
            {
                using (var gridion2 = GridionFactory.Start(configuration2))
                {
                    IDistributedDictionary<string, int> dictionary = gridion1.GetDictionary<string, int>("testDictionary");
                    var addedValue = dictionary.AddOrUpdate("key", s => 1, (s, i) => 1);

                    IDistributedDictionary<string, int> dictionary2 = gridion2.GetDictionary<string, int>("testDictionary");

                    Assert.AreEqual(dictionary.Count, dictionary2.Count, "The collection lengths are different.");
                    foreach (KeyValuePair<string, int> pair in dictionary)
                    {
                        Assert.IsTrue(dictionary2.TryGetValue(pair.Key, out var val), "dictionary2.TryGetValue(pair.Key, out var val) failed.");
                        Assert.AreEqual(addedValue, val, "The values are different.");
                        Assert.IsTrue(dictionary[pair.Key] == val, "dictionary[pair.Key] is not equal to val.");
                    }
                }
            }
        }
    }
}