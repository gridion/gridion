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
    using Gridion.Core.Utils;

    /// <summary>
    ///     Represents an internal implementation of <see cref="IDistributedList{T}" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in a collection.
    /// </typeparam>
    /// <inheritdoc cref="IDistributedList{T}" />
    internal class DistributedList<T> : IDistributedList<T>
    {
        /// <summary>
        ///     The list.
        /// </summary>
        private readonly List<T> list;

        /// <summary>
        ///    Initializes a new instance of the <see cref="DistributedList{T}" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name of collection.
        /// </param>
        internal DistributedList(string name)
        {
            Should.NotBeNull(name, nameof(name));
            Should.BeSerializable(typeof(T), "T");

            this.Name = name;
            this.list = new List<T>();
        }

        /// <inheritdoc />
        public int Count => this.list.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public T this[int index]
        {
            get => this.list[index];
            set => this.list[index] = value;
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            this.list.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.list.Clear();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return this.list.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return this.list.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            this.list.Insert(index, item);
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            return this.list.Remove(item);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}