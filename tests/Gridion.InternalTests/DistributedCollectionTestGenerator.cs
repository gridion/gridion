// <copyright file="DistributedCollectionTestGenerator.cs" company="Gridion">
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

namespace Gridion.InternalTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Gridion.Core.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a generator of tests for public distributed collections.
    /// </summary>
    internal class DistributedCollectionTestGenerator
    {
        /// <summary>
        ///     Builds the tests for public collections.
        /// </summary>
        [TestMethod]
        public void BuildTestForDistributedTypes()
        {
            var builder = new DistributedCollectionTestGenerator();
            Console.WriteLine(builder.Build(typeof(IDistributedDictionary<object, object>), "IDistributedDictionary{T, V}").ToString());
            Console.WriteLine();
            Console.WriteLine(builder.Build(typeof(IDistributedList<object>), "IDistributedList{T}").ToString());
            Console.WriteLine();
            Console.WriteLine(builder.Build(typeof(AbstractDistributedQueue<object>), "IDistributedQueue{T}").ToString());
            Console.WriteLine();
            Console.WriteLine(builder.Build(typeof(IDistributedSet<object>), "IDistributedSet{T}").ToString());
        }

        private StringBuilder Build(Type type, string name)
        {
            var tab = new string(' ', 4);
            var builder = new StringBuilder();
            var typeName = type.Name;
            while (char.IsNumber(typeName[^1]) || typeName[^1] == '`')
            {
                typeName = typeName[..^1];
            }

            builder.AppendLine("/// <summary>");
            builder.AppendLine($"/// Represents a set of test methods for <see cref=\"{name}\"/> interface.");
            builder.AppendLine("/// </summary>");
            builder.AppendLine("[TestClass]");
            builder.AppendLine("public class " + typeName + "Tests");
            builder.AppendLine("{");
            var infos = new HashSet<MethodInfo>();
            this.GetInterfaceMethods(type, infos);
            var dictionary = new Dictionary<Key, List<Val>>();

            var globalSet = new Dictionary<string, int>();

            foreach (var info in infos)
            {
                var key = new Key(info.ReturnType.ToString(), info.Name.Contains("get_", StringComparison.InvariantCulture) || info.Name.Contains("set_", StringComparison.InvariantCulture), info.ReturnType.IsGenericType);
                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = new List<Val>();
                }

                var s = info.Name;

                var containsKey = globalSet.ContainsKey(s);
                if (!containsKey)
                {
                    globalSet.Add(s, 0);
                }

                globalSet[s]++;
                if (globalSet.ContainsKey(info.Name))
                {
                    if (globalSet[info.Name] > 0)
                    {
                        s += globalSet[info.Name];
                    }
                }

                if (s.Contains("Values", StringComparison.InvariantCulture))
                {
                    Console.WriteLine();
                }

                var fullName = info.ToString();

                if (info.DeclaringType != null)
                {
                    fullName = info.ReturnType.Name + " " + info.DeclaringType + "." + info.Name;
                }

                dictionary[key].Add(new Val(s, fullName));
            }

            dictionary = dictionary.OrderBy(pair => !pair.Key.IsProperty).ThenBy(pair => pair.Key.ReturnType).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (KeyValuePair<Key, List<Val>> info in dictionary)
            {
                foreach (var val in info.Value)
                {
                    var generic = info.Key.IsGeneric ? "Generic" : string.Empty;

                    builder.AppendLine(tab + "/// <summary>");
                    builder.AppendLine(tab + $"/// Tests the \"{val.FullName}\" method.");
                    builder.AppendLine(tab + "/// </summary>");
                    builder.AppendLine(tab + "[TestMethod]");
                    builder.AppendLine(tab + "public void " + val.Name.Replace("set_", "Set", StringComparison.InvariantCulture).Replace("get_", "Get", StringComparison.InvariantCulture) + generic + "Test()");
                    builder.AppendLine(tab + "{");
                    builder.AppendLine(tab + tab + "using (IGridion gridion = GridionFactory.Start())");
                    builder.AppendLine(tab + tab + "{");
                    builder.AppendLine(tab + tab + "}");
                    builder.AppendLine(tab + "}");
                    builder.AppendLine();
                }
            }

            builder.AppendLine("}");
            builder.AppendLine();
            return builder;
        }

        private void GetInterfaceMethods(Type interfaceType, ISet<MethodInfo> list)
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
                this.GetInterfaceMethods(@base, list);
            }
        }

        private class Key : IComparable<Key>, IEquatable<Key>
        {
            public Key(string returnType, bool isProperty, bool isGeneric)
            {
                this.ReturnType = returnType;
                this.IsProperty = isProperty;
                this.IsGeneric = isGeneric;
            }

            public bool IsGeneric { get; }

            public bool IsProperty { get; }

            public string ReturnType { get; }

            /// <inheritdoc />
            public int CompareTo(Key other)
            {
                if (ReferenceEquals(this, other))
                {
                    return 0;
                }

                if (other is null)
                {
                    return 1;
                }

                var returnTypeComparison = string.Compare(this.ReturnType, other.ReturnType, StringComparison.Ordinal);
                return returnTypeComparison != 0 ? returnTypeComparison : this.IsProperty.CompareTo(other.IsProperty);
            }

            /// <inheritdoc />
            public bool Equals(Key other)
            {
                if (other is null)
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.IsGeneric == other.IsGeneric && this.IsProperty == other.IsProperty && this.ReturnType == other.ReturnType;
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

                return obj.GetType() == this.GetType() && this.Equals((Key)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return HashCode.Combine(this.IsGeneric, this.IsProperty, this.ReturnType);
            }
        }

        private class Val : IComparable<Val>
        {
            public Val(string name, string fullName)
            {
                this.Name = name;
                this.FullName = fullName;
            }

            public string FullName { get; private set; }

            public string Name { get; private set; }

            /// <inheritdoc />
            public int CompareTo(Val other)
            {
                if (ReferenceEquals(this, other))
                {
                    return 0;
                }

                if (other is null)
                {
                    return 1;
                }

                var nameComparison = string.Compare(this.Name, other.Name, StringComparison.Ordinal);
                return nameComparison != 0 ? nameComparison : string.Compare(this.FullName, other.FullName, StringComparison.Ordinal);
            }
        }
    }
}