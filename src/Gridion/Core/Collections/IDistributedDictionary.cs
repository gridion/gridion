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
    using System.Diagnostics.CodeAnalysis;

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
    [SuppressMessage("NDepend", "ND1200:AvoidInterfacesTooBig", Justification = "By design.")]
    public interface IDistributedDictionary<TKey, TValue> : IDistributedCollection
        where TKey : notnull
    {
        /// <summary>
        ///     Gets the number of key/value pairs contained in the <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="IDistributedDictionary{TKey,TValue}" /> is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        ///     Gets a collection containing the keys in the <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        ICollection<TKey> Keys { get; }

        /// <summary>
        ///     Gets a collection that contains the values in the <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        ICollection<TValue> Values { get; }

        /// <summary>
        ///     Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value of the key/value pair at the specified index.</returns>
        TValue this[TKey key] { get; set; }

        /// <summary>
        ///     Uses the specified functions to add a key/value pair to the <see cref="IDistributedDictionary{TKey,TValue}" /> if
        ///     the key does not already exist, or to update a key/value pair in the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" /> if the key already exists.
        /// </summary>
        /// <param name="key">The key to be added or whose value should be updated.</param>
        /// <param name="addValueFactory">The function used to generate a value for an absent key.</param>
        /// <param name="updateValueFactory">
        ///     The function used to generate a new value for an existing key based on the key's
        ///     existing value.
        /// </param>
        /// <returns>
        ///     The new value for the key. This will be either be the result of addValueFactory (if the key was absent) or the
        ///     result of updateValueFactory (if the key was present).
        /// </returns>
        TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory);

        /// <summary>
        ///     Adds a key/value pair to the <see cref="IDistributedDictionary{TKey,TValue}" /> if the key does not already exist,
        ///     or updates a key/value pair in the <see cref="IDistributedDictionary{TKey,TValue}" /> by using the specified
        ///     function if the key already exists.
        /// </summary>
        /// <param name="key">The key to be added or whose value should be updated.</param>
        /// <param name="value">The value to be added for an absent key.</param>
        /// <param name="updateValueFactory">
        ///     The function used to generate a new value for an existing key based on the key's
        ///     existing value.
        /// </param>
        /// <returns>
        ///     The new value for the key. This will be either be addValue (if the key was absent) or the result of
        ///     updateValueFactory (if the key was present).
        /// </returns>
        TValue AddOrUpdate(TKey key, TValue value, Func<TKey, TValue, TValue> updateValueFactory);

        /// <summary>
        ///     Uses the specified functions and argument to add a key/value pair to the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" /> if the key does not already exist, or to update a key/value pair
        ///     in the <see cref="IDistributedDictionary{TKey,TValue}" /> if the key already exists.
        /// </summary>
        /// <typeparam name="TArgument">The type of an argument to pass into addValueFactory and updateValueFactory.</typeparam>
        /// <param name="key">The key to be added or whose value should be updated.</param>
        /// <param name="addValueFactory">The function used to generate a value for an absent key.</param>
        /// <param name="updateValueFactory">
        ///     The function used to generate a new value for an existing key based on the key's
        ///     existing value.
        /// </param>
        /// <param name="factoryArgument">An argument to pass into addValueFactory and updateValueFactory.</param>
        /// <returns>
        ///     The new value for the key. This will be either be the result of addValueFactory (if the key was absent) or the
        ///     result of updateValueFactory (if the key was present).
        /// </returns>
        TValue AddOrUpdate<TArgument>(
            TKey key,
            Func<TKey, TArgument, TValue> addValueFactory,
            Func<TKey, TValue, TArgument, TValue> updateValueFactory,
            TArgument factoryArgument);

        /// <summary>
        ///     Clears the <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Checks whether the <see cref="IDistributedDictionary{TKey,TValue}" /> contains the key.
        /// </summary>
        /// <param name="key">The key to check against to.</param>
        /// <returns>true if the dictionary contains the key; false otherwise.</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        ///     Returns an enumerator of the <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        /// <returns>the enumerator of the dictionary.</returns>
        IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        /// <summary>
        ///     Adds a key/value pair to the <see cref="IDistributedDictionary{TKey,TValue}" /> by using the specified function if
        ///     the key does not already exist. Returns the new value, or the existing value if the key exists.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key.</param>
        /// <returns>
        ///     the value for the key. This will be either the existing value for the key if the key is already in the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" />, or the new value if the key was not in the dictionary.
        /// </returns>
        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);

        /// <summary>
        ///     Adds a key/value pair to the <see cref="IDistributedDictionary{TKey,TValue}" /> if the key does not already exist.
        ///     Returns the new value, or the existing value if the key exists.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value to be added, if the key does not already exist.</param>
        /// <returns>
        ///     The value for the key. This will be either the existing value for the key if the key is already in the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" />, or the new value if the key was not in the dictionary.
        /// </returns>
        TValue GetOrAdd(TKey key, TValue value);

        /// <summary>
        ///     Adds a key/value pair to the <see cref="IDistributedDictionary{TKey,TValue}" /> by using the specified function and
        ///     an argument if the key does not already exist, or returns the existing value if the key exists.
        /// </summary>
        /// <typeparam name="TArgument">The type of an argument to pass into valueFactory.</typeparam>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key.</param>
        /// <param name="factoryArgument">An argument value to pass into valueFactory.</param>
        /// <returns>
        ///     The value for the key. This will be either the existing value for the key if the key is already in the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" />, or the new value if the key was not in the dictionary.
        /// </returns>
        TValue GetOrAdd<TArgument>(TKey key, Func<TKey, TArgument, TValue> valueFactory, TArgument factoryArgument);

        /// <summary>
        ///     Copies the key and value pairs stored in the <see cref="IDistributedDictionary{TKey,TValue}" /> to a new array.
        /// </summary>
        /// <returns>
        ///     A new array containing a snapshot of key and value pairs copied from the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </returns>
        KeyValuePair<TKey, TValue>[] ToArray();

        /// <summary>
        ///     Attempts to add the specified key and value to the <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <returns>
        ///     true if the key/value pair was added to the <see cref="IDistributedDictionary{TKey,TValue}" /> successfully;
        ///     false if the key already exists.
        /// </returns>
        bool TryAdd(TKey key, TValue value);

        /// <summary>
        ///     Attempts to get the value associated with the specified key from the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        ///     When this method returns, contains the object from the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" /> that has the specified key, or the default value of the type if
        ///     the operation failed.
        /// </param>
        /// <returns>true if the key was found in the <see cref="IDistributedDictionary{TKey,TValue}" />; otherwise, false.</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        ///     Attempts to remove and return the value that has the specified key from the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        /// <param name="key">The key of the element to remove and return.</param>
        /// <param name="value">
        ///     When this method returns, contains the object removed from the
        ///     <see cref="IDistributedDictionary{TKey,TValue}" />, or the default value of the TValue type if key does not exist.
        /// </param>
        /// <returns>true if the object was removed successfully; otherwise, false.</returns>
        bool TryRemove(TKey key, out TValue value);

        /// <summary>
        ///     Updates the value associated with key to newValue if the existing value with key is equal to comparisonValue.
        /// </summary>
        /// <param name="key">The key of the value that is compared with comparisonValue and possibly replaced.</param>
        /// <param name="newValue">
        ///     The value that replaces the value of the element that has the specified key if the comparison
        ///     results in equality.
        /// </param>
        /// <param name="comparisonValue">The value that is compared with the value of the element that has the specified key.</param>
        /// <returns>true if the value with key was equal to comparisonValue and was replaced with newValue; otherwise, false.</returns>
        bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue);
    }
}