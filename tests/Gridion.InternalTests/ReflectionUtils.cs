// <copyright file="ReflectionUtils.cs" company="Gridion">
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

namespace Gridion.InternalTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Represents a set of reflection utils.
    /// </summary>
    internal static class ReflectionUtils
    {
        /// <summary>
        ///     Builds a method signature.
        /// </summary>
        /// <param name="mi">The method info.</param>
        /// <returns>a method signature as a string.</returns>
        internal static string BuildMethodSignature(MethodInfo mi)
        {
            var param = mi.GetParameters().Select(
                    p => string.Format(
                        CultureInfo.InvariantCulture,
                        "{0} {1}",
                        p.ParameterType.Name.Replace("&", string.Empty, StringComparison.InvariantCulture),
                        p.Name))
                .ToArray();

            var signature = string.Format(CultureInfo.InvariantCulture, "{0} {1}({2})", mi.ReturnType.Name, mi.Name, string.Join(",", param));

            return signature;
        }

        /// <summary>
        ///     Gather a list of interface methods.
        /// </summary>
        /// <param name="interfaceType">The type to gather against to.</param>
        /// <param name="list">The list of methods.</param>
        internal static void GetInterfaceMethods(Type interfaceType, ISet<MethodInfo> list)
        {
            foreach (var mi in interfaceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (!list.Contains(mi))
                {
                    list.Add(mi);
                }
            }

            foreach (var @base in interfaceType.GetInterfaces())
            {
                GetInterfaceMethods(@base, list);
            }
        }
    }
}