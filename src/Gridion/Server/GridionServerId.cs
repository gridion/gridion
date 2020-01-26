// <copyright file="GridionServerId.cs" company="Gridion">
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
    using Gridion.Core.Utils;

    /// <summary>
    ///     A wrapper for <see cref="IGridionServer" /> ID.
    /// </summary>
    /// <inheritdoc cref="IEquatable{T}" />
    internal class GridionServerId : IEquatable<GridionServerId>
    {
        /// <summary>
        ///     The ID of <see cref="IGridionServer" />.
        /// </summary>
        private readonly string id;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GridionServerId" /> class.
        /// </summary>
        /// <param name="id">
        ///     The ID to initialize on.
        /// </param>
        internal GridionServerId(string id)
        {
            Should.NotBeNull(id, nameof(id));

            this.id = id;
        }

        /// <inheritdoc />
        public bool Equals(GridionServerId other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.id == other.id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((GridionServerId)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.id);
        }
    }
}