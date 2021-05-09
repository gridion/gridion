// <copyright file="DistributedCollectionService.cs" company="Gridion">
//     Copyright (c) 2019-2021, Alex Efremov (https://github.com/alexander-efremov)
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

namespace Gridion.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    using Gridion.Core.Collections;

    /// <summary>
    ///     Represents a service that is working with distributed collections.
    /// </summary>
    /// <inheritdoc cref="IGridionCollectionService" />
    /// <inheritdoc cref="GridionService" />
    internal sealed class DistributedCollectionService : GridionService, IGridionCollectionService
    {
        /// <summary>
        ///     The cancellation token source.
        /// </summary>
        [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "The field will be disposed in the DisposeManaged method.")]
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        ///     The blocking collection of distributed collections.
        /// </summary>
        [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "The field will be disposed in the DisposeManaged method.")]
        private readonly BlockingCollection<IDistributedCollection> items = new BlockingCollection<IDistributedCollection>(
            new ConcurrentQueue<IDistributedCollection>(),
            500);

        /// <inheritdoc cref="GridionService" />
        public DistributedCollectionService()
            : base("DistributedCollectionService")
        {
        }

        // /// <inheritdoc />
        // public Task<IDistributedDictionary<TKey, TValue>> CreateDictionaryAsync<TKey, TValue>(string name)
        // {
        //     throw new NotImplementedException();
        // }
           
        // /// <inheritdoc />
        // public Task<IDistributedList<T>> CreateListAsync<T>(string name)
        // {
        //     throw new NotImplementedException();
        // }
           
        // /// <inheritdoc />
        // public Task<IDistributedQueue<T>> CreateQueueAsync<T>(string name)
        // {
        //     throw new NotImplementedException();
        // }
           
        // /// <inheritdoc />
        // public Task<IDistributedSet<T>> CreateSetAsync<T>(string name)
        // {
        //     throw new NotImplementedException();
        // }

        /// <inheritdoc />
        public override void Start()
        {
            base.Start();
            var thread = new Thread(() => this.WorkingOnQueue(this.cancellationTokenSource.Token)) { IsBackground = true };
            thread.Start();
        }

        /// <inheritdoc />
        public override void Stop()
        {
            this.cancellationTokenSource.Cancel();
            WaitHandle.WaitAll(new[] { this.cancellationTokenSource.Token.WaitHandle });
            this.items.CompleteAdding();
            base.Stop();
        }

        /// <summary>
        ///     Dispose the internal managed/unmanaged resources.
        /// </summary>
        /// <inheritdoc />
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            this.items.Dispose();
            this.cancellationTokenSource.Dispose();
        }

        /// <summary>
        ///     Works on jobs in the queue.
        /// </summary>
        /// <param name="token">
        ///     The cancellation token.
        /// </param>
        private void WorkingOnQueue(CancellationToken token)
        {
            try
            {
                foreach (var unused in this.items.GetConsumingEnumerable(token))
                {
                }
            }

#pragma warning disable CA1031 // Do not catch general exception types
            catch (OperationCanceledException)
            {
            }

#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}