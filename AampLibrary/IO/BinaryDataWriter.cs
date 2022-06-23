using System.IO;
using System.Text;
using Syroot.BinaryData;
using Syroot.Maths;

namespace Nintendo.Aamp.IO
{
    internal class FileWriter : BinaryStream
    {
        internal FileWriter(Stream stream, bool leaveOpen = false) : base(stream, ByteConverter.Big, Encoding.ASCII, leaveOpen: leaveOpen) => Position = 0;

        internal void WriteSize(long EndPosition, long startPosition)
        {
            using (TemporarySeek(startPosition, SeekOrigin.Begin))
                Write((uint)(EndPosition - startPosition));
        }

        internal new void WriteBoolean(bool boolean) => Write(boolean == false ? 0 : 1);

        internal void WriteVector2F(Vector2F vector2F)
        {
            Write(vector2F.X);
            Write(vector2F.Y);
        }

        internal void WriteVector3F(Vector3F vector3F)
        {
            Write(vector3F.X);
            Write(vector3F.Y);
            Write(vector3F.Z);
        }

        internal void WriteVector4F(Vector4F vector4F)
        {
            Write(vector4F.X);
            Write(vector4F.Y);
            Write(vector4F.Z);
            Write(vector4F.W);
        }

        internal void WriteColor4F(Color4F color4F)
        {
            Write(color4F.R);
            Write(color4F.G);
            Write(color4F.B);
            Write(color4F.A);
        }

        /// <summary>
        /// Checks the byte order mark to determine the endianness of the writer.
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
    }
}
