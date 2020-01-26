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

namespace Gridion.InternalTests.DistributedCollections
{
    using Gridion.Core;
    using Gridion.Core.Collections;
    using Gridion.Core.Configurations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of test methods for <see cref="IDistributedDictionary{TKey,TValue}" /> interface.
    /// </summary>
    [TestClass]
    public class DistributedDictionaryTests
    {
        /// <summary>
        ///     Tests the "Void System.Collections.Generic.IDictionary`2[System.Object,System.Object].Add" method.
        /// </summary>
        [TestMethod]
        public void Add1Test()
        {
            using (var gridion = GridionFactory.Start())
            {
                using (var gridion2 = GridionFactory.Start(new GridionConfiguration("Node2", "127.0.0.1", 24001)))
                {
                    IDistributedDictionary<string, int> expected = gridion.GetDictionary<string, int>("Add1Test");
                    expected.TryAdd("item1", 1);

                    IDistributedDictionary<string, int> actual = gridion2.GetDictionary<string, int>("Add1Test");

                    Assert.AreNotSame(expected, actual);
                    Assert.AreEqual(expected.Count, actual.Count);
                }
            }
        }

        /// <summary>
        ///     Tests the "Void
        ///     System.Collections.Generic.ICollection`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].Add"
        ///     method.
        /// </summary>
        [TestMethod]
        public void Add2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.IDictionary.Add" method.
        /// </summary>
        [TestMethod]
        public void Add3Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void
        ///     System.Collections.Generic.ICollection`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].Clear"
        ///     method.
        /// </summary>
        [TestMethod]
        public void Clear1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.IDictionary.Clear" method.
        /// </summary>
        [TestMethod]
        public void Clear2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean
        ///     System.Collections.Generic.ICollection`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].Contains"
        ///     method.
        /// </summary>
        [TestMethod]
        public void Contains1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.IDictionary.Contains" method.
        /// </summary>
        [TestMethod]
        public void Contains2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.Generic.IDictionary`2[System.Object,System.Object].ContainsKey" method.
        /// </summary>
        [TestMethod]
        public void ContainsKey1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.Generic.IReadOnlyDictionary`2[System.Object,System.Object].ContainsKey"
        ///     method.
        /// </summary>
        [TestMethod]
        public void ContainsKey2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void
        ///     System.Collections.Generic.ICollection`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].CopyTo"
        ///     method.
        /// </summary>
        [TestMethod]
        public void CopyTo1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.ICollection.CopyTo" method.
        /// </summary>
        [TestMethod]
        public void CopyTo2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Integer
        ///     System.Collections.Generic.ICollection`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].get_Count"
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetCount1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Integer
        ///     System.Collections.Generic.IReadOnlyCollection`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].get_Count"
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetCount2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Integer System.Collections.ICollection.get_Count" method.
        /// </summary>
        [TestMethod]
        public void GetCount3Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "IEnumerator`1
        ///     System.Collections.Generic.IEnumerable`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].GetEnumerator"
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetEnumerator1GenericTest()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "IEnumerator System.Collections.IEnumerable.GetEnumerator" method.
        /// </summary>
        [TestMethod]
        public void GetEnumerator2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "IDictionaryEnumerator System.Collections.IDictionary.GetEnumerator" method.
        /// </summary>
        [TestMethod]
        public void GetEnumerator3Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.IDictionary.get_IsFixedSize" method.
        /// </summary>
        [TestMethod]
        public void GetIsFixedSize1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean
        ///     System.Collections.Generic.ICollection`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].get_IsReadOnly"
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetIsReadOnly1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.IDictionary.get_IsReadOnly" method.
        /// </summary>
        [TestMethod]
        public void GetIsReadOnly2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.ICollection.get_IsSynchronized" method.
        /// </summary>
        [TestMethod]
        public void GetIsSynchronized1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Object System.Collections.Generic.IDictionary`2[System.Object,System.Object].get_Item" method.
        /// </summary>
        [TestMethod]
        public void GetItem1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Object System.Collections.Generic.IReadOnlyDictionary`2[System.Object,System.Object].get_Item" method.
        /// </summary>
        [TestMethod]
        public void GetItem2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Object System.Collections.IDictionary.get_Item" method.
        /// </summary>
        [TestMethod]
        public void GetItem3Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "ICollection`1 System.Collections.Generic.IDictionary`2[System.Object,System.Object].get_Keys" method.
        /// </summary>
        [TestMethod]
        public void GetKeys1GenericTest()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "IEnumerable`1 System.Collections.Generic.IReadOnlyDictionary`2[System.Object,System.Object].get_Keys"
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetKeys2GenericTest()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "ICollection System.Collections.IDictionary.get_Keys" method.
        /// </summary>
        [TestMethod]
        public void GetKeys3Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "String Collections.IDistributedCollection.get_Name" method.
        /// </summary>
        [TestMethod]
        public void GetName1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Runtime.Serialization.ISerializable.GetObjectData" method.
        /// </summary>
        [TestMethod]
        public void GetObjectData1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Object System.Collections.ICollection.get_SyncRoot" method.
        /// </summary>
        [TestMethod]
        public void GetSyncRoot1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "ICollection`1 System.Collections.Generic.IDictionary`2[System.Object,System.Object].get_Values" method.
        /// </summary>
        [TestMethod]
        public void GetValues1GenericTest()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "IEnumerable`1 System.Collections.Generic.IReadOnlyDictionary`2[System.Object,System.Object].get_Values"
        ///     method.
        /// </summary>
        [TestMethod]
        public void GetValues2GenericTest()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "ICollection System.Collections.IDictionary.get_Values" method.
        /// </summary>
        [TestMethod]
        public void GetValues3Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Runtime.Serialization.IDeserializationCallback.OnDeserialization" method.
        /// </summary>
        [TestMethod]
        public void OnDeserialization1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.Generic.IDictionary`2[System.Object,System.Object].Remove" method.
        /// </summary>
        [TestMethod]
        public void Remove1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean
        ///     System.Collections.Generic.ICollection`1[System.Collections.Generic.KeyValuePair`2[System.Object,System.Object]].Remove"
        ///     method.
        /// </summary>
        [TestMethod]
        public void Remove2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.IDictionary.Remove" method.
        /// </summary>
        [TestMethod]
        public void Remove3Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.Generic.IDictionary`2[System.Object,System.Object].set_Item" method.
        /// </summary>
        [TestMethod]
        public void SetItem1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.IDictionary.set_Item" method.
        /// </summary>
        [TestMethod]
        public void SetItem2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.Generic.IDictionary`2[System.Object,System.Object].TryGetValue" method.
        /// </summary>
        [TestMethod]
        public void TryGetValue1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.Generic.IReadOnlyDictionary`2[System.Object,System.Object].TryGetValue"
        ///     method.
        /// </summary>
        [TestMethod]
        public void TryGetValue2Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }
    }
}