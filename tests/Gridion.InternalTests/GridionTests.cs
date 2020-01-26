// <copyright file="GridionTests.cs" company="Gridion">
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
    using Gridion.Core.Configurations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of tests for a testing of <see cref="GridionFactory" /> internal functionality.
    /// </summary>
    [TestClass]
    public class GridionTests
    {
        /// <summary>
        ///     Tests a stopping of all <see cref="IGridion" /> instances.
        /// </summary>
        [TestMethod]
        public void GridionStopAllTest()
        {
            GridionFactory.ResetDisposedInstanceNumber();

            var configuration1 = new GridionConfiguration("127.0.0.1", 24000);
            var configuration2 = new GridionConfiguration("127.0.0.1", 24001);
            using (GridionFactory.Start(configuration1))
            {
                using (GridionFactory.Start(configuration2))
                {
                    Assert.AreEqual(2, GridionFactory.GetAll().Count, "Invalid instance count.");

                    GridionFactory.StopAll();

                    Assert.AreEqual(0, GridionFactory.GetAll().Count, "Invalid instance count.");
                    Assert.AreEqual(2, GridionFactory.GetDisposedInstanceNumber(), "Invalid instance count.");
                }
            }
        }
    }
}