// <copyright file="DistributedList.cs" company="Gridion">
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

    /// <summary>
    ///     Represents an internal implementation of <see cref="IDistributedList{T}" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in a collection.
    /// </typeparam>
    /// <inheritdoc cref="IDistributedList{T}" />
    internal sealed class DistributedList<T> : IDistributedList<T>
    {
        /// <summary>
        ///     The list.
        /// </summary>
        private readonly List<T> items;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DistributedList{T}" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name of the collection.
        /// </param>
        internal DistributedList(string name)
        {
            Should.NotBeNull(name, nameof(name));
            Should.BeSerializable(typeof(T), "T");

            this.Name = name;
            this.items = new List<T>();
        }

        /// <inheritdoc />
        public int Count => this.items.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public T this[int index]
        {
            get => this.items[index];
            set => this.items[index] = value;
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            this.items.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.items.Clear();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return this.items.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            this.items.Insert(index, item);
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            return this.items.Remove(item);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            this.items.RemoveAt(index);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}