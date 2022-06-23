using System;
using System.IO;
using System.Text;
using Syroot.BinaryData;
using Syroot.Maths;

namespace Nintendo.Aamp.IO
{
    // Thanks to Syroot for the IO and methods
    internal class FileReader : BinaryStream
    {
        internal FileReader(Stream stream, bool leaveOpen = false) : base(stream, ByteConverter.Big, Encoding.ASCII, leaveOpen: leaveOpen) => Position = 0;

        /// <summary>
        /// Checks the byte order mark to determine the endianness of the reader.
        /// </summary>
        /// <param name="ByteOrderMark">The byte order value being read. 0x01000000 = Little, 0x00000000 = Big. </param>
        /// <returns></returns>
        internal void CheckByteOrderMark(uint ByteOrderMark)
        {
            if (ByteOrderMark == 0x01000000)
                ByteConverter = ByteConverter.Little;
            else
                ByteConverter = ByteConverter.Big;
        }

        internal T? LoadCustom<T>(Func<T> callback, long? offset = null)
        {
            offset ??= ReadOffset();
            if (offset == 0) return default;

            using (TemporarySeek(offset.Value, SeekOrigin.Begin))
                return callback.Invoke();
        }

        internal string? LoadString()
        {
            long offset = ReadInt64();
            if (offset == 0) return null;

            using (TemporarySeek(offset, SeekOrigin.Begin))
            {
                ushort count = ReadUInt16();
                return ReadString(count);
            }
        }

        internal void CheckSignature(string validSignature)
        {
            // Read the actual signature and compare it.
            string signature = this.ReadString(sizeof(uint), Encoding.ASCII);

            if (signature != validSignature)
                throw new Exception($"Invalid signature, expected '{validSignature}' but got '{signature}'.");
        }

        internal long ReadOffset()
        {
            long offset = ReadInt64();
            return offset == 0 ? 0 : offset;
        }

        internal bool ReadIntBoolean() => ReadInt32() != 0;
        internal Color4F ReadColor4F() => new(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        internal Vector4F ReadVector4F() => new(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        internal Vector3F ReadVector3F() => new(ReadSingle(), ReadSingle(), ReadSingle());
        internal Vector2F ReadVector2F() => new(ReadSingle(), ReadSingle());
    }
}
