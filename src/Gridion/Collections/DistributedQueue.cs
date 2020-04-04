// <copyright file="DistributedQueue.cs" company="Gridion">
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
    ///     Represents an internal implementation of <see cref="IDistributedQueue{T}" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items in a collection.
    /// </typeparam>
    /// <inheritdoc cref="IDistributedQueue{T}" />
    internal class DistributedQueue<T> : IDistributedQueue<T>
    {
        /// <summary>
        ///     The queue.
        /// </summary>
        private readonly Queue<T> queue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DistributedQueue{T}" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name of collection.
        /// </param>
        internal DistributedQueue(string name)
        {
            Should.NotBeNull(name, nameof(name));
            Should.BeSerializable(typeof(T), "T");

            this.Name = name;
            this.queue = new Queue<T>();
        }

        /// <inheritdoc />
        public int Count => this.queue.Count;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public T Dequeue()
        {
            return default;
        }

        /// <inheritdoc />
        public void Enqueue(T item)
        {
        }

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.queue.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.queue.GetEnumerator();
        }
    }
}