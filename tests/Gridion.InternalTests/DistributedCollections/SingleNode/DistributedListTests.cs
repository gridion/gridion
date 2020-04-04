// <copyright file="DistributedListTests.cs" company="Gridion">
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
    using System.Diagnostics.CodeAnalysis;

    using Gridion.Core;
    using Gridion.Core.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of test methods for <see cref="IDistributedList{T}" /> interface.
    /// </summary>
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    public class DistributedListTests
    {
        /// <summary>
        ///     Tests the "Void System.Collections.Generic.ICollection`1[System.Object].Add" method.
        /// </summary>
        [TestMethod]
        public void Add1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.Generic.ICollection`1[System.Object].Clear" method.
        /// </summary>
        [TestMethod]
        public void Clear1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.Generic.ICollection`1[System.Object].Contains" method.
        /// </summary>
        [TestMethod]
        public void Contains1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.Generic.ICollection`1[System.Object].CopyTo" method.
        /// </summary>
        [TestMethod]
        public void CopyTo1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Int32 System.Collections.Generic.ICollection`1[System.Object].get_Count" method.
        /// </summary>
        [TestMethod]
        public void GetCount1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "IEnumerator`1 System.Collections.Generic.IEnumerable`1[System.Object].GetEnumerator" method.
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
        ///     Tests the "Boolean System.Collections.Generic.ICollection`1[System.Object].get_IsReadOnly" method.
        /// </summary>
        [TestMethod]
        public void GetIsReadOnly1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Object System.Collections.Generic.IList`1[System.Object].get_Item" method.
        /// </summary>
        [TestMethod]
        public void GetItem1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "String Gridion.Core.Collections.IDistributedCollection.get_Name" method.
        /// </summary>
        [TestMethod]
        public void GetName1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Int32 System.Collections.Generic.IList`1[System.Object].IndexOf" method.
        /// </summary>
        [TestMethod]
        public void IndexOf1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.Generic.IList`1[System.Object].Insert" method.
        /// </summary>
        [TestMethod]
        public void Insert1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Boolean System.Collections.Generic.ICollection`1[System.Object].Remove" method.
        /// </summary>
        [TestMethod]
        public void Remove1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.Generic.IList`1[System.Object].RemoveAt" method.
        /// </summary>
        [TestMethod]
        public void RemoveAt1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }

        /// <summary>
        ///     Tests the "Void System.Collections.Generic.IList`1[System.Object].set_Item" method.
        /// </summary>
        [TestMethod]
        public void SetItem1Test()
        {
            using (var unused = GridionFactory.Start())
            {
            }
        }
    }
}