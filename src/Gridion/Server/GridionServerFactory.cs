// <copyright file="GridionServerFactory.cs" company="Gridion">
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

namespace Gridion.Core.Server
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    using Gridion.Core.Configurations;
    using Gridion.Core.Interfaces.Internals;
    using Gridion.Core.Utils;

    /// <summary>
    ///     Represents a set of operations to work with <see cref="IGridionServer" /> instances.
    /// </summary>
    internal static class GridionServerFactory
    {
        /// <summary>
        ///     The sync object to access <see cref="TcpListeners" /> field.
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        ///     Contains a list of started <see cref="TcpListener" /> instances.
        /// </summary>
        private static readonly IDictionary<GridionServerId, TcpListenerWrapper> TcpListeners = new Dictionary<GridionServerId, TcpListenerWrapper>();

        /// <summary>
        ///     Starts a new instance of <see cref="IGridionServer" />.
        /// </summary>
        /// <param name="configuration">
        ///     The configuration of <see cref="IGridionServer" />.
        /// </param>
        /// <returns>
        ///     a started <see cref="IGridionServer" /> instance.
        /// </returns>
        internal static IGridionServer RegisterNewServer(GridionServerConfiguration configuration)
        {
            Should.NotBeNull(configuration, nameof(configuration));

            lock (LockObject)
            {
                var serverId = BuildServerId(configuration.Address);
                if (TcpListeners.ContainsKey(serverId))
                {
                    throw new GridionException($"The server with address {configuration.Address} is already exists.");
                }

                var listener = new TcpListenerWrapper(IPAddress.Parse(configuration.Host), configuration.Port);
                TcpListeners[serverId] = listener;
                IGridionServer server = new GridionServer(serverId, listener, configuration);
                return server;
            }
        }

        /// <summary>
        ///     Remove the <see cref="IGridionServer" /> server from the list of registered instances.
        /// </summary>
        /// <param name="server">
        ///     The server to stop.
        /// </param>
        internal static void UnRegister(IGridionServer server)
        {
            Should.NotBeNull(server, nameof(server));

            lock (LockObject)
            {
                TcpListeners.Remove(server.Id);
            }
        }

        /// <summary>
        ///     Build <see cref="GridionServerId" /> by IP address.
        /// </summary>
        /// <param name="address">
        ///     The address to build a key on.
        /// </param>
        /// <returns>
        ///     an instance of <see cref="GridionServerId" />.
        /// </returns>
        private static GridionServerId BuildServerId(string address)
        {
            return new GridionServerId(address);
        }
    }
}