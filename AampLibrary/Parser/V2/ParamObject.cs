using Nintendo.Aamp.IO;
using System.IO;

namespace Nintendo.Aamp.Parser
{
    internal class ParamObjectV2 
    {
        internal static ParamObject Read(FileReader reader)
        {
            ParamObject entry = new();
            long CurrentPosition = reader.Position;

            entry.Hash = reader.ReadUInt32();
            ushort ChildOffset = reader.ReadUInt16();
            ushort ChildCount = reader.ReadUInt16();

            if (ChildOffset != 0)
            {
                using (reader.TemporarySeek(ChildOffset * 4 + CurrentPosition, SeekOrigin.Begin))
                {
                    entry.paramEntries = new ParamEntry[ChildCount];
                    for (int i = 0; i < ChildCount; i++)
                    {
                        entry.paramEntries[i] = ParamEntryV2.Read(reader);
                        entry.paramMap.Add(entry.paramEntries[i].Hash, i);
                    }
                }
            }
            return entry;
        }
    }
}
