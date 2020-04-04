// <copyright file="GridionEndpoint.cs" company="Gridion">
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

namespace Gridion.Core.Configurations
{
    using System.Net;

    using Gridion.Core.Validators;

    /// <summary>
    ///     Represents a endpoint wrapper.
    /// </summary>
    /// <inheritdoc />
    internal class GridionEndpoint : IPEndPoint
    {
        /// <inheritdoc cref="IPEndPoint" />
        private GridionEndpoint(string host, int port)
            : base(IPAddress.Parse(host), port)
        {
            HostValidator.Validate(host, port);

            this.Host = host;
            this.Port = port;
        }

        /// <summary>
        ///     Gets a host.
        /// </summary>
        internal string Host { get; private set; }

        /// <summary>
        ///     Gets a default endpoint.
        /// </summary>
        /// <returns>
        ///     an initialized default endpoint.
        /// </returns>
        internal static GridionEndpoint GetDefault()
        {
            return new GridionEndpoint("127.0.0.1", 24000);
        }
    }
}