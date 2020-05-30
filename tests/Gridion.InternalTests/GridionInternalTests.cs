// <copyright file="GridionInternalTests.cs" company="Gridion">
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

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of tests to check a <see cref="IGridionInternal" /> functionality.
    /// </summary>
    [TestClass]
    public class GridionInternalTests
    {
        /// <summary>
        ///     Tests a stopping of the <see cref="IGridion" /> instance.
        /// </summary>
        [TestMethod]
        public void GridionStopTest()
        {
            IGridionInternal gridionInternal = null;
            try
            {
                var configuration = new NodeConfiguration("127.0.0.1", 24000);

                using (gridionInternal = (IGridionInternal)GridionFactory.Start(configuration))
                {
                    gridionInternal.NodeStart();

                    Assert.IsTrue(gridionInternal.IsRunning, "Invalid gridion node state.");
            
                    gridionInternal.NodeStop();

                    Assert.IsFalse(gridionInternal.IsRunning, "Invalid gridion node state.");
                }
            }
            finally
            {
                if (gridionInternal != null)
                {
                    gridionInternal.Dispose();
                }
            }
        }
    }
}