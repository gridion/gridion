// <copyright file="LoggerExtensionsTests.cs" company="Gridion">
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
    using System.Diagnostics.CodeAnalysis;

    using Gridion.Core.Extensions;
    using Gridion.Internal.Logging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NSubstitute;

    /// <summary>
    ///     Represents a set of tests for a testing of <see cref="LoggerExtensions" /> class.
    /// </summary>
    [TestClass]
    public class LoggerExtensionsTests
    {
        /// <summary>
        ///     Tests a <see cref="LoggerExtensions" /> Info method.
        /// </summary>
        [TestMethod]
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute", Justification = "Reviewed. Suppression is OK here.")]
        public void LoggerExtensionsInfoTest()
        {
            var logger = Substitute.For<ILogger>();
            Assert.ThrowsException<ArgumentNullException>(() => LoggerExtensions.Info(null, null));
            Assert.ThrowsException<ArgumentNullException>(() => LoggerExtensions.Info(logger, null));
            LoggerExtensions.Info(logger, string.Empty);
            logger.Received().Log(LogLevel.Info, string.Empty, null, null, null);
#pragma warning disable CA1303 // Do not pass literals as localized parameters
            LoggerExtensions.Info(logger, "message");
            logger.Received().Log(LogLevel.Info, "message", null, null, null);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        }
    }
}