// <copyright file="GridionServer.cs" company="Gridion">
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
    using System;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    using Gridion.Core.Configurations;
    using Gridion.Core.Extensions;
    using Gridion.Core.Interfaces.Internals;
    using Gridion.Core.Logging;
    using Gridion.Core.Properties;
    using Gridion.Core.Services;

    /// <summary>
    ///     Represents an internal <see cref="IGridionServer" /> implementation.
    /// </summary>
    /// <inheritdoc cref="Disposable" />
    /// <inheritdoc cref="IGridionServer" />
    internal sealed class GridionServer : Disposable, IGridionServer
    {
        /// <summary>
        ///     The <see cref="GridionServer" /> ID.
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
        ///     Initializes a new instance of the <see cref="GridionServer" /> class.
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
        internal GridionServer(GridionServerId id, TcpListener listener, GridionServerConfiguration configuration)
        {
            this.id = id;
            this.listener = listener;
            this.Configuration = configuration;
            this.logger = this.Configuration.Logger;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="Gridion.Core.Server.GridionServer" /> class.
        /// </summary>
        /// <inheritdoc cref="TcpListener" />
        ~GridionServer()
        {
            this.DisposeUnmanaged();
        }

        /// <inheritdoc />
        public bool IsListening { get; private set; }
        
        /// <inheritdoc />
        public GridionServerConfiguration Configuration { get; }

        /// <inheritdoc />
        GridionServerId IGridionServer.Id => this.id;

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
        ///     Dispose the internal managed/unmanaged resources.
        /// </summary>
        /// <inheritdoc />
        protected override void DisposeManaged()
        {
            GridionServerFactory.UnRegister(this);
            this.listener.Stop();
            this.IsListening = false;
        }
    }
}