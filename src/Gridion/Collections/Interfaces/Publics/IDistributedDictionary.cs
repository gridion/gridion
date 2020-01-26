// <copyright file="IDistributedDictionary.cs" company="Gridion">
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
    using System.Collections.Generic;

    /// <summary>
    ///     Represents a partitioned implementation of a dictionary of objects.
    /// </summary>
    /// <typeparam name="TKey">
    ///     The type of item key.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     The type of value.
    /// </typeparam>
    /// <inheritdoc cref="IDistributedCollection" />
    public interface IDistributedDictionary<TKey, TValue> :
        IDistributedCollection
        where TKey : notnull
    {
        int Count { get; }

        bool IsEmpty { get; }

        ICollection<TKey> Keys { get; }

        ICollection<TValue> Values { get; }

        TValue this[TKey key] { get; set; }

        TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory);

        TValue AddOrUpdate(TKey key, TValue value, Func<TKey, TValue, TValue> updateValueFactory);

        TValue AddOrUpdate<TArgument>(
            TKey key,
            Func<TKey, TArgument, TValue> addValueFactory,
            Func<TKey, TValue, TArgument, TValue> updateValueFactory,
            TArgument factoryArgument);

        void Clear();

        bool ContainsKey(TKey key);

        IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);

        TValue GetOrAdd(TKey key, TValue value);

        TValue GetOrAdd<TArgument>(TKey key, Func<TKey, TArgument, TValue> valueFactory, TArgument factoryArgument);

        KeyValuePair<TKey, TValue>[] ToArray();

        bool TryAdd(TKey key, TValue value);

        bool TryGetValue(TKey key, out TValue value);

        bool TryRemove(TKey key, out TValue value);

        bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue);
    }
}