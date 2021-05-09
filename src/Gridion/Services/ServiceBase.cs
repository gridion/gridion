// <copyright file="ServiceBase.cs" company="Gridion">
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

    /// <summary>
    ///     Represents a disposable object with a skeleton of actions are invoked on a disposing.
    /// </summary>
    /// <inheritdoc />
    internal abstract class ServiceBase : IDisposable
    {
        /// <summary>
        ///     Gets or sets a value indicating whether the instance is disposed.
        /// </summary>
        private bool Disposed { get; set; }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Disposes managed resources.
        /// </summary>
        protected virtual void DisposeManaged()
        {
        }

        /// <summary>
        ///     Disposes the resources.
        /// </summary>
        /// <param name="disposing">
        ///     Indicates whether need to dispose managed resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
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

            // dispose unmanaged resources
            this.Disposed = true;
        }
    }
}