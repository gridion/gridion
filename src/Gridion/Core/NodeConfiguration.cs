﻿// <copyright file="NodeConfiguration.cs" company="Gridion">
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
    using System.Globalization;

    using Gridion.Internal.Logging;

    /// <summary>
    ///     Represents a configuration of <see cref="IGridion" /> instance.
    /// </summary>
    public class NodeConfiguration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NodeConfiguration" /> class.
        /// </summary>
        /// <param name="nodeName">
        ///     The name of node.
        /// </param>
        /// <param name="host">
        ///     The IP address.
        /// </param>
        /// <param name="port">
        ///     The port.
        /// </param>
        public NodeConfiguration(string nodeName, string host, int port)
        {
            this.NodeName = nodeName;
            this.ServerConfiguration = new ServerConfiguration(new ConsoleLogger(), host, port);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Gridion.Core.NodeConfiguration" /> class.
        /// </summary>
        /// <param name="host">
        ///     The IP address.
        /// </param>
        /// <param name="port">
        ///     The port.
        /// </param>
        public NodeConfiguration(string host, int port)
            : this(BuildFullNodeName(), host, port)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NodeConfiguration" /> class.
        /// </summary>
        /// <param name="nodeName">
        ///     The name of node.
        /// </param>
        /// <inheritdoc cref="ServerConfiguration" />
        public NodeConfiguration(string nodeName)
            : this(nodeName, Endpoint.GetDefault().Host, Endpoint.GetDefault().Port)
        {
        }

        /// <summary>
        ///     Gets the node name.
        /// </summary>
        internal string NodeName { get; private set; }

        /// <summary>
        ///     Gets the server configuration.
        /// </summary>
        internal ServerConfiguration ServerConfiguration { get; private set; }

        /// <summary>
        ///     Gets the default configuration.
        /// </summary>
        /// <returns>
        ///     a default <see cref="NodeConfiguration" /> instance.
        /// </returns>
        internal static NodeConfiguration GetDefaultConfiguration()
        {
            var nodeName = BuildFullNodeName();
            var configuration = new NodeConfiguration(nodeName);
            return configuration;
        }

        /// <summary>
        ///     Validates <see cref="ServerConfiguration" /> instance.
        /// </summary>
        internal void Validate()
        {
            Should.NotBeNull(this.NodeName, nameof(this.NodeName));
            Should.NotBeNull(this.ServerConfiguration, nameof(this.ServerConfiguration));
            this.ServerConfiguration.Validate();
        }

        /// <summary>
        ///     Builds the full node name by using default prefix and random GUID.
        /// </summary>
        /// <returns>
        ///     a full node name.
        /// </returns>
        private static string BuildFullNodeName()
        {
            return GetDefaultNamePrefix() + Guid.NewGuid().ToString("B", CultureInfo.CurrentCulture).ToUpperInvariant();
        }

        /// <summary>
        ///     Gets the default prefix of a node name.
        /// </summary>
        /// <returns>
        ///     the default prefix.
        /// </returns>
        private static string GetDefaultNamePrefix()
        {
            return "[Gridion] Node - ";
        }
    }
}