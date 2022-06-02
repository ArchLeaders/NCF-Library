using Nintendo.Aamp.IO;
using Syroot.BinaryData;
using System.Text;

namespace Nintendo.Aamp.Parser
{
    internal class AampFileV1 : AampFile
    {
        public AampFileV1() { }
        public AampFileV1(string FileName) => Read(new FileReader(new FileStream(FileName, FileMode.Open)));
        public AampFileV1(Stream Stream) => Read(new FileReader(Stream));

        internal byte[]? EffectName { get; set; }

        internal void Read(FileReader reader)
        {
            reader.ByteConverter = ByteConverter.Little;
            reader.CheckSignature("AAMP");
            Version = reader.ReadUInt32();
            Endianness = reader.ReadUInt32();

            uint _ = reader.ReadUInt32();
            ParameterIOVersion = reader.ReadUInt32();
            uint nameLength = reader.ReadUInt32();
            long pos = reader.Position;
            EffectName = reader.ReadBytes((int)nameLength);

            //read the string as zero terminated for now
            reader.Seek(pos, SeekOrigin.Begin);
            ParameterIOType = reader.ReadString(StringCoding.ZeroTerminated);

            reader.Seek(pos + nameLength, SeekOrigin.Begin);
            RootNode = ParamListV1.Read(reader);
        }

        internal byte[] CompileV1()
        {
            MemoryStream ms = new();
            CompileV1(new(ms));
            return ms.ToArray();
        }

        internal void CompileV1(FileWriter writer)
        {
            writer.ByteConverter = ByteConverter.Little;
            writer.Write(Encoding.ASCII.GetBytes("AAMP"));
            writer.Write(Version);
            writer.Write(Endianness);

            long sizeOffset = writer.Position;
            writer.Write(0); //Write the file size later
            writer.Write(ParameterIOVersion); 
            writer.Write(AlignUp(ParameterIOType.Length + 1, 4));
            writer.Write(ParameterIOType, StringCoding.ZeroTerminated);
            writer.Align(4);

            ParamListV1.Write(RootNode, writer);

            //Write end of file
            writer.Seek(sizeOffset, System.IO.SeekOrigin.Begin);
            uint FileSize = (uint)writer.BaseStream.Length;
            writer.Write(FileSize);

            writer.Close();
            writer.Dispose();
        }

        private static int AlignUp(int n, int align) => (n + align - 1) & -align;
    }
}
