using System;
using System.Text;
using Syroot.BinaryData;

namespace Nintendo.Bfres.Core
{
    public static class BinaryStreamExtensions
    {
        public static void Write<T>(this BinaryStream stream, T value, bool strict) where T : struct, IComparable, IFormattable => stream.WriteEnum(typeof(T), value, strict);
        public static void Write(this BinaryStream stream, char[] chars)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));

            byte[] bytes = stream.Encoding.GetBytes(chars, 0, chars.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static string ReadString(this BinaryStream stream, int length, Encoding encoding) => encoding.GetString(stream.ReadBytes(length));
    }
}
