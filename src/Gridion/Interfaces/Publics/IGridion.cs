// <copyright file="IGridion.cs" company="Gridion">
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

    using Gridion.Core.Collections;

    /// <summary>
    ///     Represents a node in a cluster.
    /// </summary>
    /// <inheritdoc cref="IDisposable" />
    public interface IGridion : IDisposable
    {
        /// <summary>
        ///     Gets the cluster that the <see cref="IGridion" /> node is belong to.
        /// </summary>
        /// <returns>
        ///     the cluster that the <see cref="IGridion" /> node is belong to.
        /// </returns>
        ICluster Cluster { get; }

        /// <summary>
        ///     Gets the name of <see cref="IGridion" /> instance.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Creates a partitioned version <see cref="IDistributedDictionary{TKey,TValue}" />.
        /// </summary>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <param name="name">
        ///     The collection name.
        /// </param>
        /// <returns>a created <see cref="IDistributedDictionary{TKey,TValue}" /> instance.</returns>
        IDistributedDictionary<TKey, TValue> GetDictionary<TKey, TValue>(string name);

        /// <summary>
        ///     Creates a partitioned version <see cref="IDistributedList{T}" />.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of item.
        /// </typeparam>
        /// <param name="name">
        ///     The collection name.
        /// </param>
        /// <returns>
        ///     a created <see cref="IDistributedList{T}" /> instance.
        /// </returns>
        IDistributedList<T> GetList<T>(string name);

        /// <summary>
        ///     Creates a partitioned version <see cref="IDistributedQueue{T}" />.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of item.
        /// </typeparam>
        /// <param name="name">
        ///     The collection name.
        /// </param>
        /// <returns>
        ///     a created <see cref="IDistributedQueue{T}" /> instance.
        /// </returns>
        IDistributedQueue<T> GetQueue<T>(string name);

        /// <summary>
        ///     Creates a partitioned version <see cref="IDistributedSet{T}" />.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of item.
        /// </typeparam>
        /// <param name="name">
        ///     The collection name.
        /// </param>
        /// <returns>
        ///     a created <see cref="IDistributedSet{T}" /> instance.
        /// </returns>
        IDistributedSet<T> GetSet<T>(string name);
    }
}