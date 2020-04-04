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

namespace Gridion.InternalTests.DistributedCollections.SingleNode
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
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                var addedValue = dictionary.AddOrUpdate("key", s => 1, (s, i) => 1);

                IDistributedDictionary<string, int> dictionary2 = gridion.GetDictionary<string, int>("testDictionary");

                Assert.AreEqual(dictionary.Count, dictionary2.Count, "The collection lengths are different.");
                foreach (KeyValuePair<string, int> pair in dictionary)
                {
                    Assert.IsTrue(dictionary2.TryGetValue(pair.Key, out var val), "dictionary2.TryGetValue(pair.Key, out var val) failed.");
                    Assert.AreEqual(addedValue, val, "The values are different.");
                    Assert.IsTrue(dictionary[pair.Key] == val, "dictionary[pair.Key] is not equal to val.");
                }
            }
        }

        /// <summary>
        ///     Tests the "Object Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].AddOrUpdate"
        ///     Object AddOrUpdate(Object key,Object value,Func`3 updateValueFactory)
        ///     method.
        /// </summary>
        [TestMethod]
        public void AddOrUpdate2Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                var addedValue = dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                IDistributedDictionary<string, int> dictionary2 = gridion.GetDictionary<string, int>("testDictionary");

                Assert.AreEqual(dictionary.Count, dictionary2.Count, "The collection lengths are different.");
                foreach (KeyValuePair<string, int> pair in dictionary)
                {
                    Assert.IsTrue(dictionary2.TryGetValue(pair.Key, out var val), "dictionary2.TryGetValue(pair.Key, out var val) failed.");
                    Assert.AreEqual(addedValue, val, "The values are different.");
                    Assert.IsTrue(dictionary[pair.Key] == val, "dictionary[pair.Key] is not equal to val.");
                }
            }
        }

        /// <summary>
        ///     Tests the "Object Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].AddOrUpdate"
        ///     Object AddOrUpdate(Object key,Func`3 addValueFactory,Func`4 updateValueFactory,TArgument factoryArgument)
        ///     method.
        /// </summary>
        [TestMethod]
        public void AddOrUpdate3Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                var addedValue = dictionary.AddOrUpdate("key", (s, d) => 1, (s, i, a) => 1, 0d);

                IDistributedDictionary<string, int> dictionary2 = gridion.GetDictionary<string, int>("testDictionary");

                Assert.AreEqual(dictionary.Count, dictionary2.Count, "The collection lengths are different.");
                foreach (KeyValuePair<string, int> pair in dictionary)
                {
                    Assert.IsTrue(dictionary2.TryGetValue(pair.Key, out var val), "dictionary2.TryGetValue(pair.Key, out var val) failed.");
                    Assert.AreEqual(addedValue, val, "The values are different.");
                    Assert.IsTrue(dictionary[pair.Key] == val, "dictionary[pair.Key] is not equal to val.");
                }
            }
        }

        /// <summary>
        ///     Tests the "Void Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].Clear"
        ///     Void Clear()
        ///     method.
        /// </summary>
        [TestMethod]
        public void Clear1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.AreEqual(1, dictionary.Count, "dictionary.Count is invalid.");

                dictionary.Clear();

                Assert.AreEqual(0, dictionary.Count, "dictionary.Count is invalid.");
            }
        }

        /// <summary>
        ///     Tests the "Boolean Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].ContainsKey"
        ///     Boolean ContainsKey(Object key)
        ///     method.
        /// </summary>
        [TestMethod]
        public void ContainsKey1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.IsTrue(dictionary.ContainsKey("key"), "dictionary.ContainsKey('key') failed.");
            }
        }

        /// <summary>
        ///     Tests the "Int32 Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].get_Count"
        ///     Int32 get_Count()
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetCount1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.AreEqual(1, dictionary.Count, "The collection count is incorrect.");
            }
        }

        /// <summary>
        ///     Tests the "IEnumerator`1
        ///     Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].GetEnumerator"
        ///     IEnumerator`1 GetEnumerator()
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetEnumerator1GenericTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, val) => 1);

                var i = 0;
                foreach (KeyValuePair<string, int> unused in dictionary)
                {
                    i++;
                }

                Assert.AreEqual(1, i, "The collection count is incorrect.");
            }
        }

        /// <summary>
        ///     Tests the "Boolean Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].get_IsEmpty"
        ///     Boolean get_IsEmpty()
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetIsEmpty1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.IsFalse(dictionary.IsEmpty, "dictionary.IsEmpty is incorrect.");

                dictionary.TryRemove("key", out _);

                Assert.IsTrue(dictionary.IsEmpty, "dictionary.IsEmpty is incorrect.");
            }
        }

        /// <summary>
        ///     Tests the "Object Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].get_Item"
        ///     Object get_Item(Object key)
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetItem1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.AreEqual(1, dictionary["key"]);
            }
        }

        /// <summary>
        ///     Tests the "ICollection`1 Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].get_Keys"
        ///     ICollection`1 get_Keys()
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetKeys1GenericTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                var val = dictionary["key"];

                foreach (var key in dictionary.Keys)
                {
                    Assert.AreEqual("key", key);
                }

                Assert.AreEqual(1, val, "Values are different.");
            }
        }

        /// <summary>
        ///     Tests the "String Gridion.Core.Collections.IDistributedCollection.get_Name"
        ///     String get_Name()
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetName1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.AreEqual("testDictionary", dictionary.Name, "The collection name is invalid.");
            }
        }

        /// <summary>
        ///     Tests the "Object Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].GetOrAdd"
        ///     Object GetOrAdd(Object key,Func`2 valueFactory)
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetOrAdd1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, DictionaryTestObject> dictionary = gridion.GetDictionary<string, DictionaryTestObject>("testDictionary");
                var expected = new DictionaryTestObject(1);
                var actual = dictionary.GetOrAdd("key", s => expected);

                Assert.IsNotNull(actual, "actual != null.");
                Assert.AreEqual(expected.Value, actual.Value, "The objects are different.");
                Assert.AreEqual(1, dictionary.Count, "The collection size is invalid.");

                actual = dictionary.GetOrAdd("key", expected);

                Assert.IsNotNull(actual, "actual != null.");
                Assert.AreEqual(1, dictionary.Count, "The collection size is invalid.");
            }
        }

        /// <summary>
        ///     Tests the "Object Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].GetOrAdd"
        ///     Object GetOrAdd(Object key,Object value)
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetOrAdd2Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, DictionaryTestObject> dictionary = gridion.GetDictionary<string, DictionaryTestObject>("testDictionary");
                var expected = new DictionaryTestObject(1);
                var actual = dictionary.GetOrAdd("key", expected);

                Assert.IsNotNull(actual, "actual != null.");
                Assert.AreEqual(expected, actual, "The objects are different.");
                Assert.AreEqual(1, dictionary.Count, "The collection size is invalid.");

                actual = dictionary.GetOrAdd("key", expected);

                Assert.IsNotNull(actual, "actual != null.");
                Assert.AreEqual(1, dictionary.Count, "The collection size is invalid.");
            }
        }

        /// <summary>
        ///     Tests the "Object Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].GetOrAdd"
        ///     Object GetOrAdd(Object key,Func`3 valueFactory,TArgument factoryArgument)
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetOrAdd3Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, DictionaryTestObject> dictionary = gridion.GetDictionary<string, DictionaryTestObject>("testDictionary");
                var expected = new DictionaryTestObject(1);
                var actual = dictionary.GetOrAdd("key", (d, val) => expected, 0d);

                Assert.IsNotNull(actual, "actual != null.");
                Assert.AreEqual(expected, actual, "The objects are different.");
                Assert.AreEqual(1, dictionary.Count, "The collection size is invalid.");

                actual = dictionary.GetOrAdd("key", expected);

                Assert.IsNotNull(actual, "actual != null.");
                Assert.AreEqual(1, dictionary.Count, "The collection size is invalid.");
            }
        }

        /// <summary>
        ///     Tests the "ICollection`1 Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].get_Values"
        ///     ICollection`1 get_Values()
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetValues1GenericTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, val) => 1);

                var i = 0;
                foreach (var unused in dictionary.Values)
                {
                    i++;
                }

                Assert.AreEqual(1, i, "The collection count is incorrect.");
            }
        }

        /// <summary>
        ///     Tests the "Void Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].set_Item"
        ///     Void set_Item(Object key,Object value)
        ///     method.
        /// </summary>
        [TestMethod]
        public void SetItem1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("keyItem", 1, (s, i) => 1);

                dictionary["keyItem"] = 2;

                Assert.AreEqual(2, dictionary["keyItem"], "The values are invalid.");
            }
        }

        /// <summary>
        ///     Tests the "KeyValuePair`2[] Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].ToArray"
        ///     KeyValuePair`2[] ToArray()
        ///     method.
        /// </summary>
        [TestMethod]
        public void ToArray1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                KeyValuePair<string, int>[] array = dictionary.ToArray();

                Assert.AreEqual(1, array.Length, "The length is invalid.");

                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.AreEqual(1, array.Length, "The length is invalid.");

                dictionary.AddOrUpdate("key2", 2, (s, i) => 2);

                array = dictionary.ToArray();

                Assert.AreEqual(2, array.Length, "The length is invalid.");

                Assert.AreEqual(1, array[0].Value, "The values are invalid.");
                Assert.AreEqual(2, array[1].Value, "The values are invalid.");
            }
        }

        /// <summary>
        ///     Tests the "Boolean Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].TryAdd"
        ///     Boolean TryAdd(Object key,Object value)
        ///     method.
        /// </summary>
        [TestMethod]
        public void TryAdd1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");

                // the first try
                Assert.IsTrue(dictionary.TryAdd("key", 1), "dictionary.TryAdd('key', 1) is invalid.");

                // the second try
                Assert.IsFalse(dictionary.TryAdd("key", 1), "dictionary.TryAdd('key', 1) is invalid.");
            }
        }

        /// <summary>
        ///     Tests the "Boolean Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].TryGetValue"
        ///     Boolean TryGetValue(Object key,Object value)
        ///     method.
        /// </summary>
        [TestMethod]
        public void TryGetValue1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.IsTrue(dictionary.TryGetValue("key", out var value), "dictionary.TryGetValue('key', out int value) is incorrect.");
                Assert.AreEqual(1, value, "The value is incorrect.");
            }
        }

        /// <summary>
        ///     Tests the "Boolean Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].TryRemove"
        ///     Boolean TryRemove(Object key,Object value)
        ///     method.
        /// </summary>
        [TestMethod]
        public void TryRemove1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.IsTrue(dictionary.TryRemove("key", out var value), "dictionary.TryRemove('key', out int value) is invalid.");
                Assert.AreEqual(1, value, "The values are different.");
                Assert.IsFalse(dictionary.TryRemove("key", out _), "dictionary.TryRemove('key', out int value) is invalid.");
            }
        }

        /// <summary>
        ///     Tests the "Boolean Gridion.Core.Collections.IDistributedDictionary`2[System.Object,System.Object].TryUpdate"
        ///     Boolean TryUpdate(Object key,Object newValue,Object comparisonValue)
        ///     method.
        /// </summary>
        [TestMethod]
        public void TryUpdate1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>("testDictionary");
                dictionary.AddOrUpdate("key", 1, (s, i) => 1);

                Assert.IsTrue(dictionary.TryUpdate("key", 2, 1), "dictionary.TryUpdate('key', 2, 1) is invalid.");
            }
        }

        /// <summary>
        ///     Represents a dictionaary test object.
        /// </summary>
        /// <inheritdoc />
        [Serializable]
        private class DictionaryTestObject : IEquatable<DictionaryTestObject>
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="DictionaryTestObject" /> class.
            /// </summary>
            /// <param name="value">
            ///     The value to wrap into a test object.
            /// </param>
            internal DictionaryTestObject(int value)
            {
                this.Value = value;
            }

            /// <summary>
            ///     Gets a value.
            /// </summary>
            internal int Value { get; }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }

                if (object.ReferenceEquals(this, obj))
                {
                    return true;
                }

                return obj.GetType() == this.GetType() && this.Equals((DictionaryTestObject)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return this.Value;
            }

            /// <inheritdoc />
            bool IEquatable<DictionaryTestObject>.Equals(DictionaryTestObject other)
            {
                if (other is null)
                {
                    return false;
                }

                if (object.ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.Value == other.Value;
            }
        }
    }
}