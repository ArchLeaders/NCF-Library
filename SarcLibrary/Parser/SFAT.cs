// SARC IO Pulled from EditorCore : https://github.com/exelix11/EditorCore

using Syroot.BinaryData;
using System.Collections.Generic;

namespace Nintendo.Sarc.Parser
{
    internal class SFAT
    {
        internal List<Node> Nodes { get; set; } = new();
        internal ushort ChunkSize { get; set; }
        internal ushort NodeCount { get; set; }
        internal uint HashMultiplier { get; set; }

        internal struct Node
        {
            public uint Hash { get; set; }
            public byte FileBool { get; set; }
            public byte Unknown1 { get; set; }
            public ushort FileNameOffset { get; set; }
            public uint NodeOffset { get; set; }
            public uint EON { get; set; }
        }

        internal void Parse(BinaryStream binaryStream)
        {
            binaryStream.ReadUInt32(); // Header;
            ChunkSize = binaryStream.ReadUInt16();
            NodeCount = binaryStream.ReadUInt16();
            HashMultiplier = binaryStream.ReadUInt32();

            for (int i = 0; i < NodeCount; i++)
            {
                Node node = new();
                node.Hash = binaryStream.ReadUInt32();
                //node.fileBool = br.ReadByte();
                //node.unknown1 = br.ReadByte();
                //node.fileNameOffset = br.ReadUInt16();
                var attributes = binaryStream.ReadUInt32();
                node.FileBool = (byte)(attributes >> 24);
                node.Unknown1 = (byte)((attributes >> 16) & 0xFF);
                node.FileNameOffset = (ushort)(attributes & 0xFFFF);
                node.NodeOffset = binaryStream.ReadUInt32();
                node.EON = binaryStream.ReadUInt32();
                Nodes.Add(node);
            }
        }
    }
}
