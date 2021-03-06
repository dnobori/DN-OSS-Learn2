// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace Microsoft.Data.SqlClient
{
    /// <summary>
    /// This is a factory class for AEAD_AES_256_CBC_HMAC_SHA256
    /// </summary>
    internal class SqlAeadAes256CbcHmac256Factory : SqlClientEncryptionAlgorithmFactory
    {
        /// <summary>
        /// Factory classes caches the SqlAeadAes256CbcHmac256EncryptionKey objects to avoid computation of the derived keys
        /// </summary>
        private readonly ConcurrentDictionary<string, SqlAeadAes256CbcHmac256Algorithm> _encryptionAlgorithms =
            new ConcurrentDictionary<string, SqlAeadAes256CbcHmac256Algorithm>(concurrencyLevel: 4 * Environment.ProcessorCount /* default value in ConcurrentDictionary*/, capacity: 2);

        /// <summary>
        /// Creates an instance of AeadAes256CbcHmac256Algorithm class with a given key
        /// </summary>
        /// <param name="encryptionKey">Root key</param>
        /// <param name="encryptionType">Encryption Type. Expected values are either Deterministic or Randomized.</param>
        /// <param name="encryptionAlgorithm">Encryption Algorithm.</param>
        /// <returns></returns>
        internal override SqlClientEncryptionAlgorithm Create(SqlClientSymmetricKey encryptionKey, SqlClientEncryptionType encryptionType, string encryptionAlgorithm)
        {
            // Callers should have validated the encryption algorithm and the encryption key
            Debug.Assert(encryptionKey != null);
            Debug.Assert(string.Equals(encryptionAlgorithm, SqlAeadAes256CbcHmac256Algorithm.AlgorithmName, StringComparison.OrdinalIgnoreCase) == true);

            // Validate encryption type
            if (!((encryptionType == SqlClientEncryptionType.Deterministic) || (encryptionType == SqlClientEncryptionType.Randomized)))
            {
                throw SQL.InvalidEncryptionType(SqlAeadAes256CbcHmac256Algorithm.AlgorithmName,
                                                encryptionType,
                                                SqlClientEncryptionType.Deterministic,
                                                SqlClientEncryptionType.Randomized);
            }

            // Get the cached encryption algorithm if one exists or create a new one, add it to cache and use it
            //
            // For now, we only have one version. In future, we may need to parse the algorithm names to derive the version byte.
            const byte algorithmVersion = 0x1;

            StringBuilder algorithmKeyBuilder = new StringBuilder(Convert.ToBase64String(encryptionKey.RootKey), SqlSecurityUtility.GetBase64LengthFromByteLength(encryptionKey.RootKey.Length) + 4/*separators, type and version*/);

#if DEBUG
            int capacity = algorithmKeyBuilder.Capacity;
#endif //DEBUG

            algorithmKeyBuilder.Append(":");
            algorithmKeyBuilder.Append((int)encryptionType);
            algorithmKeyBuilder.Append(":");
            algorithmKeyBuilder.Append(algorithmVersion);

            string algorithmKey = algorithmKeyBuilder.ToString();

#if DEBUG
            Debug.Assert(algorithmKey.Length <= capacity, "We needed to allocate a larger array");
#endif //DEBUG

            SqlAeadAes256CbcHmac256Algorithm aesAlgorithm;
            if (!_encryptionAlgorithms.TryGetValue(algorithmKey, out aesAlgorithm))
            {
                SqlAeadAes256CbcHmac256EncryptionKey encryptedKey = new SqlAeadAes256CbcHmac256EncryptionKey(encryptionKey.RootKey, SqlAeadAes256CbcHmac256Algorithm.AlgorithmName);
                aesAlgorithm = new SqlAeadAes256CbcHmac256Algorithm(encryptedKey, encryptionType, algorithmVersion);

                // In case multiple threads reach here at the same time, the first one adds the value
                // the second one will be a no-op, the allocated memory will be claimed by Garbage Collector.
                _encryptionAlgorithms.TryAdd(algorithmKey, aesAlgorithm);
            }

            return aesAlgorithm;
        }
    }
}
