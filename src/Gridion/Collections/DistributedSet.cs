// <copyright file="DistributedSet.cs" company="Gridion">
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

namespace Gridion.Core.Collections
{
    using System.Collections;
    using System.Collections.Generic;
    using Gridion.Core.Utils;

    /// <summary>
    ///     Represents an internal implementation of <see cref="IDistributedSet{T}" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in a collection.
    /// </typeparam>
    /// <inheritdoc cref="IDistributedSet{T}" />
    internal class DistributedSet<T> : IDistributedSet<T>
    {
        /// <summary>
        ///     The set.
        /// </summary>
        private readonly ISet<T> set;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DistributedSet{T}"/> class.
        /// </summary>
        /// <param name="name">
        ///     The name of collection.
        /// </param>
        internal DistributedSet(string name)
        {
            Should.NotBeNull(name, nameof(name));
            Should.BeSerializable(typeof(T), "T");

            this.Name = name;
            this.set = new HashSet<T>();
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        int ICollection<T>.Count => this.set.Count;

        /// <inheritdoc />
        bool ICollection<T>.IsReadOnly => this.set.IsReadOnly;

        /// <inheritdoc />
        void ICollection<T>.Add(T item)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            this.set.Add(item);
        }

        /// <inheritdoc />
        bool ISet<T>.Add(T item)
        {
            return this.set.Add(item);
        }

        /// <inheritdoc />
        void ICollection<T>.Clear()
        {
            this.set.Clear();
        }

        /// <inheritdoc />
        bool ICollection<T>.Contains(T item)
        {
            return this.set.Contains(item);
        }

        /// <inheritdoc />
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.set.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            this.set.ExceptWith(other);
        }

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.set.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.set.GetEnumerator();
        }

        /// <inheritdoc />
        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            this.set.IntersectWith(other);
        }

        /// <inheritdoc />
        bool ISet<T>.IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.set.IsProperSubsetOf(other);
        }

        /// <inheritdoc />
        bool ISet<T>.IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.set.IsProperSupersetOf(other);
        }

        /// <inheritdoc />
        bool ISet<T>.IsSubsetOf(IEnumerable<T> other)
        {
            return this.set.IsSubsetOf(other);
        }

        /// <inheritdoc />
        bool ISet<T>.IsSupersetOf(IEnumerable<T> other)
        {
            return this.set.IsSupersetOf(other);
        }

        /// <inheritdoc />
        bool ISet<T>.Overlaps(IEnumerable<T> other)
        {
            return this.set.Overlaps(other);
        }

        /// <inheritdoc />
        bool ICollection<T>.Remove(T item)
        {
            return this.set.Remove(item);
        }

        /// <inheritdoc />
        bool ISet<T>.SetEquals(IEnumerable<T> other)
        {
            return this.set.SetEquals(other);
        }

        /// <inheritdoc />
        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            this.set.SymmetricExceptWith(other);
        }

        /// <inheritdoc />
        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            this.set.UnionWith(other);
        }
    }
}