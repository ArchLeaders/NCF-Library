using Nintendo.Aamp.IO;
using System.IO;

namespace Nintendo.Aamp.Parser
{
    internal class ParamListV2 
    {
        internal static ParamList Read(FileReader reader)
        {
            ParamList entry = new();
            long CurrentPosition = reader.Position;

            entry.Hash = reader.ReadUInt32();
            ushort ChildListOffset = reader.ReadUInt16();
            ushort ChildListCount = reader.ReadUInt16();
            ushort ParamObjectOffset = reader.ReadUInt16();
            ushort ParamObjectCount = reader.ReadUInt16();

            entry.ChildParams = new ParamList[ChildListCount];
            entry.ParamObjects = new ParamObject[ParamObjectCount];

            if (ChildListOffset != 0)
            {
                using (reader.TemporarySeek(ChildListOffset * 4 + CurrentPosition, SeekOrigin.Begin))
                {
                    for (int i = 0; i < ChildListCount; i++)
                    {
                        entry.ChildParams[i] = ParamListV2.Read(reader);
                        entry.listMap.Add(entry.ChildParams[i].Hash, i);
                    }
                }
            }
            if (ParamObjectOffset != 0)
            {
                using (reader.TemporarySeek(ParamObjectOffset * 4 + CurrentPosition, SeekOrigin.Begin))
                {
                    for (int i = 0; i < ParamObjectCount; i++)
                    {
                        entry.ParamObjects[i] = ParamObjectV2.Read(reader);
                        entry.objectMap.Add(entry.ParamObjects[i].Hash, i);
                    }
                }
            }

            return entry;
        }
    }
}
