// <copyright file="GridionCollection.cs" company="Gridion">
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
    using System.Collections.Generic;
    using Gridion.Core.Utils;

    /// <summary>
    ///     Represents a dictionary of <see cref="IGridion" /> items.
    /// </summary>
    /// <inheritdoc cref="Dictionary{TKey,TValue}" />
    internal class GridionCollection : Dictionary<GridionCollection.GridionCollectionKey, IGridion>
    {
        /// <summary>
        ///     Add an <see cref="IGridion" /> into the collection.
        /// </summary>
        /// <param name="gridion">
        ///     The item to add.
        /// </param>
        internal void Add(IGridion gridion)
        {
            Should.NotBeNull(gridion, nameof(gridion));
            Should.NotBeNull(gridion.Name, nameof(gridion.Name));

            var key = BuildKey(gridion.Name);
            this.Add(key, gridion);
        }

        /// <summary>
        ///     Clears the collection with disposing of each of items.
        /// </summary>
        /// <returns>
        ///     a number of disposed objects.
        /// </returns>
        internal int ClearAll()
        {
            var cnt = this.Values.Count;
            foreach (var value in this.Values)
            {
                value.Dispose();
            }

            this.Clear();

            return cnt;
        }

        /// <summary>
        ///     Tries to remove the item by its name.
        /// </summary>
        /// <param name="name">
        ///     The name of item.
        /// </param>
        internal void Remove(string name)
        {
            Should.NotBeNull(name, nameof(name));

            var key = BuildKey(name);
            this.Remove(key);
        }

        /// <summary>
        ///     Creates an array of <see cref="IGridion" />s located in the collection.
        /// </summary>
        /// <returns>
        ///     an array of <see cref="IGridion" />s.
        /// </returns>
        internal IGridion[] ToArray()
        {
            var list = new IGridion[this.Count];

            var i = 0;
            foreach (var pair in this.Values)
            {
                list[i++] = pair;
            }

            return list;
        }

        /// <summary>
        ///     Tries to get the item by its name.
        /// </summary>
        /// <param name="name">
        ///     The name of item.
        /// </param>
        /// <param name="val">
        ///     The resulting <see cref="IGridion" /> item.
        /// </param>
        /// <returns>
        ///     true when the value has been found; otherwise returns false.
        /// </returns>
        internal bool TryGetByName(string name, out IGridion val)
        {
            Should.NotBeNull(name, nameof(name));

            var key = BuildKey(name);
            return this.TryGetValue(key, out val);
        }

        /// <summary>
        ///     Build a collection key.
        /// </summary>
        /// <param name="key">
        ///     A key to build on.
        /// </param>
        /// <returns>
        ///     a key for <see cref="GridionCollection" />.
        /// </returns>
        private static GridionCollectionKey BuildKey(string key)
        {
            return new GridionCollectionKey(key ?? string.Empty);
        }

        /// <summary>
        ///     Represents a key wrapper for <see cref="GridionCollection" />.
        /// </summary>
        /// <inheritdoc cref="IEquatable{T}" />
        internal class GridionCollectionKey : IEquatable<GridionCollectionKey>
        {
            /// <summary>
            ///     The name of <see cref="IGridion" /> instance.
            /// </summary>
            private readonly string key;

            /// <summary>
            ///     Initializes a new instance of the <see cref="GridionCollectionKey" /> class.
            /// </summary>
            /// <param name="key">
            ///     The key of item.
            /// </param>
            internal GridionCollectionKey(string key)
            {
                this.key = key ?? string.Empty;
            }

            /// <inheritdoc />
            public bool Equals(GridionCollectionKey other)
            {
                if (other is null)
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.key == other.key;
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

                return obj.GetType() == this.GetType() && this.Equals((GridionCollectionKey)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return HashCode.Combine(this.key);
            }
        }
    }
}