// <copyright file="TaskExtensions.cs" company="Gridion">
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

namespace Gridion.Core.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Gridion.Core.Utils;

    /// <summary>
    ///     Represents a set of helper methods to work with <see cref="Task" /> instances.
    /// </summary>
    internal static class TaskExtensions
    {
        /// <summary>
        ///     A method which allow to cancel operations that has no an Async method overload with cancellation token.
        /// </summary>
        /// <param name="task">
        ///     The task to tune in.
        /// </param>
        /// <param name="cancellationToken">
        ///     The cancellation token.
        /// </param>
        /// <typeparam name="T">
        ///     The type which the task is used.
        /// </typeparam>
        /// <returns>
        ///     a patched task with cancellation feature.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        ///     When <see cref="CancellationTokenSource" /> instance call Cancel method this method throws
        ///     <see cref="OperationCanceledException" /> exception.
        /// </exception>
        internal static async Task<T> WithCancellationAsync<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            Should.NotBeNull(task, nameof(task));

            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }

            return await task.ConfigureAwait(false);
        }
    }
}