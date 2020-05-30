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

namespace Gridion.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Gridion.Core;
    using Gridion.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of tests for a testing of <see cref="Gridion.Core.GridionFactory" /> methods.
    /// </summary>
    /// <inheritdoc />
    [TestClass]
    public class GridionTests : GridionTestBase
    {
        /// <summary>
        ///     Tests a starting of multiple <see cref="IGridion" /> instances with a default and non-default configurations.
        /// </summary>
        [TestMethod]
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Reviewed. Suppression is OK here.")]
        public void GridionDisposeTest()
        {
            try
            {
                using (GridionFactory.Start())
                {
                }
            }
            catch (Exception e)
            {
                Assert.Fail("The creation of IGridion inside the 'using' directive has been failed: " + e);
            }

            try
            {
                using (GridionFactory.Start(new NodeConfiguration("node")))
                {
                }
            }
            catch (Exception e)
            {
                Assert.Fail("The creation of IGridion inside the 'using' directive has been failed: " + e);
            }
        }

        /// <summary>
        ///     Tests <see cref="IGridion.Name" /> property.
        /// </summary>
        [TestMethod]
        public void GridionInterfaceNameTest()
        {
            using (var gridion = GridionFactory.Start(new NodeConfiguration("New Node")))
            {
                Assert.AreEqual("New Node", gridion.Name, "The node name is invalid.");
            }
        }

        /// <summary>
        ///     Tests a starting of the concrete <see cref="IGridion" /> instance.
        /// </summary>
        [TestMethod]
        public void GridionStartTest()
        {
            using (var gridion = GridionFactory.Start())
            {
                Assert.IsNotNull(gridion);
                Assert.AreEqual(1, GridionFactory.GetAll().Count, "Invalid instance count.");
            }
        }

        /// <summary>
        ///     Tests a stopping of the concrete <see cref="IGridion" /> instance.
        /// </summary>
        [TestMethod]
        public void GridionStopAllTest()
        {
            GridionFactory.Start();

            Assert.AreEqual(1, GridionFactory.GetAll().Count, "Invalid instance count.");

            GridionFactory.StopAll();

            Assert.AreEqual(0, GridionFactory.GetAll().Count, "Invalid instance count.");
        }

        /// <summary>
        ///     Tests a stopping of the concrete <see cref="IGridion" /> instance.
        /// </summary>
        [TestMethod]
        public void GridionStopTest()
        {
            GridionFactory.Start();

            Assert.AreEqual(1, GridionFactory.GetAll().Count, "Invalid instance count.");

            var stopped = GridionFactory.Stop(new List<IGridion>(GridionFactory.GetAll())[0].Name);

            Assert.IsTrue(stopped, "The instance hasn't been stopped.");
            Assert.AreEqual(0, GridionFactory.GetAll().Count, "Invalid instance count.");
        }

        /// <summary>
        ///     Tests a starting of multiple <see cref="IGridion" /> instances.
        /// </summary>
        [TestMethod]
        public void MultipleDefaultGridionStartThrowsTest()
        {
            GridionFactory.Start();

            Assert.ThrowsException<GridionException>(() => { GridionFactory.Start(); }, "The expected exception hasn't been thrown.");
        }

        /// <summary>
        ///     Tests a starting of multiple <see cref="IGridion" /> instances with a default configuration.
        /// </summary>
        [TestMethod]
        public void MultipleDefaultGridionStartWithConfigThrowsTest()
        {
            var configuration = new NodeConfiguration("127.0.0.1", 24000);
            GridionFactory.Start(configuration);

            Assert.ThrowsException<GridionException>(() => { GridionFactory.Start(configuration); }, "The expected exception hasn't been thrown.");
        }

        /// <summary>
        ///     Tests a starting of multiple <see cref="IGridion" /> instances with a default and non-default configurations.
        /// </summary>
        [TestMethod]
        public void MultipleGridionStartTest()
        {
            var configuration1 = new NodeConfiguration("127.0.0.1", 24000);
            var gridion1 = GridionFactory.Start(configuration1);
            var configuration2 = new NodeConfiguration("127.0.0.1", 24001);
            var gridion2 = GridionFactory.Start(configuration2);

            Assert.IsTrue(gridion1.Name.StartsWith("[Gridion] Node - {", StringComparison.InvariantCulture), "Gridion name is incorrect.");
            Assert.IsTrue(gridion2.Name.StartsWith("[Gridion] Node - {", StringComparison.InvariantCulture), "Gridion name is incorrect.");
            Assert.AreNotEqual(gridion1.Name, gridion2.Name, "Gridion names are the same.");
        }
    }
}