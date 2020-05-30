// <copyright file="IDistributedCollectionFactory.cs" company="Gridion">
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
    /// <summary>
    ///     Represents an interface of collection factory.
    /// </summary>
    internal interface IDistributedCollectionFactory
    {
        /// <summary>
        ///     Creates an instance of <see cref="IDistributedDictionary{TKey,TValue}" /> interface.
        /// </summary>
        /// <param name="name">
        ///     The collection name.
        /// </param>
        /// <typeparam name="TKey">
        ///     The type of key.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of value.
        /// </typeparam>
        /// <returns>
        ///     an initialized named distributed <see cref="IDistributedDictionary{TKey,TValue}" /> instance.
        /// </returns>
        IDistributedDictionary<TKey, TValue> GetOrCreateDictionary<TKey, TValue>(string name);

        /// <summary>
        ///     Creates an instance of <see cref="IDistributedList{T}" /> interface.
        /// </summary>
        /// <param name="name">
        ///     The collection name.
        /// </param>
        /// <typeparam name="T">
        ///     The type of collection items.
        /// </typeparam>
        /// <returns>
        ///     an initialized named distributed <see cref="IDistributedList{T}" /> instance.
        /// </returns>
        IDistributedList<T> GetOrCreateList<T>(string name);

        /// <summary>
        ///     Creates an instance of <see cref="IDistributedQueue{T}" /> interface.
        /// </summary>
        /// <param name="name">
        ///     The collection name.
        /// </param>
        /// <typeparam name="T">
        ///     The type of collection items.
        /// </typeparam>
        /// <returns>
        ///     an initialized named distributed <see cref="IDistributedQueue{T}" /> instance.
        /// </returns>
        IDistributedQueue<T> GetOrCreateQueue<T>(string name);

        /// <summary>
        ///     Creates an instance of <see cref="IDistributedSet{T}" /> interface.
        /// </summary>
        /// <param name="name">
        ///     The collection name.
        /// </param>
        /// <typeparam name="T">
        ///     The type of collection items.
        /// </typeparam>
        /// <returns>
        ///     an initialized named distributed <see cref="IDistributedSet{T}" /> instance.
        /// </returns>
        IDistributedSet<T> GetOrCreateSet<T>(string name);
    }
}