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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Gridion.Core.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     Represents a generator of tests for public distributed collections.
    /// </summary>
    [TestClass]
    public class DistributedCollectionTestGenerator
    {
        /// <summary>
        ///     Builds the tests for public collections.
        /// </summary>
        [TestMethod]
        public void BuildDictionaryTests()
        {
            Console.WriteLine(DistributedCollectionTestGenerator.Generate(typeof(IDistributedDictionary<object, object>), "IDistributedDictionary{T, V}", this.DefaultBodyBuilder, 'd'));
        }

        /// <summary>
        ///     Builds the tests for public collections.
        /// </summary>
        [TestMethod]
        public void BuildListTests()
        {
            Console.WriteLine(DistributedCollectionTestGenerator.Generate(typeof(IDistributedList<object>), "IDistributedList{T}", this.DefaultBodyBuilder, 'l'));
        }

        /// <summary>
        ///     Builds the tests for public collections.
        /// </summary>
        [TestMethod]
        public void BuildQueueTests()
        {
            Console.WriteLine(DistributedCollectionTestGenerator.Generate(typeof(AbstractDistributedQueue<object>), "IDistributedQueue{T}", this.DefaultBodyBuilder, 'q'));
        }

        /// <summary>
        ///     Builds the tests for public collections.
        /// </summary>
        [TestMethod]
        public void BuildSetTests()
        {
            Console.WriteLine(DistributedCollectionTestGenerator.Generate(typeof(IDistributedSet<object>), "IDistributedSet{T}", this.DefaultBodyBuilder, 's'));
        }

        /// <summary>
        ///     Builds the tests for public collections.
        /// </summary>
        [TestMethod]
        public void BuildTestForDistributedTypes()
        {
            this.BuildDictionaryTests();
            Console.WriteLine();
            this.BuildListTests();
            Console.WriteLine();
            this.BuildQueueTests();
            Console.WriteLine();
            this.BuildSetTests();
        }

        /// <summary>
        ///     Creates a code of a test.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="memberName">The name of a member.</param>
        /// <param name="defaultBodyBuilder">The builder of default stub.</param>
        /// <param name="id">The ID of type (dictionary, list, queue, set).</param>
        /// <returns>the code of a test.</returns>
        [SuppressMessage("ReSharper", "UseIndexFromEndExpression", Justification = "Reviewed.")]
        private static string Generate(Type type, string memberName, Func<char, string, string> defaultBodyBuilder, char id)
        {
            var tab = new string(' ', 4);
            var builder = new StringBuilder();
            var typeName = type.Name;
#pragma warning disable IDE0056 // Use index operator
            while (char.IsNumber(typeName[typeName.Length - 1]) || typeName[typeName.Length - 1] == '`')
#pragma warning restore IDE0056 // Use index operator
            {
#pragma warning disable IDE0057 // Use range operator
                typeName = typeName.Substring(0, typeName.Length - 1);
#pragma warning restore IDE0057 // Use range operator
            }

            if (typeName.StartsWith("I", StringComparison.Ordinal))
            {
#pragma warning disable IDE0057 // Use range operator
                typeName = typeName.Substring(1, typeName.Length - 1);
#pragma warning restore IDE0057 // Use range operator
            }

            builder.AppendLine("/// <summary>");
            builder.AppendLine($"/// Represents a set of test methods for <see cref=\"{memberName}\"/> interface.");
            builder.AppendLine("/// </summary>");
            builder.AppendLine("[TestClass]");
            builder.AppendLine(
                @"[System.Diagnostics.CodeAnalysis.SuppressMessage(""StyleCop.CSharp.DocumentationRules"", ""SA1650:ElementDocumentationMustBeSpelledCorrectly"", Justification = ""Reviewed."")]");
            builder.AppendLine("public class " + typeName + "Tests");
            builder.AppendLine("{");
            var infos = new HashSet<MethodInfo>();
            ReflectionUtils.GetInterfaceMethods(type, infos);
            var dictionary = new Dictionary<Key, List<Val>>();

            var globalSet = new Dictionary<string, int>();

            foreach (var info in infos)
            {
                var key = new Key(
                    info.ReturnType.ToString(),
                    info.Name.Contains("get_", StringComparison.InvariantCulture) || info.Name.Contains("set_", StringComparison.InvariantCulture),
                    info.ReturnType.IsGenericType);
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

                var paramInfo = ReflectionUtils.BuildMethodSignature(info);

                dictionary[key].Add(new Val(s, fullName, paramInfo));
            }

            dictionary = dictionary.OrderBy(pair => !pair.Key.IsProperty).ThenBy(pair => pair.Key.ReturnType).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (KeyValuePair<Key, List<Val>> info in dictionary)
            {
                foreach (var val in info.Value)
                {
                    var generic = info.Key.IsGeneric ? "Generic" : string.Empty;

                    builder.AppendLine(tab + "/// <summary>");
                    builder.AppendLine(tab + $"/// Tests the \"{val.FullName}\" ");
                    builder.AppendLine(tab + "/// " + tab + $"{val.ParamInfo}");
                    builder.AppendLine(tab + "/// method.");
                    builder.AppendLine(tab + "/// </summary>");
                    builder.AppendLine(tab + "[TestMethod]");
                    builder.AppendLine(
                        tab + "public void "
                            + val.Name.Replace("set_", "Set", StringComparison.InvariantCulture).Replace("get_", "Get", StringComparison.InvariantCulture) + generic
                            + "Test()");
                    builder.AppendLine(tab + "{");
                    builder.AppendLine(tab + tab + "using (IGridion gridion = GridionFactory.Start())");
                    builder.AppendLine(tab + tab + "{");
                    builder.Append(defaultBodyBuilder(id, tab + tab + tab));
                    builder.AppendLine(tab + tab + "}");
                    builder.AppendLine(tab + "}");
                    builder.AppendLine();
                }
            }

            builder.AppendLine("}");
            builder.AppendLine();

            return builder.ToString();
        }

        /// <summary>
        ///     Represents a builder of default body.
        /// </summary>
        /// <param name="id">The ID of type (dictionary, list, queue, set).</param>
        /// <param name="tab">The tabulation.</param>
        /// <returns>the default body.</returns>
        private string DefaultBodyBuilder(char id, string tab)
        {
            var sb = new StringBuilder();
            switch (id)
            {
                case 'd':
                    sb.AppendLine(tab + @"IDistributedDictionary<string, int> dictionary = gridion.GetDictionary<string, int>(""testDictionary"");");
                    sb.AppendLine(tab + @"dictionary.AddOrUpdate(""key"", 1, (s, i) => 1);");
                    break;
                case 'l':
                    sb.AppendLine(tab + @"IDistributedList<string> list = unused.GetList<string>(""testList"");");
                    sb.AppendLine(tab + @"list.Add(""val"");");
                    break;
                case 'q':
                    sb.AppendLine(tab + @"IDistributedQueue<string> queue = unused.GetQueue<string>(""testQueue"");");
                    sb.AppendLine(tab + @"queue.Enqueue(""val"");");
                    break;
                case 's':
                    sb.AppendLine(tab + @"IDistributedSet<string> set = unused.GetSet<string>(""testSet"");");
                    sb.AppendLine(tab + @"set.Add(""val"");");
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Wraps properties of types that are used to generate tests.
        /// </summary>
        /// <inheritdoc cref="IEquatable{T}" />
        /// <inheritdoc cref="IComparable{T}" />
        private class Key : IComparable<Key>, IEquatable<Key>
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="Key" /> class.
            /// </summary>
            /// <param name="returnType">
            ///     The return type.
            /// </param>
            /// <param name="isProperty">
            ///     Indicates whether object is property.
            /// </param>
            /// <param name="isGeneric">
            ///     Indicates whether object is generic.
            /// </param>
            internal Key(string returnType, bool isProperty, bool isGeneric)
            {
                this.ReturnType = returnType;
                this.IsProperty = isProperty;
                this.IsGeneric = isGeneric;
            }

            /// <summary>
            ///     Gets a value indicating whether a property or a method is generic.
            /// </summary>
            internal bool IsGeneric { get; }

            /// <summary>
            ///     Gets a value indicating whether the object is a property.
            /// </summary>
            internal bool IsProperty { get; }

            /// <summary>
            ///     Gets a return type.
            /// </summary>
            internal string ReturnType { get; }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }

                if (object.ReferenceEquals(this, obj))
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

            /// <inheritdoc />
            int IComparable<Key>.CompareTo(Key other)
            {
                if (object.ReferenceEquals(this, other))
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
            bool IEquatable<Key>.Equals(Key other)
            {
                if (other is null)
                {
                    return false;
                }

                if (object.ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.IsGeneric == other.IsGeneric && this.IsProperty == other.IsProperty && this.ReturnType == other.ReturnType;
            }
        }

        /// <summary>
        ///     Wraps properties of types that are used to generate tests.
        /// </summary>
        /// <inheritdoc />
        private class Val : IComparable<Val>
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="Val" /> class.
            /// </summary>
            /// <param name="name">
            ///     The name.
            /// </param>
            /// <param name="fullName">
            ///     The full name.
            /// </param>
            /// <param name="paramInfo">
            ///     The parameter info.
            /// </param>
            internal Val(string name, string fullName, string paramInfo)
            {
                this.Name = name;
                this.FullName = fullName;
                this.ParamInfo = paramInfo;
            }

            /// <summary>
            ///     Gets the full name.
            /// </summary>
            internal string FullName { get; private set; }

            /// <summary>
            ///     Gets the name.
            /// </summary>
            internal string Name { get; private set; }

            /// <summary>
            ///     Gets the parameter info.
            /// </summary>
            internal string ParamInfo { get; }

            /// <inheritdoc />
            int IComparable<Val>.CompareTo(Val other)
            {
                if (object.ReferenceEquals(this, other))
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