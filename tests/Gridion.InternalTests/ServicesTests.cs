// <copyright file="ServicesTests.cs" company="Gridion">
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

namespace Gridion.InternalTests
{
    using Gridion.Services;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of tests for a testing of classes with <see cref="IGridionService" /> interface.
    /// </summary>
    [TestClass]
    public class ServicesTests
    {
        /// <summary>
        ///     Tests the name of a service.
        /// </summary>
        [TestMethod]
        public void ServiceNamesTest()
        {
            using (IGridionService distributedCollectionService = new DistributedCollectionService())
            {
                Assert.AreEqual("DistributedCollectionService", distributedCollectionService.Name);
            }

            using (IGridionService outMessengerService = new MemoryMessengerService())
            {
                Assert.AreEqual("MemoryMessengerService", outMessengerService.Name);
            }
        }

        /// <summary>
        ///     Tests the 'Stop' operation of a service.
        /// </summary>
        [TestMethod]
        public void StartStopTest()
        {
            using (IGridionService distributedCollectionService = new DistributedCollectionService())
            {
                using (IGridionService messengerService = new MemoryMessengerService())
                {
                    distributedCollectionService.Start();
                    messengerService.Start();

                    Assert.IsTrue(distributedCollectionService.IsRunning);
                    Assert.IsTrue(messengerService.IsRunning);

                    distributedCollectionService.Stop();
                    messengerService.Stop();

                    Assert.IsFalse(distributedCollectionService.IsRunning);
                    Assert.IsFalse(messengerService.IsRunning);
                }
            }
        }
    }
}