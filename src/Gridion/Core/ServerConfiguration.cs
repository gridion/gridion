// <copyright file="ServerConfiguration.cs" company="Gridion">
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

namespace Gridion.Core
{
    using System.Net;

    using Gridion.Internal.Logging;

    /// <summary>
    ///     Represents a configuration of <see cref="IGridionServer" /> instance.
    /// </summary>
    internal sealed class ServerConfiguration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ServerConfiguration" /> class.
        /// </summary>
        /// <param name="logger">
        ///     The logger to log internal operations.
        /// </param>
        /// <param name="host">
        ///     The IP address.
        /// </param>
        /// <param name="port">
        ///     The port.
        /// </param>
        /// <inheritdoc cref="ServerConfiguration" />
        internal ServerConfiguration(ILogger logger, string host, int port)
            : this(host, port)
        {
            Should.NotBeNull(logger, nameof(logger));

            this.Logger = logger;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServerConfiguration" /> class.
        /// </summary>
        /// <param name="host">
        ///     The IP address.
        /// </param>
        /// <param name="port">
        ///     The port.
        /// </param>
        private ServerConfiguration(string host, int port)
        {
            Should.NotBeNull(host, nameof(host));
            Should.BeTrue(IPAddress.TryParse(host, out _), nameof(host));
            Should.BePositive(port, nameof(port));

            this.Logger = new ConsoleLogger();
            this.Host = host;
            this.Port = port;
        }

        /// <summary>
        ///     Gets the full address of a host.
        /// </summary>
        internal string Address => this.Host + ":" + this.Port;

        /// <summary>
        ///     Gets the IP address.
        /// </summary>
        internal string Host { get; }

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        internal ILogger Logger { get; }

        /// <summary>
        ///     Gets the port.
        /// </summary>
        internal int Port { get; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{this.Host}:{this.Port}";
        }

        /// <summary>
        /// Validates the server configuration.
        /// </summary>
        internal void Validate()
        {
            Should.BeTrue(IPAddress.TryParse(this.Host, out _), nameof(this.Host));
            Should.BePositive(this.Port, nameof(this.Port));
        }
    }
}