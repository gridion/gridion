// <copyright file="ClientServerTests.cs" company="Gridion">
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
    using System.Threading;
    using Gridion.Core;
    using Gridion.Core.Client;
    using Gridion.Core.Client.Configurations;
    using Gridion.Core.Configurations;
    using Gridion.Core.Server;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a set of tests to check a client-server interaction.
    /// </summary>
    [TestClass]
    public class ClientServerTests
    {
        /// <summary>
        ///     Tests a client-server interaction.
        /// </summary>
        [TestMethod]
        public void GridionClientServerTest()
        {
            var serverConfiguration = GridionServerConfiguration.GetDefault();
            using (var server = GridionServerFactory.RegisterNewServer(serverConfiguration))
            {
                using (var cts = new CancellationTokenSource())
                {
                    server.StartListenAsync(cts.Token);

                    Assert.IsTrue(server.IsListening, "server.IsListening");

                    var client = GridionClientFactory.Create();
                    var clientConfiguration = new GridionClientConfiguration(serverConfiguration.Host, serverConfiguration.Port);
                    client.Connect(clientConfiguration.Host, clientConfiguration.Port);

                    Assert.IsTrue(server.IsListening, "server.IsListening");
                    Assert.IsTrue(client.IsConnected, "client.IsConnected");
                }
            }
        }

        /// <summary>
        ///     Tests the running of <see cref="IGridionServer" /> instance.
        /// </summary>
        [TestMethod]
        public void GridionServerCancellableStartListenAsyncTest()
        {
            TestCancellable(true);
            TestCancellable(false);
        }

        /// <summary>
        ///     Tests that another server cannot runs on the same port.
        /// </summary>
        [TestMethod]
        public void GridionServerCannotRunOnSamePortTest()
        {
            using (var server = GridionServerFactory.RegisterNewServer(new GridionServerConfiguration("127.0.0.1", 24000)))
            {
                using (var cts = new CancellationTokenSource())
                {
                    server.StartListenAsync(cts.Token);
                    Assert.IsTrue(server.IsListening, "server.IsListening");

                    Assert.ThrowsException<GridionException>(
                        () =>
                        {
                            var configuration2 = new GridionServerConfiguration("127.0.0.1", 24000);
                            GridionServerFactory.RegisterNewServer(configuration2);
                        },
                        "The expected exception hasn't been thrown.");
                }
            }
        }

        /// <summary>
        ///     The method tests the cancellation of server.
        /// </summary>
        /// <param name="cancel">
        ///     The flag indicating whether need to cancel the operation.
        /// </param>
        private static void TestCancellable(bool cancel)
        {
            IGridionServer server;
            using (server = GridionServerFactory.RegisterNewServer(GridionServerConfiguration.GetDefault()))
            {
                Assert.IsFalse(server.IsListening, "server.IsListening");

                using (var cts = new CancellationTokenSource())
                {
                    server.StartListenAsync(cts.Token);
                    if (cancel)
                    {
                        cts.Cancel();
                        Assert.IsFalse(server.IsListening, "server.IsListening");
                    }
                    else
                    {
                        Assert.IsTrue(server.IsListening, "server.IsListening");
                    }
                }
            }

            Assert.IsFalse(server.IsListening, "server.IsListening");
        }
    }
}