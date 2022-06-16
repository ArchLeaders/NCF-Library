using System.IO;
using Nintendo.Bfres.Core;
using Syroot.BinaryData;
using Syroot.BinaryData.Core;

namespace Nintendo.Bfres
{
    public class Brtcamera
    {
        public byte[] Name { get; set; }
        public uint FrameCount { get; set; }
        public KeyFrame[] KeyFrames { get; set; }
        public float UnknownValue { get; set; }
        public bool IsBigEndian { get; set; } = true;

        public Brtcamera(Stream stream, bool bigEndian) {
            IsBigEndian = bigEndian;
            using (var reader = new BinaryStream(stream)) {
                Read(reader);
            }
        }

        public void Save(Stream stream)
        {
            using (var writer = new BinaryStream(stream)) {
                Write(writer);
            }
        }

        private void Read(BinaryStream reader)
        {
            reader.ByteConverter = ByteConverter.Little;
            if (IsBigEndian)
                reader.ByteConverter = ByteConverter.Big;

            Name = reader.ReadBytes(64);
            uint count = reader.ReadUInt32();
            FrameCount = reader.ReadUInt32();
            UnknownValue = reader.ReadSingle();

            KeyFrames = new KeyFrame[count];
            for (int i = 0; i < count; i++)
            {
                KeyFrames[i] = new KeyFrame()
                {
                    Flag = reader.ReadUInt32(),
                    Frame = reader.ReadSingle(),
                    Data = reader.ReadSingles(8),
                };
            }
        }

        private void Write(BinaryStream writer)
        {
            writer.ByteConverter = ByteConverter.Little;
            if (IsBigEndian)
                writer.ByteConverter = ByteConverter.Big;
            writer.Write(Name);
            writer.Write(KeyFrames.Length);
            writer.Write(FrameCount);
            writer.Write(UnknownValue);
            for (int i = 0; i < KeyFrames.Length; i++)
            {
                writer.Write(KeyFrames[i].Flag);
                writer.Write(KeyFrames[i].Frame);
                writer.Write(KeyFrames[i].Data);
            }
        }

        public class KeyFrame
        {
            public uint Flag { get; set; }
            public float Frame { get; set; }
            public float[] Data { get; set; } = new float[8];
        }
    }
}
