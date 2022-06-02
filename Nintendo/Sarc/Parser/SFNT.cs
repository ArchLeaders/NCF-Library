// SARC IO Pulled from EditorCore : https://github.com/exelix11/EditorCore

using Syroot.BinaryData;
using System.Text;

namespace Nintendo.Sarc.Parser
{
    internal class SFNT
    {
        internal List<string> FileNames { get; set; } = new();
        internal uint ChunkID { get; set; }
        internal ushort ChunkSize { get; set; }
        internal ushort Unknown1 { get; set; }

        internal void Parse(BinaryStream binaryStream, int pos, SFAT sfat, int start)
        {
            ChunkID = binaryStream.ReadUInt32();
            ChunkSize = binaryStream.ReadUInt16();
            Unknown1 = binaryStream.ReadUInt16();

            byte[] tempBytes = binaryStream.ReadBytes(start - (int)binaryStream.BaseStream.Position);
            string tempString = Encoding.UTF8.GetString(tempBytes);
            char[] splitter = { (char)0x00 };
            string[] names = tempString.Split(splitter);

            for (int j = 0; j < names.Length; j++)
                if (names[j] != "")
                    FileNames.Add(names[j]);
        }
    }
}
