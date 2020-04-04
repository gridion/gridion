// <copyright file="LogUtils.cs" company="Gridion">
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

    /// <summary>
    ///     Represents a set of utility methods to work with internal logging API.
    /// </summary>
    internal static class LogUtils
    {
        /// <summary>
        ///     Converts the <see cref="LogLevel" /> level to a string.
        /// </summary>
        /// <param name="this">
        ///     The level to convert.
        /// </param>
        /// <returns>
        ///     a string representation of <see cref="LogLevel" /> item.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the <see cref="LogLevel" /> item is not found.
        /// </exception>
        internal static string AsString(this LogLevel @this)
        {
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (@this)
#pragma warning restore IDE0066 // Convert switch statement to expression
            {
                case LogLevel.Trace:
                    return "[TRACE]";
                case LogLevel.Debug:
                    return "[DEBUG]";
                case LogLevel.Info:
                    return "[INFO]";
                case LogLevel.Warn:
                    return "[WARN]";
                case LogLevel.Error:
                    return "[ERROR]";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@this), @this, null);
            }
        }
    }
}