// <copyright file="GridionInternal.cs" company="Gridion">
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

namespace Gridion.Core.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Gridion.Core.Collections;
    using Gridion.Core.Configurations;
    using Gridion.Core.Extensions;
    using Gridion.Core.Logging;
    using Gridion.Core.Server;
    using Gridion.Core.Services;
    using Gridion.Core.Utils;
    using Gridion.Core.Validators;

    /// <summary>
    ///     Represents an internal implementation of <see cref="IGridion" />.
    /// </summary>
    /// <inheritdoc cref="IGridion" />
    internal sealed class GridionInternal : Disposable, IGridion, IClusterInternal, IEquatable<GridionInternal>
    {
        /// <summary>
        ///     The lock object to sync access to shared resources.
        /// </summary>
        private static readonly object Lock = new object();

        /// <summary>
        ///     The <see cref="IGridion" /> configuration.
        /// </summary>
        private readonly GridionConfiguration configuration;

        /// <summary>
        ///     The reference to the <see cref="ILogger" /> instance.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        ///     The reference to the node which wraps this <see cref="IGridion" /> instance.
        /// </summary>
        [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "The field will be disposed in the DisposeManaged method.")]
        private readonly INodeInternal node;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GridionInternal" /> class.
        /// </summary>
        /// <param name="configuration">
        ///     The configuration of <see cref="IGridion" /> instance.
        /// </param>
        internal GridionInternal(GridionConfiguration configuration)
        {
            GridionConfigurationValidator.Validate(configuration);

            this.configuration = configuration;
            this.node = new Node(
                GridionServerFactory.RegisterNewServer(this.configuration.ServerConfiguration),
                new DistributedCollectionService(),
                new InMessengerService(),
                new OutMessengerService(),
                new ConsoleLogger());
            this.logger = configuration.ServerConfiguration.Logger;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="Gridion.Core.Implementations.GridionInternal" /> class.
        /// </summary>
        /// <inheritdoc cref="IGridion" />
        ~GridionInternal()
        {
            this.DisposeUnmanaged();
        }

        /// <inheritdoc />
        public ICluster Cluster => this;

        /// <inheritdoc />
        ISet<INode> INodeGroup.Nodes
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
        string IGridion.Name => this.configuration.NodeName;

        /// <inheritdoc />
        public bool Equals(GridionInternal other)
        {
            if (other is null)
            {
                return false;
            }

            return object.ReferenceEquals(this, other) || object.Equals(this.node, other.node);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
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
        ///     Starts the local node.
        /// </summary>
        internal void Start()
        {
            this.node.Start();
            this.logger.Info($"The node has been started on {this.configuration.ServerConfiguration}.");
        }

        /// <summary>
        ///     Stops the local node.
        /// </summary>
        internal void Stop()
        {
            this.node.Stop();
            this.logger.Info($"The node on {this.configuration.ServerConfiguration} has been stopped.");
        }

        /// <summary>
        ///     Dispose the internal managed/unmanaged resources.
        /// </summary>
        /// <inheritdoc />
        protected override void DisposeManaged()
        {
            lock (Lock)
            {
                this.Stop();
                ClusterCurator.Instance.Remove(this.node);
                this.node.Dispose();
                GridionFactory.RemoveGridion(this);
            }
        }

        /// <inheritdoc />
        public long DistributedObjectCount
        {
            get
            {
                long res = 0;
                foreach (var nodeInternal in ClusterCurator.Instance.GetNodes())
                {
                    res += nodeInternal.DistributedObjectsCount;
                }

                return res;
            }
        }
    }
}