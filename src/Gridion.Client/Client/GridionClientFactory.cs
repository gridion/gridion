// <copyright file="GridionClientFactory.cs" company="Gridion">
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

namespace Gridion.Client
{
    using Gridion.Common;

    /// <summary>
    ///     The client factory.
    /// </summary>
    public static class GridionClientFactory
    {
        /// <summary>
        ///     Creates a new instance of client.
        /// </summary>
        /// <returns>
        ///     an initialized client.
        /// </returns>
        public static IGridionClient Create()
        {
            return new GridionClient();
        }

        /// <summary>
        ///     Represents an internal <see cref="Gridion.Client.IGridionClient" /> implementation.
        /// </summary>
        /// <inheritdoc />
        private sealed class GridionClient : IGridionClient
        {
            /// <inheritdoc />
            public bool IsConnected { get; private set; }

            /// <inheritdoc />
            void IGridionClient.Connect(Configuration configuration)
            {
                Should.NotBeNull(configuration, nameof(configuration));
                configuration.Validate();
                this.IsConnected = true;
            }
        }
    }
}