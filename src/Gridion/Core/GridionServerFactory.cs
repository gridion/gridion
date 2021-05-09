// <copyright file="GridionServerFactory.cs" company="Gridion">
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

namespace Gridion.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    using Gridion.Core.Extensions;
    using Gridion.Exceptions;
    using Gridion.Internal.Logging;

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
        private static readonly IDictionary<GridionServerId, TcpListener> TcpListeners = new Dictionary<GridionServerId, TcpListener>();

        /// <summary>
        ///     Starts a new instance of <see cref="IGridionServer" />.
        /// </summary>
        /// <param name="configuration">
        ///     The configuration of <see cref="IGridionServer" />.
        /// </param>
        /// <returns>
        ///     a started <see cref="IGridionServer" /> instance.
        /// </returns>
        internal static IGridionServer RegisterNewServer(ServerConfiguration configuration)
        {
            Should.NotBeNull(configuration, nameof(configuration));

            lock (LockObject)
            {
                var serverId = BuildServerId(configuration.Address);
                if (TcpListeners.ContainsKey(serverId))
                {
                    throw new GridionException($"The server with address {configuration.Address} is already exists.");
                }

                var listener = new TcpListener(IPAddress.Parse(configuration.Host), configuration.Port);
                TcpListeners[serverId] = listener;
                IGridionServer server = new GridionGridionServer(serverId, listener, configuration);
                return server;
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

        /// <summary>
        ///     Remove the <see cref="IGridionServer" /> server from the list of registered instances.
        /// </summary>
        /// <param name="server">
        ///     The server to stop.
        /// </param>
        private static void UnRegister(IGridionServer server)
        {
            Should.NotBeNull(server, nameof(server));

            lock (LockObject)
            {
                TcpListeners.Remove(server.Id);
            }
        }

        /// <summary>
        ///     Represents an internal <see cref="IGridionServer" /> implementation.
        /// </summary>
        /// <inheritdoc cref="IGridionServer" />
        private sealed class GridionGridionServer : IGridionServer
        {
            /// <summary>
            ///     The <see cref="GridionGridionServer" /> ID.
            /// </summary>
            private readonly GridionServerId id;

            /// <summary>
            ///     The assigned instance of <see cref="TcpListener" /> class.
            /// </summary>
            private readonly TcpListener listener;

            /// <summary>
            ///     The logger.
            /// </summary>
            private readonly ILogger logger;

            /// <summary>
            ///     Initializes a new instance of the <see cref="GridionGridionServer" /> class.
            /// </summary>
            /// <param name="id">
            ///     The <see cref="IGridionServer" /> ID.
            /// </param>
            /// <param name="listener">
            ///     The <see cref="TcpListener" />.
            /// </param>
            /// <param name="configuration">
            ///     The <see cref="IGridionServer" /> configuration.
            /// </param>
            internal GridionGridionServer(GridionServerId id, TcpListener listener, ServerConfiguration configuration)
            {
                this.id = id;
                this.listener = listener;
                this.Configuration = configuration;
                this.logger = this.Configuration.Logger;
            }

            /// <inheritdoc />
            public ServerConfiguration Configuration { get; }

            /// <inheritdoc />
            public bool IsListening { get; private set; }

            /// <inheritdoc />
            GridionServerId IGridionServer.Id => this.id;

            /// <summary>
            ///     Gets or sets a value indicating whether the instance is disposed.
            /// </summary>
            private bool Disposed { get; set; }

            /// <inheritdoc />
            public void Dispose()
            {
                this.Dispose(true);
            }

            /// <inheritdoc />
            public void Stop()
            {
                UnRegister(this);
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return this.Configuration != null ? this.Configuration.ToString() : "[Empty Server]";
            }

            /// <inheritdoc />
            async Task IGridionServer.StartListenAsync(CancellationToken cancellationToken)
            {
                try
                {
                    this.listener.Start();
                    this.logger.Info($"Started to listening on {this.Configuration.Address}.");
                    this.IsListening = true;
                    while (true)
                    {
                        Task<TcpClient> task = this.listener.AcceptTcpClientAsync();
                        var client = await task.WithCancellationAsync(cancellationToken).ConfigureAwait(false);
                        if (client.Connected)
                        {
                            this.Configuration.Logger.Info($"Client has been connected to {this}");
                        }
                    }
                }

#pragma warning disable CA1031 // Do not catch general exception types
                catch (OperationCanceledException)
                {
                    this.IsListening = false;
                    this.logger.Info(SR.StoppedToListen);
                }

#pragma warning restore CA1031 // Do not catch general exception types
                catch (SocketException e)
                {
                    throw ExceptionHelper.ThrowGridionServerException(SR.FailedServerToStart, e);
                }
            }

            /// <summary>
            ///     Disposes the resources.
            /// </summary>
            /// <param name="disposing">
            ///     Indicates whether need to dispose managed resources.
            /// </param>
            private void Dispose(bool disposing)
            {
                if (this.Disposed)
                {
                    return;
                }

                if (disposing)
                {
                    // dispose managed resources
                    this.DisposeManaged();
                }

                // dispose unmanaged resources
                this.Disposed = true;
            }

            /// <summary>
            ///     Dispose the internal managed/unmanaged resources.
            /// </summary>
            private void DisposeManaged()
            {
                UnRegister(this);
                this.listener.Stop();
                this.IsListening = false;
            }
        }
    }
}

// limitations under the License.

// The latest version of this file can be found at https://github.com/gridion/gridion