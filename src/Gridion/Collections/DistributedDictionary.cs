// <copyright file="DistributedDictionary.cs" company="Gridion">
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
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Gridion.Core.Utils;

    /// <summary>
    ///     Represents an internal implementation of <see cref="IDistributedDictionary{TKey,TValue}" />.
    /// </summary>
    /// <typeparam name="TKey">
    ///     The type of key.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The type of value.
    /// </typeparam>
    /// <inheritdoc cref="IDistributedDictionary{TKey,TValue}" />
    internal class DistributedDictionary<TKey, TValue> : IDistributedDictionary<TKey, TValue>
    {
        /// <summary>
        ///     The dictionary.
        /// </summary>
        private readonly ConcurrentDictionary<TKey, TValue> dictionary;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DistributedDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="name">
        ///     The name of collection.
        /// </param>
        internal DistributedDictionary(string name)
        {
            Should.NotBeNull(name, nameof(name));
            Should.BeSerializable(typeof(TKey), "TKey");
            Should.BeSerializable(typeof(TValue), "TKey");

            this.Name = name;
            this.dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        /// <inheritdoc />
        public int Count => this.dictionary.Count;

        /// <inheritdoc />
        public bool IsEmpty => this.dictionary.IsEmpty;

        /// <inheritdoc />
        public ICollection<TKey> Keys => this.dictionary.Keys;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ICollection<TValue> Values => this.dictionary.Values;

        /// <inheritdoc />
        public TValue this[TKey key]
        {
            get => this.dictionary[key];

            set => this.dictionary[key] = value;
        }

        /// <inheritdoc />
        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            return this.dictionary.AddOrUpdate(key, addValueFactory, updateValueFactory);
        }

        /// <inheritdoc />
        public TValue AddOrUpdate(TKey key, TValue value, Func<TKey, TValue, TValue> updateValueFactory)
        {
            return this.dictionary.AddOrUpdate(key, value, updateValueFactory);
        }

        /// <inheritdoc />
        public TValue AddOrUpdate<TArgument>(
            TKey key,
            Func<TKey, TArgument, TValue> addValueFactory,
            Func<TKey, TValue, TArgument, TValue> updateValueFactory,
            TArgument factoryArgument)
        {
            return this.dictionary.AddOrUpdate(key, addValueFactory, updateValueFactory, factoryArgument);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.dictionary.Clear();
        }

        /// <inheritdoc cref="IDictionary{T,V}" />
        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        /// <inheritdoc />
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            return this.dictionary.GetOrAdd(key, valueFactory);
        }

        /// <inheritdoc />
        public TValue GetOrAdd(TKey key, TValue value)
        {
            return this.dictionary.GetOrAdd(key, value);
        }

        /// <inheritdoc />
        public TValue GetOrAdd<TArgument>(TKey key, Func<TKey, TArgument, TValue> valueFactory, TArgument factoryArgument)
        {
            return this.dictionary.GetOrAdd(key, valueFactory, factoryArgument);
        }

        /// <inheritdoc />
        public KeyValuePair<TKey, TValue>[] ToArray()
        {
            return this.dictionary.ToArray();
        }

        /// <inheritdoc />
        public bool TryAdd(TKey key, TValue value)
        {
            return this.dictionary.TryAdd(key, value);
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc />
        public bool TryRemove(TKey key, out TValue value)
        {
            return this.dictionary.TryRemove(key, out value);
        }

        /// <inheritdoc />
        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
        {
            return this.dictionary.TryUpdate(key, newValue, comparisonValue);
        }
    }
}