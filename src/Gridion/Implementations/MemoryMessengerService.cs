// <copyright file="MemoryMessengerService.cs" company="Gridion">
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

namespace Gridion.Core.Services
{
    using System;
    using System.Collections.Generic;

    using Gridion.Core.Interfaces.Internals;
    using Gridion.Core.Messages.Interfaces;

    /// <summary>
    ///     Represents a service which works with in messages.
    /// </summary>
    /// <inheritdoc />
    internal class MemoryMessengerService : MessengerService
    {
        /// <summary>
        /// The cluster curator.
        /// </summary>
        private readonly IClusterCurator curator;

        /// <inheritdoc cref="GridionService" />
        public MemoryMessengerService(IClusterCurator curator)
            : base("MemoryMessengerService")
        {
            this.curator = curator;
        }

        /// <inheritdoc />
        public override IMessage Accept()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void SendToAll(IMessage message)
        {
            IEnumerable<INodeInternal> nodeInternals = this.curator.GetNodes();
            foreach (var node in nodeInternals)
            {
                if (!node.Equals(message.Sender))
                {
                    node.Accept(message);
                }
            }
        }
    }
}