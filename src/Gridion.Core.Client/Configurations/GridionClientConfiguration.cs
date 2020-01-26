// <copyright file="GridionClientConfiguration.cs" company="Gridion">
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

namespace Gridion.Core.Client.Configurations
{
    using Gridion.Core.Client.Validators;

    /// <summary>
    ///     Represents a configuration of <see cref="GridionClient" />.
    /// </summary>
    internal class GridionClientConfiguration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GridionClientConfiguration" /> class.
        /// </summary>
        /// <param name="host">
        ///     The IP address.
        /// </param>
        /// <param name="port">
        ///     The port on the address.
        /// </param>
        internal GridionClientConfiguration(string host, int port)
        {
            HostValidator.Validate(host, port);

            this.Host = host;
            this.Port = port;
        }

        /// <summary>
        ///     Gets the IP address.
        /// </summary>
        internal string Host { get; }

        /// <summary>
        ///     Gets the port.
        /// </summary>
        internal int Port { get; }
    }
}