// <copyright file="GridionFactory.cs" company="Gridion">
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

namespace Gridion.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;

    using Gridion.Core.Collections;
    using Gridion.Core.Extensions;
    using Gridion.Internal.Logging;
    using Gridion.Services;
    using Gridion.Services.Messages;

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
        ///     The internal collection of running <see cref="IGridion" /> instances.
        /// </summary>
        private static readonly GridionCollection GridionList = new GridionCollection();

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
            var configuration = NodeConfiguration.GetDefaultConfiguration();
            return Start(configuration);
        }

        /// <summary>
        ///     Starts a new <see cref="IGridion" /> instance.
        /// </summary>
        /// <param name="nodeConfiguration">
        ///     The <see cref="IGridion" /> configuration.
        /// </param>
        /// <returns>
        ///     a started <see cref="IGridion" /> instance.
        /// </returns>
        public static IGridion Start(NodeConfiguration nodeConfiguration)
        {
            Should.NotBeNull(nodeConfiguration, nameof(nodeConfiguration));

            lock (GridionList)
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                var instance = CreateNew(nodeConfiguration, ClusterCurator.Instance);
#pragma warning restore CA2000 // Dispose objects before losing scope
                instance.NodeStart();
                GridionList.Add(instance);
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
                GridionList.ClearAll();
            }
        }

        /// <summary>Creates a new <see cref="IGridionInternal" /> instance.</summary>
        /// <param name="nodeConfiguration">The configuration of the instance.</param>
        /// <param name="curator">The cluster curator.</param>
        /// <returns>an initialized instance.</returns>
        private static IGridionInternal CreateNew(NodeConfiguration nodeConfiguration, IClusterCurator curator)
        {
            return new GridionInternal(nodeConfiguration, curator);
        }

        /// <summary>
        ///     Remove the <see cref="IGridion" /> item from active server list.
        /// </summary>
        /// <param name="gridion">
        ///     The <see cref="IGridion" /> instance to remove.
        /// </param>
        private static void RemoveGridion(IGridion gridion)
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
        ///     Represents a dictionary of <see cref="IGridion" /> items.
        /// </summary>
        /// <inheritdoc cref="Dictionary{TKey,TValue}" />
        private sealed class GridionCollection : Dictionary<GridionCollectionKey, IGridionInternal>
        {
            /// <summary>
            ///     Add an <see cref="IGridion" /> into the collection.
            /// </summary>
            /// <param name="gridion">
            ///     The item to add.
            /// </param>
            internal void Add(IGridionInternal gridion)
            {
                Should.NotBeNull(gridion, nameof(gridion));
                Should.NotBeNull(gridion.Name, nameof(gridion.Name));

                var key = BuildKey(gridion.Name);
                this.Add(key, gridion);
            }

            /// <summary>
            ///     Clears the collection with disposing of each of items.
            /// </summary>
            internal void ClearAll()
            {
                foreach (var value in this.Values)
                {
                    value.Dispose();
                }

                this.Clear();
            }

            /// <summary>
            ///     Tries to remove the item by its name.
            /// </summary>
            /// <param name="name">
            ///     The name of item.
            /// </param>
            internal void Remove(string name)
            {
                Should.NotBeNull(name, nameof(name));

                var key = BuildKey(name);
                this.Remove(key);
            }

            /// <summary>
            ///     Creates an array of <see cref="IGridion" />s located in the collection.
            /// </summary>
            /// <returns>
            ///     an array of <see cref="IGridion" />s.
            /// </returns>
            internal IGridion[] ToArray()
            {
                var list = new IGridion[this.Count];

                var i = 0;
                foreach (var pair in this.Values)
                {
                    list[i++] = pair;
                }

                return list;
            }

            /// <summary>
            ///     Tries to get the item by its name.
            /// </summary>
            /// <param name="name">
            ///     The name of item.
            /// </param>
            /// <param name="val">
            ///     The resulting <see cref="IGridion" /> item.
            /// </param>
            /// <returns>
            ///     true when the value has been found; otherwise returns false.
            /// </returns>
            internal bool TryGetByName(string name, out IGridionInternal val)
            {
                Should.NotBeNull(name, nameof(name));

                var key = BuildKey(name);
                return this.TryGetValue(key, out val);
            }

            /// <summary>
            ///     Build a collection key.
            /// </summary>
            /// <param name="key">
            ///     A key to build on.
            /// </param>
            /// <returns>
            ///     a key for <see cref="GridionCollection" />.
            /// </returns>
            private static GridionCollectionKey BuildKey(string key)
            {
                return new GridionCollectionKey(key ?? string.Empty);
            }
        }

        /// <summary>
        ///     Represents a key wrapper for <see cref="GridionCollection" />.
        /// </summary>
        /// <inheritdoc cref="IEquatable{T}" />
        private sealed class GridionCollectionKey : IEquatable<GridionCollectionKey>
        {
            /// <summary>
            ///     The name of <see cref="IGridion" /> instance.
            /// </summary>
            private readonly string key;

            /// <summary>
            ///     Initializes a new instance of the <see cref="GridionCollectionKey" /> class.
            /// </summary>
            /// <param name="key">
            ///     The key of item.
            /// </param>
            internal GridionCollectionKey(string key)
            {
                this.key = key ?? string.Empty;
            }

            /// <inheritdoc />
            public bool Equals(GridionCollectionKey other)
            {
                if (other is null)
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.key == other.key;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                return obj.GetType() == this.GetType() && this.Equals((GridionCollectionKey)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return HashCode.Combine(this.key);
            }
        }

        /// <summary>
        ///     Represents an internal implementation of <see cref="IGridion" />.
        /// </summary>
        /// <inheritdoc cref="IGridion" />
        private sealed class GridionInternal : IGridionInternal, IClusterInternal, IEquatable<GridionInternal>
        {
            /// <summary>
            ///     The lock object to sync access to shared resources.
            /// </summary>
            private static readonly object Lock = new object();

            /// <summary>
            ///     The reference to the node which wraps this <see cref="IGridion" /> instance.
            /// </summary>
            [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "The field will be disposed in the DisposeManaged method.")]
            private readonly INodeInternal node;

            /// <summary>
            ///     The <see cref="IGridion" /> configuration.
            /// </summary>
            private readonly NodeConfiguration nodeConfiguration;

            private EventHandler disposed;

            /// <summary>
            ///     Initializes a new instance of the <see cref="GridionInternal" /> class.
            /// </summary>
            /// <param name="nodeConfiguration">
            ///     The configuration of <see cref="IGridion" /> instance.
            /// </param>
            /// <param name="curator">
            ///     The cluster curator.
            /// </param>
            internal GridionInternal(NodeConfiguration nodeConfiguration, IClusterCurator curator)
            {
                Should.NotBeNull(nodeConfiguration, nameof(nodeConfiguration));
                nodeConfiguration.Validate();

                this.nodeConfiguration = nodeConfiguration;
                this.node = new Node(nodeConfiguration.ServerConfiguration, curator, new ConsoleLogger());
            }

            /// <inheritdoc />
            event EventHandler IGridionInternal.Disposed
            {
                add
                {
                    this.disposed += value;
                }

                remove
                {
                    this.disposed -= value;
                }
            }

            /// <inheritdoc />
            public ICluster Cluster => this;

            /// <inheritdoc />
            public bool IsRunning => this.node.IsRunning;

            /// <inheritdoc />
            ISet<INode> ICluster.Nodes
            {
                get
                {
                    IEnumerable<INodeInternal> nodeInternals = ClusterCurator.Instance.GetNodes();
                    var set = new HashSet<INode>();
                    foreach (var @internal in nodeInternals)
                    {
                        set.Add(@internal);
                    }

                    return set;
                }
            }

            /// <inheritdoc />
            string IGridion.Name => this.nodeConfiguration.NodeName;

            /// <summary>
            ///     Gets or sets a value indicating whether the instance is disposed.
            /// </summary>
            private bool IsDisposed { get; set; }

            /// <inheritdoc />
            public void Dispose()
            {
                this.Dispose(true);
            }

            /// <inheritdoc />
            public bool Equals(GridionInternal other)
            {
                if (other is null)
                {
                    return false;
                }

                return ReferenceEquals(this, other) || Equals(this.node, other.node);
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj is null)
                {
                    return false;
                }

                return obj.GetType() == this.GetType() && this.Equals((GridionInternal)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return this.node?.GetHashCode() ?? 0;
            }

            /// <summary>
            ///     Starts the local node.
            /// </summary>
            public void NodeStart()
            {
                this.node.Start();
            }

            /// <summary>
            ///     Stops the local node.
            /// </summary>
            public void NodeStop()
            {
                this.node.Stop();
            }

            /// <inheritdoc />
            IDistributedDictionary<TKey, TValue> IGridion.GetDictionary<TKey, TValue>(string name)
            {
                Should.NotBeNullOrEmpty(name, nameof(name));
                Should.NotBeNullOrWhitespace(name, nameof(name));

                return this.node.GetOrCreateDictionary<TKey, TValue>(name);
            }

            /// <inheritdoc />
            IDistributedList<T> IGridion.GetList<T>(string name)
            {
                Should.NotBeNullOrEmpty(name, nameof(name));
                Should.NotBeNullOrWhitespace(name, nameof(name));

                return this.node.GetOrCreateList<T>(name);
            }

            /// <inheritdoc />
            IDistributedQueue<T> IGridion.GetQueue<T>(string name)
            {
                Should.NotBeNullOrEmpty(name, nameof(name));
                Should.NotBeNullOrWhitespace(name, nameof(name));

                return this.node.GetOrCreateQueue<T>(name);
            }

            /// <inheritdoc />
            IDistributedSet<T> IGridion.GetSet<T>(string name)
            {
                Should.NotBeNullOrEmpty(name, nameof(name));
                Should.NotBeNullOrWhitespace(name, nameof(name));

                return this.node.GetOrCreateSet<T>(name);
            }

            /// <summary>
            ///     Disposes the resources.
            /// </summary>
            /// <param name="disposing">
            ///     Indicates whether need to dispose managed resources.
            /// </param>
            private void Dispose(bool disposing)
            {
                if (this.IsDisposed)
                {
                    return;
                }

                if (disposing)
                {
                    // dispose managed resources
                    this.DisposeManaged();
                }
                
                this.IsDisposed = true;
                this.OnDisposed(new GridionDisposedEventArgs());
            }

            private void OnDisposed(EventArgs e)  
            {  
                this.disposed?.Invoke(this, e);  
            }

            /// <summary>
            ///     Dispose the internal managed/unmanaged resources.
            /// </summary>
            private void DisposeManaged()
            {
                lock (Lock)
                {
                    this.NodeStop();
                    ClusterCurator.Instance.Remove(this.node);
                    this.node.Dispose();
                    RemoveGridion(this);
                }
            }

            /// <summary>
            ///     Represents internal implementation of <see cref="INodeInternal" /> interface.
            /// </summary>
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
            private sealed class Node : INodeInternal, IEquatable<Node>
            {
                /// <summary>
                ///     The cancellation token source to cancel operations.
                /// </summary>
                [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "The field will be disposed in the DisposeManaged method.")]
                private readonly CancellationTokenSource cts = new CancellationTokenSource();

                /// <summary>
                /// The cluster curator.
                /// </summary>
                private readonly IClusterCurator curator;

                /// <summary>
                ///     The collection of initiated dictionaries on the node.
                /// </summary>
                private readonly ConcurrentDictionary<string, object> dictionaryMap = new ConcurrentDictionary<string, object>();

                /// <summary>
                ///     The associated <see cref="IGridionServer" /> instance.
                /// </summary>
                private readonly IGridionServer gridionServer;

                /// <summary>
                ///     The service that routes in messages.
                /// </summary>
                private readonly IMessengerService inMessengerService;

                /// <summary>
                ///     The lock object.
                /// </summary>
                private readonly object @lock = new object();

                /// <summary>
                ///     The node logger.
                /// </summary>
                private readonly ILogger logger;

                /// <summary>
                ///     The service that routes out messages.
                /// </summary>
                private readonly IMessengerService outMessengerService;

                /// <summary>The ID of the node.</summary>
                private readonly Guid id;

                ///// <summary>
                /////     The collection of initiated lists on the node.
                ///// </summary>
                // private readonly ConcurrentDictionary<string, object> listMap = new ConcurrentDictionary<string, object>();

                ///// <summary>
                /////     The collection of initiated queue on the node.
                ///// </summary>
                // private readonly ConcurrentDictionary<string, object> queueMap = new ConcurrentDictionary<string, object>();

                ///// <summary>
                /////     The collection of initiated sets on the node.
                ///// </summary>
                // private readonly ConcurrentDictionary<string, object> setMap = new ConcurrentDictionary<string, object>();

                /// <summary>
                ///     Initializes a new instance of the <see cref="Node" /> class.
                /// </summary>
                /// <param name="configuration">The server configuration.</param>
                /// <param name="curator">The cluster curator.</param>
                /// <param name="logger">The logger.</param>
                /// <exception cref="Exceptions.GridionException">
                ///     When there are presented some issues with server initialization then a <see cref="Exceptions.GridionException" />
                ///     is thrown.
                /// </exception>
                internal Node(ServerConfiguration configuration, IClusterCurator curator, ILogger logger)
                {
                    Should.NotBeNull(configuration, nameof(configuration));
                    Should.NotBeNull(curator, nameof(curator));
                    Should.NotBeNull(logger, nameof(logger));

                    this.gridionServer = GridionServerFactory.RegisterNewServer(configuration);
                    this.inMessengerService = new MemoryMessengerService();
                    this.outMessengerService = new MemoryMessengerService();
                    this.curator = curator;
                    this.logger = logger;
                    this.id = Guid.NewGuid();
                }

                /// <inheritdoc />
                string INode.Name
                {
                    get
                    {
                        return this.id.ToString("B", CultureInfo.InvariantCulture);
                    }
                }
                
                /// <inheritdoc />
                string ISender.Name
                {
                    get => ((INode)this).Name;
                }

                /// <inheritdoc />
                public long DistributedObjectsNumber
                {
                    get
                    {
                        long res = 0;

                        foreach (KeyValuePair<string, object> _ in this.dictionaryMap)
                        {
                            res++;
                        }

                        // res += this.listMap.Count;
                        // res += this.queueMap.Count;
                        // res += this.setMap.Count;
                        return res;
                    }
                }

                /// <inheritdoc />
                public bool IsRunning { get; private set; }

                /// <inheritdoc />
                bool INodeInternal.IsMasterNode { get; set; }

                /// <summary>
                ///     Gets or sets a value indicating whether the instance is disposed.
                /// </summary>
                private bool Disposed { get; set; }

                /// <inheritdoc />
                public void Accept(IMessage message)
                {
                    Should.NotBeNull(message, nameof(message));
                    Should.NotBeNull(message.Sender, nameof(message.Sender));

                    ISender recipient = this;
                    if (!recipient.Name.Equals(message.Sender.Name))
                    {
                        return;
                    }

                    if (!(message is CollectionCreatedMessageBase actionMessage))
                    {
                        return;
                    }

                    var distributedCollection = actionMessage.Create();
                    this.dictionaryMap.TryAdd(actionMessage.Name, distributedCollection);
                }

                /// <inheritdoc />
                public void Dispose()
                {
                    this.Dispose(true);
                }

                /// <inheritdoc />
                public bool Equals(Node other)
                {
                    if (other is null)
                    {
                        return false;
                    }

                    return ReferenceEquals(this, other) || Equals(this.gridionServer, other.gridionServer);
                }

                /// <inheritdoc />
                public override bool Equals(object obj)
                {
                    return ReferenceEquals(this, obj) || (obj is Node other && this.Equals(other));
                }

                /// <inheritdoc />
                public override int GetHashCode()
                {
                    return HashCode.Combine(this.gridionServer);
                }

                /// <inheritdoc />
                public IDistributedDictionary<TKey, TValue> GetOrCreateDictionary<TKey, TValue>(string name)
                {
                    lock (this.@lock)
                    {
                        if (this.dictionaryMap.ContainsKey(name))
                        {
                            return (IDistributedDictionary<TKey, TValue>)this.dictionaryMap[name];
                        }

                        var dictionary = new DistributedDictionary<TKey, TValue>(name);

                        var message = new DictionaryCreatedMessage(this, name, typeof(TKey), typeof(TValue));

                        this.outMessengerService.Send(this.curator.GetNodes(), message);

                        this.dictionaryMap.TryAdd(name, dictionary);

                        return dictionary;
                    }
                }

                /// <inheritdoc />
                public IDistributedList<T> GetOrCreateList<T>(string name)
                {
                    Should.NotBeNull(name, nameof(name));
                    return new DistributedList<T>(name);
                }

                /// <inheritdoc />
                public IDistributedQueue<T> GetOrCreateQueue<T>(string name)
                {
                    Should.NotBeNull(name, nameof(name));
                    return new DistributedQueue<T>(name);
                }

                /// <inheritdoc />
                public IDistributedSet<T> GetOrCreateSet<T>(string name)
                {
                    Should.NotBeNull(name, nameof(name));
                    return new DistributedSet<T>(name);
                }

                /// <inheritdoc />
                public void Stop()
                {
                    lock (this.@lock)
                    {
                        if (!this.IsRunning)
                        {
                            return;
                        }

                        this.cts.Cancel();

                        WaitHandle.WaitAny(new[] { this.cts.Token.WaitHandle });

                        this.gridionServer.Stop();

                        this.IsRunning = false;
                    }

                    this.logger.Info($"The node on {this.gridionServer.Configuration} has been stopped.");
                }

                /// <inheritdoc />
                public override string ToString()
                {
                    return this.gridionServer.ToString();
                }

                /// <inheritdoc />
                void INodeInternal.Start()
                {
                    if (this.IsRunning)
                    {
                        return;
                    }

                    lock (this.@lock)
                    {
                        this.inMessengerService.Start();
                        this.outMessengerService.Start();

                        ClusterCurator.Instance.Join(this);

                        this.IsRunning = true;
                    }

                    this.logger.Info($"The node has been started on {this.gridionServer.Configuration}.");
                }

                /// <summary>
                ///     Disposes the resources.
                /// </summary>
                /// <param name="disposing">
                ///     Indicates whether need to dispose managed resources.
                /// </param>
                private void Dispose(bool disposing)
                {
                    if (this.Disposed)
                    {
                        return;
                    }

                    if (disposing)
                    {
                        // dispose managed resources
                        this.DisposeManaged();
                    }

                    this.Disposed = true;
                }

                /// <summary>
                ///     Dispose the internal managed resources.
                /// </summary>
                private void DisposeManaged()
                {
                    this.inMessengerService?.Dispose();
                    this.outMessengerService?.Dispose();
                    this.gridionServer?.Dispose();
                    this.cts?.Dispose();
                }
            }

            /// <inheritdoc />
            private sealed class GridionDisposedEventArgs : EventArgs
            {
            }
        }
    }
}