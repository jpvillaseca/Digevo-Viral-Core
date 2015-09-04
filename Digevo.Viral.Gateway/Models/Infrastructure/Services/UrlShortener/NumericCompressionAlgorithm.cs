using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Digevo.Viral.Gateway.Models.Infrastructure.Services.UrlShortener
{
    /// <summary>
    /// Lossless compression algorithm to shorten numeric messages with characters
    /// </summary>
    public static class NumericCompressionAlgorithm
    {
        private const string dictionary = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static long Decompress(string number)
        {
            int radix = dictionary.Length;

            if (String.IsNullOrEmpty(number))
                return 0;

            long result = 0;
            long multiplier = 1;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                char c = number[i];
                if (i == 0 && c == '-')
                {
                    // This is the negative sign symbol
                    result = -result;
                    break;
                }

                int digit = dictionary.IndexOf(c);
                if (digit == -1)
                    throw new ArgumentException(
                        "Invalid character in the arbitrary numeral system number",
                        "number");

                result += digit * multiplier;
                multiplier *= radix;
            }

            return result;
        }

        public static string Compress(long decimalNumber)
        {
            int radix = dictionary.Length;
            const int BitsInLong = 64;

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = dictionary[remainder];
                currentNumber = currentNumber / radix;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }
    }
}