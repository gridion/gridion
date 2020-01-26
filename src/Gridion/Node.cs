// <copyright file="Node.cs" company="Gridion">
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
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Gridion.Core.Collections;
    using Gridion.Core.Logging;
    using Gridion.Core.Services;
    using Gridion.Core.Utils;

    /// <summary>
    ///     Represents internal implementation of <see cref="INodeInternal" /> interface.
    /// </summary>
    /// <inheritdoc cref="Disposable" />
    /// <inheritdoc cref="INodeInternal" />
    /// <p>
    ///     https://medium.com/@urib_61381/electing-a-leader-with-hazelcast-3928559bfb08 .
    /// </p>
    /// <p>
    ///     https://stackoverflow.com/questions/53605172/leader-election-with-etcd-vs-zookeeper-vs-hazelcast
    ///     1) Hazelcast, by version 3.12, provides a CPSubsystem which is a CP system in terms of CAP and built using Raft
    ///     consensus algorithm inside the Hazelcast cluster. CPSubsytem has a distributed lock implementation called
    ///     FencedLock which can be used to implement a leader election.
    ///     For more information about CPSubsystem and FencedLock see;
    ///     CP Subsystem Reference manual
    ///     Riding the CP Subsystem
    ///     Distributed Locks are Dead; Long Live Distributed Locks!
    ///     Hazelcast versions before 3.12 are not suitable for leader election. As you already mentioned, it can choose
    ///     availability during network splits, which can lead to election of multiple leaders.
    ///     2) Zookeeper doesn't suffer from the mentioned split-brain problem, you will not observe multiple leaders when
    ///     network split happens. Zookeeper is built on ZAB atomic broadcast protocol.
    ///     3) Etcd is using Raft consensus protocol. Raft and ZAB have similar consistency guarantees, which both can be used
    ///     to implement a leader election process.
    /// </p>
    /// <p>
    ///     17.3. Internals 3: Cluster Membership
    ///     It is important to note that Hazelcast is a peer to peer clustering so there is no 'master' kind of server in
    ///     Hazelcast. Every member in the cluster is equal and has the same rights and responsibilities.
    ///     When a node starts up, it will check to see if there is already a cluster in the network. There are two ways to
    ///     find this out:
    ///     Multicast discovery: If multicast discovery is enabled (that is the defualt) then the node will send a join request
    ///     in the form of a multicast datagram packet.
    ///     Unicast discovery: if multicast discovery is disabled and TCP/IP join is enabled then the node will try to connect
    ///     to he IPs defined in the hazelcast.xml configuration file. If it can successfully connect to at least one node,
    ///     then it will send a join request through the TCP/IP connection.
    ///     If there is no existing node, then the node will be the first member of the cluster. If multicast is enabled then
    ///     it will start a multicast listener so that it can respond to incoming join requests. Otherwise it will listen for
    ///     join request coming viaTCP/IP.
    ///     If there is an existing cluster already, then the oldest member in the cluster will receive the join request and
    ///     check if the request is for the right group. If so, the oldest member in the cluster will start the join process.
    ///     In the join process, the oldest member will:
    ///     send the new member list to all members
    ///     tell members to sync data in order to balance the data load
    ///     Every member in the cluster has the same member list in the same order. First member is the oldest member so if the
    ///     oldest member dies, second member in the list becomes the first member in the list and the new oldest member.
    ///     See com.hazelcast.impl.Node and com.hazelcast.impl.ClusterManager for details.
    ///     Q. If, let say 50+, nodes are trying to join the cluster at the same time, are they going to join the cluster one
    ///     by one?
    ///     No. As soon as the oldest member receives the first valid join request, it will wait 5 seconds for others to join
    ///     so that it can join multiple members in one shot. If there is no new node willing to join for the next 5 seconds,
    ///     then oldest member will start the join process. If a member leaves the cluster though, because of a JVM crash for
    ///     example, cluster will immediately take action and oldest member will start the data recovery process.
    /// </p>
    internal sealed class Node : Disposable, INodeInternal
    {
        /// <summary>
        ///     The cancellation token source to cancel operations.
        /// </summary>
        [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "The field will be disposed in the DisposeManaged method.")]
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        ///     The collection of initiated dictionaries on the node.
        /// </summary>
        private readonly ConcurrentDictionary<string, object> dictionaryMap = new ConcurrentDictionary<string, object>();

        /// <summary>
        ///     The service that creates collections.
        /// </summary>
        private readonly IGridionCollectionService distributedCollectionService;

        /// <summary>
        ///     The associated <see cref="IGridionServer" /> instance.
        /// </summary>
        private readonly IGridionServer gridionServer;

        /// <summary>
        ///     The service that routes in messages.
        /// </summary>
        private readonly IGridionService inMessengerService;

        /// <summary>
        ///     The collection of initiated lists on the node.
        /// </summary>
        private readonly ConcurrentDictionary<string, object> listMap = new ConcurrentDictionary<string, object>();

        /// <summary>
        ///     The service that routes out messages.
        /// </summary>
        private readonly IGridionService outMessengerService;

        /// <summary>
        ///     The collection of initiated queue on the node.
        /// </summary>
        private readonly ConcurrentDictionary<string, object> queueMap = new ConcurrentDictionary<string, object>();

        /// <summary>
        ///     The collection of initiated sets on the node.
        /// </summary>
        private readonly ConcurrentDictionary<string, object> setMap = new ConcurrentDictionary<string, object>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        /// <param name="gridionServer">The server.</param>
        /// <param name="distributedCollectionService">The collection service.</param>
        /// <param name="inMessengerService">The in messenger service.</param>
        /// <param name="outMessengerService">The out messenger service.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="GridionException">
        ///     When there are presented some issues with server initialization then a <see cref="GridionException" /> is thrown.
        /// </exception>
        public Node(
            IGridionServer gridionServer,
            IGridionCollectionService distributedCollectionService,
            IGridionService inMessengerService,
            IGridionService outMessengerService,
            ILogger logger)
        {
            Should.NotBeNull(gridionServer, nameof(gridionServer));
            Should.NotBeNull(distributedCollectionService, nameof(distributedCollectionService));
            Should.NotBeNull(inMessengerService, nameof(inMessengerService));
            Should.NotBeEqual(inMessengerService, outMessengerService, nameof(inMessengerService), nameof(outMessengerService));
            Should.NotBeNull(logger, nameof(logger));

            this.gridionServer = gridionServer;
            this.distributedCollectionService = distributedCollectionService;
            this.inMessengerService = inMessengerService;
            this.outMessengerService = outMessengerService;

            ClusterCurator.Instance.Join(this);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="Gridion.Core.Node" /> class.
        /// </summary>
        /// <inheritdoc cref="Disposable" />
        ~Node()
        {
            this.DisposeUnmanaged();
        }

        /// <inheritdoc />
        public long DistributedObjectsCount
        {
            get
            {
                long res = 0;
                res += this.dictionaryMap.Count;
                res += this.listMap.Count;
                res += this.queueMap.Count;
                res += this.setMap.Count;
                return res;
            }
        }

        /// <inheritdoc />
        bool INodeInternal.IsMasterNode { get; set; }

        /// <inheritdoc />
        public void AcceptData(INodeMessage message)
        {
        }

        /// <inheritdoc />
        public IDistributedDictionary<TKey, TValue> GetOrCreateDictionary<TKey, TValue>(string name)
        {
            if (this.dictionaryMap.TryGetValue(name, out var value))
            {
                return (IDistributedDictionary<TKey, TValue>)value;
            }

            Task<IDistributedDictionary<TKey, TValue>> task = this.distributedCollectionService.CreateDictionaryAsync<TKey, TValue>(name);

            // ReSharper disable once AsyncConverter.AsyncWait
            IDistributedDictionary<TKey, TValue> dictionary = task.Result;
            this.dictionaryMap.AddOrUpdate(name, dictionary, (nm, val) => { return val; });
            return dictionary;
        }

        /// <inheritdoc />
        public IDistributedList<T> GetOrCreateList<T>(string name)
        {
            if (this.listMap.TryGetValue(name, out var value))
            {
                return (IDistributedList<T>)value;
            }

            Task<IDistributedList<T>> task = this.distributedCollectionService.CreateListAsync<T>(name);

            // ReSharper disable once AsyncConverter.AsyncWait
            IDistributedList<T> list = task.Result;
            this.listMap.AddOrUpdate(name, list, (nm, val) => { return val; });
            return list;
        }

        /// <inheritdoc />
        public IDistributedQueue<T> GetOrCreateQueue<T>(string name)
        {
            if (this.queueMap.TryGetValue(name, out var value))
            {
                return (IDistributedQueue<T>)value;
            }

            Task<IDistributedQueue<T>> task = this.distributedCollectionService.CreateQueueAsync<T>(name);

            // ReSharper disable once AsyncConverter.AsyncWait
            IDistributedQueue<T> queue = task.Result;
            this.queueMap.AddOrUpdate(name, queue, (nm, val) => { return val; });
            return queue;
        }

        /// <inheritdoc />
        public IDistributedSet<T> GetOrCreateSet<T>(string name)
        {
            if (this.setMap.TryGetValue(name, out var value))
            {
                return (IDistributedSet<T>)value;
            }

            Task<IDistributedSet<T>> task = this.distributedCollectionService.CreateSetAsync<T>(name);

            // ReSharper disable once AsyncConverter.AsyncWait
            IDistributedSet<T> set = task.Result;
            this.setMap.AddOrUpdate(name, set, (nm, val) => { return val; });
            return set;
        }

        /// <inheritdoc />
        public void Stop()
        {
            this.cts.Cancel();
            WaitHandle.WaitAny(new[] { this.cts.Token.WaitHandle });
        }

        /// <inheritdoc />
        void INodeInternal.Start()
        {
            this.distributedCollectionService.Start();
            this.inMessengerService.Start();
            this.outMessengerService.Start();
        }

        /// <summary>
        ///     Dispose the internal managed/unmanaged resources.
        /// </summary>
        /// <inheritdoc />
        protected override void DisposeManaged()
        {
            this.distributedCollectionService?.Dispose();
            this.inMessengerService?.Dispose();
            this.outMessengerService?.Dispose();
            this.gridionServer?.Dispose();
            this.cts?.Dispose();
        }
    }
}