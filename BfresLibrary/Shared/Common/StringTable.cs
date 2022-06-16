using System.Collections.Generic;
using Nintendo.Bfres.Core;
using Syroot.BinaryData;

namespace Nintendo.Bfres
{
    public class StringTable : IResData
    {
        public List<string> Strings = new List<string>();

        void IResData.Load(ResFileLoader loader)
        {
            Strings.Clear();
            if (loader.IsSwitch)
            {
                loader.Seek(-0x14, System.IO.SeekOrigin.Current);
                uint signature = loader.ReadUInt32();
                uint blockOffset = loader.ReadUInt32();
                long blockSize = loader.ReadInt64();
                uint stringCount = loader.ReadUInt32();

                for (int i = 0; i < stringCount + 1; i++)
                {
                    ushort size = loader.ReadUInt16();
                    Strings.Add(loader.ReadString(StringCoding.ZeroTerminated));
                    loader.Align(2);
                }
            }
        }

        void IResData.Save(ResFileSaver saver) { }
    }
}
