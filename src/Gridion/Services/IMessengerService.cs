// <copyright file="IMessengerService.cs" company="Gridion">
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

namespace Gridion.Services
{
    using System.Collections.Generic;

    using Gridion.Services.Messages;

    /// <summary>
    ///     Represents a data provider to send and receive raw data.
    /// </summary>
    /// <inheritdoc cref="IGridionService"/>
    internal interface IMessengerService : IGridionService
    {
        /// <summary>Sends the message to all nodes.</summary>
        /// <param name="recipients">The recipients.</param>
        /// <param name="message">The message to send.</param>
        void Send(IEnumerable<IRecipient> recipients, IMessage message);
    }
}