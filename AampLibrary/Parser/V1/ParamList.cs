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

            entry.ChildParams = new ParamList[ChildListCount];
            entry.ParamObjects = new ParamObject[ParamObjectCount];

            for (int i = 0; i < ChildListCount; i++)
            {
                entry.ChildParams[i] = ParamListV1.Read(reader);
                entry.listMap.Add(entry.ChildParams[i].Hash, i);
            }

            for (int i = 0; i < ParamObjectCount; i++)
            {
                entry.ParamObjects[i] = ParamObjectV1.Read(reader);
                entry.objectMap.Add(entry.ParamObjects[i].Hash, i);
            }

            reader.Seek(CurrentPosition + Size, SeekOrigin.Begin);
            return entry;
        }

        internal static void Write(ParamList entry, FileWriter writer)
        {
            int ChildListCount = entry.ChildParams == null ? 0 : entry.ChildParams.Length;
            int ParamObjectCount = entry.ParamObjects == null ? 0 : entry.ParamObjects.Length;

            long startPosition = writer.Position;
            writer.Write(uint.MaxValue); //Write the size after
            writer.Write(entry.Hash);
            writer.Write(ChildListCount);
            writer.Write(ParamObjectCount);

            for (int i = 0; i < ChildListCount; i++)
                if (entry.ChildParams != null)
                    Write(entry.ChildParams[i], writer);
            
            for (int i = 0; i < ParamObjectCount; i++)
                if (entry.ParamObjects != null)
                    ParamObjectV1.Write(entry.ParamObjects[i], writer);
            
            writer.WriteSize(writer.Position, startPosition);
        }
    }
}
