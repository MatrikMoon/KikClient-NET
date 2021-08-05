using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KikClient.Utilities
{
    public class KikUtilities
    {
        private static Random random = new Random();

        public static string GenerateDeviceID()
        {
            return "CAN" + Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string GenerateAndroidID()
        {
            string str = "";
            for (int i = 0; i < 16; i++)
            {
                str = str + "1234567890abcdef".ElementAt(random.Next(0, 16));
            }
            return str;
        }

        //Translated from kik source
        public static string GenerateKikUUID()
        {
            Func<long, int, int> sub_func = (long a, int b) =>
            {
                if (b > 32)
                {
                    return (int)(a >> 32 & 1 << b) >> b;
                }
                return (int)(1 << b & a) >> b;
            };

            var uuid_bytes = Guid.NewGuid().ToByteArray();

            Array.Reverse(uuid_bytes);

            var least_significant_bits = BitConverter.ToInt64(uuid_bytes, 0);

            //Microsoft stores guids weird. We need to swap some things around
            //so that what we get here would match the java equivalent of `getMostSignificantBits()`
            var first_segment = uuid_bytes.Skip(8 + 4).Take(4).ToArray();
            var second_segment = uuid_bytes.Skip(8 + 2).Take(2).ToArray();
            var third_segment = uuid_bytes.Skip(8).Take(2).ToArray();

            Array.Reverse(first_segment);
            Array.Reverse(second_segment);
            Array.Reverse(third_segment);

            var uuid_bytes_part_two = third_segment.Concat(second_segment).Concat(first_segment);

            var most_significant_bits = BitConverter.ToInt64(uuid_bytes_part_two.ToArray(), 0);
            var tuple_selector = (int)((ulong)(-1152921504606846976 & most_significant_bits) >> 62);
            var tuples = new List<int[]> {
                new int[] { 3, 6 },
                new int[] { 2, 5 },
                new int[] { 7, 1 },
                new int[] { 9, 5 }
            };

            var selected_tuple_1 = tuples[tuple_selector][0];
            var selected_tuple_2 = tuples[tuple_selector][1];

            var base_multiplier = sub_func(most_significant_bits, selected_tuple_1);
            var shift_base = sub_func(most_significant_bits, selected_tuple_2);

            var shifter = 1;
            for (var loop_iterations = 0; loop_iterations < 6; loop_iterations++)
            {
                shifter = (shifter + (shift_base + 1 | base_multiplier << 1) * 7) % 60;
                least_significant_bits = least_significant_bits & (1 << shifter + 2 ^ -1) | (long)sub_func((long)(((ulong)(-16777216 & most_significant_bits) >> 22) ^ ((ulong)(0xFF0000 & most_significant_bits) >> 16) ^ ((ulong)(65280 & most_significant_bits) >> 8)), loop_iterations) << shifter + 2;
            }

            //Reorganize bytes so that we can parse them as a string
            var most_significant_bytes = BitConverter.GetBytes(most_significant_bits);
            Array.Reverse(most_significant_bytes);

            var least_significant_bytes = BitConverter.GetBytes(least_significant_bits);
            Array.Reverse(least_significant_bytes);

            return Guid.Parse(BytesToString(most_significant_bytes) + BytesToString(least_significant_bytes)).ToString();
        }

        //Translated from kik source
        public static long GenerateKikTimestamp()
        {
            var timeInMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            long i1 = 30 & ((65280 & timeInMilliseconds) >> 8 ^ (0xFF0000 & timeInMilliseconds) >> 16 ^ (-16777216 & timeInMilliseconds) >> 24);
            long i2 = (224 & timeInMilliseconds) >> 5;
            
            if (i1 % 4 == 0) i2 = i2 / 3 * 3;
            else i2 = i2 / 2 * 2;

            return i2 << 5 | -255 & timeInMilliseconds | i1;
        }

        public static string BytesToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
        }

        public static string SHA1Hash(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return Hash(sha1, input);
            }
        }

        public static string SHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return Hash(sha256, input);
            }
        }

        public static string MD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                return Hash(md5, input);
            }
        }

        private static string Hash(HashAlgorithm algorithm, string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = algorithm.ComputeHash(inputBytes);
            return BytesToString(hashBytes);
        }

        public static byte[] PBKDF2Hash(string input, byte[] salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, 8192);
            return pbkdf2.GetBytes(16);
        }

        public static string HMACSHA1(string input, string secret)
        {
            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(input)))
            {
                return BytesToString(hmac.ComputeHash(stream));
            }
        }

        public static string HashPassword(string username, string password)
        {
            return BytesToString(
                PBKDF2Hash(
                    SHA1Hash(password),
                    Encoding.UTF8.GetBytes(username.ToLower() + "niCRwL7isZHny24qgLvy")
                )
            );
        }

        public static string SignValues(string uuid, string timestamp, string versionName, string jid)
        {
            var keyBytes = Convert.FromBase64String("MIIBVgIBADANBgkqhkiG9w0BAQEFAASCAUAwggE8AgEAAkEA0RZQQg2pXUo0btiJ\n70ZIzy3vlm91N6pPuQ4XjSS8Mcin8Le1fZtw2AtOcYWzzIDabanuEqgUujGHri9n\nHl9nKQIDAQABAkBP+ELWILeIcNtBEh0foTgz1ZPva83fbopzcwpa95PrTexQBYWV\noRrlPzQYGI/+pe309oOglZx0oevtGoOr7yehAiEA+HmFpNIa7QwWzRiItEuqKslZ\ndrhA+bhbmfPlUYpdoq0CIQDXa2lSWTLEkG64oLKQhBuJRccTDMVhswcrkT+4aQWh\n7QIhALq5iAc+pWFybkgeoczr96tDuOmQubNwKdZeBPzsAEXZAiEAjOt/IpenVl8F\nj1HQfiltugcji5q3JIpxDlceUAyj2qECIQDjfO4gySclIbBrbMu3/cWZWe4IicPo\n66fl1txieqtomg==");

            byte[] input = Encoding.UTF8.GetBytes($"{jid}:{versionName}:{timestamp}:{uuid}");
            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(keyBytes, out var bytesRead);
                return Convert.ToBase64String(rsa.SignData(input, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)).Replace("=", "").Replace("/", "_").Replace("+", "-");
            }
        }

        //Heavily borrowed from https://github.com/tomer8007/kik-bot-api-unofficial/blob/b28287334a0315353cf2b025c106c838b60d1649/kik_unofficial/utilities/cryptographic_utilities.py
        public static string BuildStringFromKStanzaDictionary(Dictionary<string, string> dictionary)
        {
            var orderedDictionary = new OrderedDictionary();
            var keyList = dictionary.Keys.Select(x => x.ToString()).OrderBy(x => x).ToList();

            var spaces = HashStanzaDictionary(dictionary, -310256979, 13) % 29;
            if (spaces < 0) spaces += 29;

            var originalSize = dictionary.Count;

            for (int i = 0; i < originalSize; i++)
            {
                var hashInstance = HashStanzaDictionary(dictionary, -1964139357, 7);
                if (hashInstance > 0) hashInstance %= dictionary.Count;
                else hashInstance %= -dictionary.Count;

                if (hashInstance < 0) hashInstance += dictionary.Count;
                var selectedKey = keyList.ElementAt(hashInstance);
                orderedDictionary.Add(selectedKey, dictionary[selectedKey]);
                keyList.RemoveAt(hashInstance);
                dictionary.Remove(selectedKey);
            }

            var kStanza = "<k";
            foreach (var key in orderedDictionary.Keys) kStanza += $" {key}=\"{orderedDictionary[key]}\"";
            kStanza += ">";

            return string.Concat(Enumerable.Repeat(" ", spaces)) + kStanza;
        }

        private static int HashStanzaDictionary(Dictionary<string, string> stanza, int hashBase, int hashOffset)
        {
            var keys = stanza.Keys.Select(x => x.ToString()).OrderBy(x => x);

            var conjoinedKeyValues = string.Empty;
            byte[] conjoinedKeyValueBytes = null;
            foreach (var key in keys) conjoinedKeyValues += $"{key}{stanza[key]}";
            conjoinedKeyValueBytes = Encoding.UTF8.GetBytes(conjoinedKeyValues);

            var reversedConjoinedKeyValues = string.Empty;
            byte[] reversedConjoinedKeyValueBytes = null;
            foreach (var key in keys.OrderByDescending(x => x)) reversedConjoinedKeyValues += $"{key}{stanza[key]}";
            reversedConjoinedKeyValueBytes = Encoding.UTF8.GetBytes(reversedConjoinedKeyValues);

            var hashResults = new List<int>();
            hashResults.Add(HashStanzaBytes(0, conjoinedKeyValueBytes));
            hashResults.Add(HashStanzaBytes(2, reversedConjoinedKeyValueBytes));
            hashResults.Add(HashStanzaBytes(1, conjoinedKeyValueBytes));

            return hashBase ^ HashStanzaBytes(0, conjoinedKeyValueBytes) << 
                hashOffset ^ HashStanzaBytes(2, reversedConjoinedKeyValueBytes) << 
                (hashOffset * 2) ^ HashStanzaBytes(1, conjoinedKeyValueBytes) << 
                hashOffset ^ HashStanzaBytes(0, conjoinedKeyValueBytes);
        }

        private static int HashStanzaBytes(int type, byte[] input)
        {
            long hashCode = 0;
            byte[] hashedBytes = null;

            switch (type)
            {
                case 0:
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        hashedBytes = sha256.ComputeHash(input);
                    }
                    break;
                case 1:
                    using (SHA1 sha1 = SHA1.Create())
                    {
                        hashedBytes = sha1.ComputeHash(input);
                    }
                    break;
                case 2:
                    using (MD5 md5 = MD5.Create())
                    {
                        hashedBytes = md5.ComputeHash(input);
                    }
                    break;
            }

            for (int i = 0; i < hashedBytes.Length; i += 4)
            {
#pragma warning disable CS0675 // Bitwise-or operator used on a sign-extended operand
                hashCode ^= (((sbyte)hashedBytes[i + 3]) << 24) | (((sbyte)hashedBytes[i + 2]) << 16) | (((sbyte)hashedBytes[i + 1]) << 8) | (sbyte)hashedBytes[i];
#pragma warning restore CS0675 // Bitwise-or operator used on a sign-extended operand
            }
            return (int)hashCode;
        }
    }
}
