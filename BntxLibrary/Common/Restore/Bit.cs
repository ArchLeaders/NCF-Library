using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BntxLibrary.Common.Restore
{
    internal static class Bit
    {
        internal static string ToBinaryString(this string text, Encoding encoding)
        {
            return string.Join("", encoding.GetBytes(text).Select(n => Convert.ToString(n, 2).PadLeft(8, '0')));
        }

        /// <remarks>
        /// <b>Remarks:</b><br/>
        /// - Bit index data is too big.
        /// </remarks>
        internal static int GetBit(this BigInteger n, int b)
        {
            BigInteger test = (n >> (int)(b & 0xffffffff)) & 1;
            return (int)test;
        }

        internal static int GetFirst1Bit(this BigInteger n)
        {
            int bitLength = GetBitLength(n);
            for (int i = 0; i < bitLength; i++)
            {
                if (((n >> i) & 1) == 1)
                {
                    return i;
                }
            }
            throw new InvalidOperationException($"Could not get first 1 bit in BitInteger '{n}'.");
        }
        internal static int GetBitLength(this BigInteger bits)
        {
            int bitLength = 0;
            while (bits / 2 != 0)
            {
                bits /= 2;
                bitLength++;
            }
            return bitLength += 1;
        }

        internal static int Mismatch(BigInteger int1, BigInteger int2)
        {
            int bitlength1 = GetBitLength(int1);
            int bitlength2 = GetBitLength(int2);

            for (int i = 0; i < Math.Max(bitlength1, bitlength2); i++)
            {
                if (((int1 >> i) & 1) != ((int2 >> i) & 1))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
