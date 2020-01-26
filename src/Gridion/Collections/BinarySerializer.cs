// <copyright file="BinarySerializer.cs" company="Gridion">
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

namespace Gridion.Core.Collections
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Represents a common binary serializer.
    /// </summary>
    internal static class BinarySerializer
    {
        /// <summary>
        /// Deserializes a byte array into object.
        /// </summary>
        /// <param name="arr">The bytes to deserialize.</param>
        /// <returns>a deserialized object.</returns>
        internal static object Deserialize(this byte[] arr)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                byte[] decompressed = Decompress(arr);

                memoryStream.Write(decompressed, 0, decompressed.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Serializes the object into a byte array.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>a read-only list of bytes.</returns>
        internal static IReadOnlyList<byte> Serialize(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(memoryStream, obj);

                IReadOnlyList<byte> compressed = Compress(memoryStream.ToArray());
                return compressed;
            }
        }

        /// <summary>
        /// Decompresses the byte array.
        /// </summary>
        /// <param name="input">The array to decompress.</param>
        /// <returns>a decompressed array.</returns>
        private static byte[] Decompress(byte[] input)
        {
            byte[] decompressedData;

            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream(input))
                {
                    using (var zip = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        zip.CopyTo(outputStream);
                    }
                }

                decompressedData = outputStream.ToArray();
            }

            return decompressedData;
        }

        /// <summary>
        /// Compresses the byte array.
        /// </summary>
        /// <param name="input">The array to compress.</param>
        /// <returns>a compressed array.</returns>
        private static IReadOnlyList<byte> Compress(byte[] input)
        {
            IReadOnlyList<byte> compressesData;

            using (var outputStream = new MemoryStream())
            {
                using (var zip = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    zip.Write(input, 0, input.Length);
                }

                compressesData = outputStream.ToArray();
            }

            return compressesData;
        }
    }
}