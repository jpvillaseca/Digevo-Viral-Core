using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.UrlShortener
{
    /// <summary>
    /// Lossless compression algorithm to shorten numeric messages with characters
    /// </summary>
    public static class NumericCompressionAlgorithm
    {
        private static string dict = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static long Decompress(string compressed)
        {
            long number = 0;

            for (int i = 0; i < compressed.Length - 1; i++)
                number += (long)Math.Pow(dict.IndexOf(compressed[i]), (4 - i) >= 2 ? (4 - i) : 2);

            number += dict.IndexOf(compressed[compressed.Length - 1]);

            return number;
        }

        public static string Compress(long number)
        {
            var compressed = "";

            //Iterate over maximum allowed factor
            for (int j = 4; j > 1 || (number > dict.Length - 1); j--)
                for (int i = dict.Length - 1; i >= 0; i--)
                    if (number >= Math.Pow(i, j >= 2 ? j : 2))
                    {
                        compressed += dict[i].ToString(); //Expand word
                        number -= (long)Math.Pow(i, j >= 2 ? j : 2);
                        break;
                    }

            compressed += dict[(int)number].ToString();

            return compressed;
        }
    }
}