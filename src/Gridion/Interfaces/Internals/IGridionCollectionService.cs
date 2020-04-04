// <copyright file="IGridionCollectionService.cs" company="Gridion">
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

namespace Gridion.Core.Interfaces.Internals
{
    using System.Threading.Tasks;

    using Gridion.Core.Collections;

    /// <summary>
    ///     Represents a service that creates distributed collections.
    /// </summary>
    /// <inheritdoc />
    internal interface IGridionCollectionService : IGridionService
    {
        /// <summary>
        ///     Creates a distributed dictionary in async manner.
        /// </summary>
        /// <param name="name">
        ///     The name of collection.
        /// </param>
        /// <typeparam name="TKey">
        ///     The type of a key.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of a value.
        /// </typeparam>
        /// <returns>
        ///     a created instance of distributed dictionary.
        /// </returns>
        Task<IDistributedDictionary<TKey, TValue>> CreateDictionaryAsync<TKey, TValue>(string name);

        /// <summary>
        ///     Creates a distributed list in async manner.
        /// </summary>
        /// <param name="name">
        ///     The name of collection.
        /// </param>
        /// <typeparam name="T">
        ///     The type of an item.
        /// </typeparam>
        /// <returns>
        ///     a created instance of distributed list.
        /// </returns>
        Task<IDistributedList<T>> CreateListAsync<T>(string name);

        /// <summary>
        ///     Creates a distributed queue in async manner.
        /// </summary>
        /// <param name="name">
        ///     The name of collection.
        /// </param>
        /// <typeparam name="T">
        ///     The type of an item.
        /// </typeparam>
        /// <returns>
        ///     a created instance of distributed queue.
        /// </returns>
        Task<IDistributedQueue<T>> CreateQueueAsync<T>(string name);

        /// <summary>
        ///     Creates a distributed set in async manner.
        /// </summary>
        /// <param name="name">
        ///     The name of collection.
        /// </param>
        /// <typeparam name="T">
        ///     The type of an item.
        /// </typeparam>
        /// <returns>
        ///     a created instance of distributed set.
        /// </returns>
        Task<IDistributedSet<T>> CreateSetAsync<T>(string name);
    }
}