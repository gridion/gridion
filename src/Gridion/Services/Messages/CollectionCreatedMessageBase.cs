﻿// <copyright file="CollectionCreatedMessageBase.cs" company="Gridion">
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

namespace Gridion.Services.Messages
{
    using Gridion.Core.Collections;

    /// <summary>
    ///  Represents a message that indicating that collection was created on the sender.
    /// </summary>
    /// <inheritdoc cref="MessageBase"/>
    internal abstract class CollectionCreatedMessageBase : MessageBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Gridion.Services.Messages.CollectionCreatedMessageBase" /> class.
        /// </summary>
        /// <param name="sender">The message sender.</param>
        /// <inheritdoc />
        protected CollectionCreatedMessageBase(ISender sender) 
            : base(sender)
        {
        }

        /// <summary>Creates a collection.</summary>
        /// <returns>the created collection.</returns>
        internal abstract IDistributedCollection Create();
    }
}