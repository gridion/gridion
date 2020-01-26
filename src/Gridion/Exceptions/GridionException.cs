// <copyright file="GridionException.cs" company="Gridion">
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
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     Represents of any exception that <inheritdoc cref="IGridion" /> may cause.
    /// </summary>
    /// <inheritdoc />
    [Serializable]
    public class GridionException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GridionException" /> class.
        /// </summary>
        /// <inheritdoc cref="Exception" />
        public GridionException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GridionException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <inheritdoc cref="Exception" />
        public GridionException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GridionException" /> class.
        /// </summary>
        /// <param name="message">The message of exception.</param>
        /// <param name="cause">The cause of exception.</param>
        /// <inheritdoc cref="Exception" />
        public GridionException(string message, Exception cause)
            : base(message, cause)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GridionException" /> class.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="ctx">The streaming context.</param>
        /// <inheritdoc cref="Exception" />
        protected GridionException(SerializationInfo info, StreamingContext ctx)
            : base(info, ctx)
        {
        }
    }
}