// <copyright file="GridionFactory.cs" company="Gridion">
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
    using System.Collections.Generic;
    using Gridion.Core.Configurations;
    using Gridion.Core.Implementations;
    using Gridion.Core.Utils;

    /*
     *Features:
Distributed implementations of java.util.{Queue, Set, List, Map}
Distributed locks via java.util.concurrency.locks.Lock
Support for cluster info and membership events
Dynamic discovery
Dynamic scaling to hundreds of servers
Dynamic partitioning with backups
Dynamic fail-over

Superness:
Super simple to use; include a single jar.
Super fast; thousands of operations per sec.
Super small; less than a MB.
Super efficient; very nice to CPU and RAM.

Where to use it:
share data/state among many servers
cache your data (distributed cache)
cluster your application
partition your in-memory data
distribute workload onto many servers
provide fail-safe data management
     *
     */

    /*
     *
     *Hazelcast has two types of distributed objects in terms of their partitioning strategies:

Data structures where each partition stores a part of the instance, namely partitioned data structures.

Data structures where a single partition stores the whole instance, namely non-partitioned data structures.

The following are the partitioned Hazelcast data structures:

Map

MultiMap

Cache (Hazelcast JCache implementation)

Event Journal

The following are the non-partitioned Hazelcast data structures:

Queue

Set

List

Ringbuffer

Lock

ISemaphore

IAtomicLong

IAtomicReference

FlakeIdGenerator

ICountdownLatch

Cardinality Estimator

PN Counter
     */

    /// <summary>
    ///     The factory to work with <see cref="IGridion" /> API.
    ///     All members are thread-safe and may be used concurrently from multiple threads.
    ///     <para />
    ///     Use <see cref="Start()" /> method to start an <see cref="IGridion" /> instance.
    /// </summary>
    public static class GridionFactory
    {
        /// <summary>
        ///     The variable to lock on.
        /// </summary>
        private static readonly object DisposedNumberLock = new object();

        /// <summary>
        ///     The internal collection of running <see cref="IGridion" /> instances.
        /// </summary>
        private static readonly GridionCollection GridionList = new GridionCollection();

        /// <summary>
        ///     A number of disposed <see cref="IGridion" /> instances.
        /// </summary>
        private static int disposedNumber;

        /// <summary>
        ///     Gets a collection of running <see cref="IGridion" /> instances.
        /// </summary>
        /// <returns>
        ///     a collection of running <see cref="IGridion" /> instances.
        /// </returns>
        public static ICollection<IGridion> GetAll()
        {
            lock (GridionList)
            {
                return GridionList.ToArray();
            }
        }

        /// <summary>
        ///     Starts a new <see cref="IGridion" /> default instance.
        /// </summary>
        /// <returns>
        ///     a started <see cref="IGridion" /> instance.
        /// </returns>
        public static IGridion Start()
        {
            var configuration = GridionConfiguration.GetDefaultConfiguration();
            return Start(configuration);
        }

        /// <summary>
        ///     Starts a new <see cref="IGridion" /> instance.
        /// </summary>
        /// <param name="configuration">
        ///     The <see cref="IGridion" /> configuration.
        /// </param>
        /// <returns>
        ///     a started <see cref="IGridion" /> instance.
        /// </returns>
        public static IGridion Start(GridionConfiguration configuration)
        {
            Should.NotBeNull(configuration, nameof(configuration));

            lock (GridionList)
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                var instance = new GridionInternal(configuration);
#pragma warning restore CA2000 // Dispose objects before losing scope
                GridionList.Add(instance);
                instance.Start();
                return instance;
            }
        }

        /// <summary>
        ///     Stops the <see cref="IGridion" /> by its name.
        /// </summary>
        /// <param name="name">
        ///     The name of <see cref="IGridion" />.
        /// </param>
        /// <returns>
        ///     true when the instance has been stopped; otherwise returns false.
        /// </returns>
        public static bool Stop(string name)
        {
            lock (GridionList)
            {
                if (!GridionList.TryGetByName(name, out var val))
                {
                    return false;
                }

                val.Dispose();
                RemoveGridion(val);
                return true;
            }
        }

        /// <summary>
        ///     Stops all active instances of <see cref="IGridion" /> class.
        /// </summary>
        public static void StopAll()
        {
            lock (GridionList)
            {
                lock (DisposedNumberLock)
                {
                    disposedNumber = GridionList.ClearAll();
                }
            }
        }

        /// <summary>
        ///     Gets the number of disposed <see cref="IGridion" />s.
        /// </summary>
        /// <returns>
        ///     a number of disposed <see cref="IGridion" />s.
        /// </returns>
        internal static int GetDisposedInstanceNumber()
        {
            return disposedNumber;
        }

        /// <summary>
        ///     Remove the <see cref="IGridion" /> item from active server list.
        /// </summary>
        /// <param name="gridion">
        ///     The <see cref="IGridion" /> instance to remove.
        /// </param>
        internal static void RemoveGridion(IGridion gridion)
        {
            lock (GridionList)
            {
                if (GridionList.TryGetByName(gridion.Name, out _))
                {
                    GridionList.Remove(gridion.Name);
                }
            }
        }

        /// <summary>
        ///     Resets the disposed count to zero.
        /// </summary>
        internal static void ResetDisposedInstanceNumber()
        {
            lock (DisposedNumberLock)
            {
                disposedNumber = 0;
            }
        }
    }
}