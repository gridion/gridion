// <copyright file="ConsoleLogger.cs" company="Gridion">
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

namespace Gridion.Core.Logging
{
    using System;
    using System.Text;

    using Gridion.Core.Utils;

    /// <summary>
    ///     Represents an implementation of <see cref="Gridion.Core.Logging.ILogger" /> that uses
    ///     <see cref="System.Console" /> to log operations.
    /// </summary>
    /// <inheritdoc />
    internal class ConsoleLogger : ILogger
    {
        /// <summary>
        ///     The lock object to sync access to shared resources.
        /// </summary>
        private static readonly object Lock = new object();

        /// <inheritdoc />
        void ILogger.Log(LogLevel level, string message, string type, IFormatProvider formatProvider, Exception exception, params object[] args)
        {
            Should.NotBeNull(message, nameof(message));

            lock (Lock)
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(type))
                {
                    sb.Append(type);
                    sb.Append(": ");
                }

                sb.Append(level.AsString());
                sb.Append(": ");
                if (args != null)
                {
                    sb.AppendFormat(formatProvider, message, args);
                }
                else
                {
                    sb.Append(message);
                }

                sb.Append(Environment.NewLine);
                if (exception != null)
                {
                    sb.Append(exception);
                }

                if (!string.IsNullOrEmpty(type))
                {
                    Console.Write($@"{type}: ");
                }

                Console.WriteLine($@"{level.AsString()}: {message.ToString(formatProvider)}");
            }
        }
    }
}