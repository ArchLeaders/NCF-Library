using Syroot.BinaryData;
using System;
using System.IO;

namespace Nintendo.Byml.IO
{
    /// <summary>
    /// BinaryStream extension methods
    /// </summary>
    public static class BinaryStreamExtension
    {
        internal static void SatisfyOffset(this BinaryStream stream, uint offset, uint value)
        {
            using (stream.TemporarySeek(offset, SeekOrigin.Begin))
                stream.Write(value);
        }

        internal static uint ReserveOffset(this BinaryStream stream)
        {
            var pos = (uint)stream.Position;
            stream.Position += 4L;
            return pos;
        }

        internal static NodeType PeekNodeType(this BinaryStream stream)
        {
            using (stream.TemporarySeek())
            {
                // If the offset is invalid, the type cannot be determined.
                uint offset = stream.ReadUInt32();
                if (offset > 0 && offset < stream.BaseStream.Length)
                {
                    // Seek to the offset and try to read a valid type.
                    stream.Position = offset;
                    NodeType nodeType = (NodeType)stream.ReadByte();
                    if (Enum.IsDefined(typeof(NodeType), nodeType))
                        return nodeType;
                }
            }
            return NodeType.None;
        }
    }
}
