// <copyright file="SerializationAssertionTests.cs" company="Gridion">
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
    using System.Runtime.Serialization;

    using Gridion.Core.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of test methods to check serialization restrictions of <see cref="Gridion" /> collections.
    /// </summary>
    [TestClass]
    public class SerializationAssertionTests
    {
        /// <summary>
        ///     Tests that <see cref="DistributedDictionary{TKey,TValue}" /> check against serialization of key and value.
        /// </summary>
        [TestMethod]
        public void DistributedDictionarySerializationTest()
        {
            Assert.ThrowsException<SerializationException>(() => new DistributedDictionary<SerializationAssertionTests, string>("dict"));
            Assert.ThrowsException<SerializationException>(() => new DistributedDictionary<string, SerializationAssertionTests>("dict"));
            Assert.ThrowsException<SerializationException>(() => new DistributedDictionary<SerializationAssertionTests, SerializationAssertionTests>("dict"));
        }

        /// <summary>
        ///     Tests that <see cref="DistributedList{T}" /> check against serialization of value.
        /// </summary>
        [TestMethod]
        public void DistributedListSerializationTest()
        {
            Assert.ThrowsException<SerializationException>(() => new DistributedList<SerializationAssertionTests>(string.Empty));
        }

        /// <summary>
        ///     Tests that <see cref="DistributedQueue{T}" /> check against serialization of value.
        /// </summary>
        [TestMethod]
        public void DistributedQueueSerializationTest()
        {
            Assert.ThrowsException<SerializationException>(() => new DistributedQueue<SerializationAssertionTests>(string.Empty));
        }

        /// <summary>
        ///     Tests that <see cref="DistributedSet{T}" /> check against serialization of value.
        /// </summary>
        [TestMethod]
        public void DistributedSetSerializationTest()
        {
            Assert.ThrowsException<SerializationException>(() => new DistributedSet<SerializationAssertionTests>(string.Empty));
        }
    }
}