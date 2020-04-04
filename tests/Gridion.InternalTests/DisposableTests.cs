// <copyright file="DisposableTests.cs" company="Gridion">
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
    using Gridion.Core.Services;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of tests to check a <see cref="Disposable" /> class.
    /// </summary>
    [TestClass]
    public class DisposableTests
    {
        /// <summary>
        ///     Tests a <see cref="Disposable" /> class.
        /// </summary>
        [TestMethod]
        public void DisposableTest()
        {
            var instance = new DisposableInstance();
            Assert.IsFalse(instance.Disposed);
            instance.Dispose();
            Assert.IsTrue(instance.Disposed);
        }

        /// <summary>
        ///     Represents a test implementation of <see cref="Disposable" /> class.
        /// </summary>
        /// <inheritdoc />
        private class DisposableInstance : Disposable
        {
            /// <summary>
            ///     Gets a value indicating whether an object is disposed.
            /// </summary>
            internal new bool Disposed => base.Disposed;
        }
    }
}