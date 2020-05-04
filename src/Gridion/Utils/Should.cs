// <copyright file="Should.cs" company="Gridion">
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

namespace Gridion.Core.Utils
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    using Gridion.Core.Properties;

    using JetBrains.Annotations;

    /// <summary>
    ///     Represents common parameter assertions.
    /// </summary>
    internal static class Should
    {
        /// <summary>
        ///     Checks that the value is less or equal to zero.
        /// </summary>
        /// <param name="value">
        ///     The value to check against to.
        /// </param>
        /// <param name="name">
        ///     The parameter name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the value is less or equal to zero.
        /// </exception>
        [DebuggerStepThrough]
        [AssertionMethod]
        internal static void BePositive(int value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentException(SR.ShouldBeGreaterThanZero, name);
            }
        }

        /// <summary>
        ///     Checks the type is serializable.
        /// </summary>
        /// <param name="type">
        ///     The type to check against to.
        /// </param>
        /// <param name="name">
        ///     The parameter name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the value is null.
        /// </exception>
        [DebuggerStepThrough]
        [AssertionMethod]
        internal static void BeSerializable(Type type, string name)
        {
            if (!type.IsSerializable)
            {
                throw new SerializationException($"The type {type} of {name} is not serializable.");
            }
        }

        /// <summary>
        ///     Checks that the value is equal to <c>true</c>.
        /// </summary>
        /// <param name="value">
        ///     The value to check against to.
        /// </param>
        /// <param name="name">
        ///     The parameter name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the value is false.
        /// </exception>
        [DebuggerStepThrough]
        [AssertionMethod]
        internal static void BeTrue(bool value, string name)
        {
            if (!value)
            {
                throw new ArgumentException(SR.ShouldBeTrue, name);
            }
        }

        /// <summary>
        ///     Checks the value against the null.
        /// </summary>
        /// <param name="value">
        ///     The value to check against to.
        /// </param>
        /// <param name="name">
        ///     The parameter name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the value is null.
        /// </exception>
        [DebuggerStepThrough]
        [AssertionMethod]
        internal static void NotBeNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        ///     Checks the string value is not null or empty.
        /// </summary>
        /// <param name="value">
        ///     The value to check against to.
        /// </param>
        /// <param name="name">
        ///     The parameter name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the value is null.
        /// </exception>
        [DebuggerStepThrough]
        [AssertionMethod]
        internal static void NotBeNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        ///     Checks the string value is not null or white-space characters.
        /// </summary>
        /// <param name="value">
        ///     The value to check against to.
        /// </param>
        /// <param name="name">
        ///     The parameter name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when the value is null.
        /// </exception>
        [DebuggerStepThrough]
        [AssertionMethod]
        internal static void NotBeNullOrWhitespace(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}