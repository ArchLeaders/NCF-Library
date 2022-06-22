using Nintendo.Aamp.IO;
using System.IO;

namespace Nintendo.Aamp.Parser
{
    internal class ParamListV1 
    {
        internal static ParamList Read(FileReader reader)
        {
            ParamList entry = new();

            long CurrentPosition = reader.Position;

            uint Size = reader.ReadUInt32();
            entry.Hash = reader.ReadUInt32();
            uint ChildListCount = reader.ReadUInt32();
            uint ParamObjectCount = reader.ReadUInt32();

            entry.childLists = new ParamList[ChildListCount];
            entry.childObjects = new ParamObject[ParamObjectCount];

            for (int i = 0; i < ChildListCount; i++)
            {
                entry.childLists[i] = ParamListV1.Read(reader);
                entry.listMap.Add(entry.childLists[i].Hash, i);
            }

            for (int i = 0; i < ParamObjectCount; i++)
            {
                entry.childObjects[i] = ParamObjectV1.Read(reader);
                entry.objectMap.Add(entry.childObjects[i].Hash, i);
            }

            reader.Seek(CurrentPosition + Size, SeekOrigin.Begin);
            return entry;
        }

        internal static void Write(ParamList entry, FileWriter writer)
        {
            int ChildListCount = entry.childLists == null ? 0 : entry.childLists.Length;
            int ParamObjectCount = entry.childObjects == null ? 0 : entry.childObjects.Length;

            long startPosition = writer.Position;
            writer.Write(uint.MaxValue); //Write the size after
            writer.Write(entry.Hash);
            writer.Write(ChildListCount);
            writer.Write(ParamObjectCount);

            for (int i = 0; i < ChildListCount; i++)
                if (entry.childLists != null)
                    Write(entry.childLists[i], writer);
            
            for (int i = 0; i < ParamObjectCount; i++)
                if (entry.childObjects != null)
                    ParamObjectV1.Write(entry.childObjects[i], writer);
            
            writer.WriteSize(writer.Position, startPosition);
        }
    }
}
