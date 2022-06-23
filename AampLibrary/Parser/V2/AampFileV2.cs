using Nintendo.Aamp.IO;
using System.Text;
using Syroot.BinaryData;
using Syroot.Maths;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Nintendo.Aamp.Parser
{
    internal class AampFileV2 : AampFile
    {
        public AampFileV2(string fileName)
        {
            using FileStream stream = File.OpenRead(fileName);
            using FileReader reader = new(stream);
            DecompileAamp(reader);
        }
        public AampFileV2(Stream stream)
        {
            using FileReader reader = new(stream);
            DecompileAamp(reader);
        }
        public AampFileV2() : base()
        {
            Version = 2;
        }

        private uint TotalListCount { get; set; } = 0;
        private uint TotalObjCount { get; set; } = 0;
        private uint TotalParamCount { get; set; } = 0;

        internal void DecompileAamp(FileReader reader)
        {
            reader.ByteConverter = ByteConverter.Little;

            reader.CheckSignature("AAMP");
            Version = reader.ReadUInt32();
            Endianness = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            ParameterIOVersion = reader.ReadUInt32();
            uint ParameterIOOffset = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            _ = reader.ReadUInt32();
            long pos = reader.Position;
            ParameterIOType = reader.ReadString(StringCoding.ZeroTerminated, Encoding.UTF8);
            reader.Seek(pos + ParameterIOOffset, SeekOrigin.Begin);

            RootNode = ParamListV2.Read(reader);
        }

        internal byte[] CompileV2()
        {
            byte[] bytes;
            using (MemoryStream ms = new())
            {
                using (FileWriter fw = new(ms))
                {
                    CompileV2(fw);
                    bytes = ms.ToArray();
                }
            }
            return bytes;
        }

        internal void CompileV2(FileWriter writer)
        {
            SavedParamObjects.Clear();
            SavedParamLists.Clear();
            DataValues.Clear();
            StringValues.Clear();
            ObjListOffsets.Clear();
            ListOffsets.Clear();

            writer.ByteConverter = ByteConverter.Little;

            writer.Write(Encoding.ASCII.GetBytes("AAMP"));
            writer.Write(Version);
            writer.Write(Endianness);

            long sizeOffset = writer.Position;
            writer.Write(0); //Write the file size later
            writer.Write(ParameterIOVersion);

            uint DataSectionSize = 0;
            uint StringSectionSize = 0;
            uint UnkUintCount = 0;

            writer.Write(AlignUp(ParameterIOType.Length + 1, 4));
            long totalCountOffset = writer.Position;
            writer.Write(TotalListCount);
            writer.Write(TotalObjCount);
            writer.Write(TotalParamCount);
            writer.Write(DataSectionSize);
            writer.Write(StringSectionSize);
            writer.Write(UnkUintCount);
            writer.Write(ParameterIOType, StringCoding.ZeroTerminated);
            writer.Align(4);

            //Write the lists
            WriteList(writer, RootNode);

            //Save the data and offsets for lists
            for (int index = 0; index < SavedParamLists.Count; index++)
                WriteListData(writer, SavedParamLists[index], ListOffsets[index]);

            //Save objects from lists
            for (int index = 0; index < SavedParamLists.Count; index++)
            {
                ListOffsets[index][1].WriteOffsetU16(writer, (uint)writer.Position);
                foreach (var obj in SavedParamLists[index].ParamObjects)
                    WriteObject(writer, obj);
            }

            while (SavedParamObjects.Count != 0)
                WriteObjectData(writer, PopAt(SavedParamObjects, 0));

            uint DataStart = (uint)writer.Position;
            foreach (var entry in DataValues)
            {
                foreach (var offset in entry.Value)
                {
                    if (IsBuffer(((ParamEntry)offset.Data).ParamType))
                    {
                        writer.Write(0); //Save offset after the size of buffer
                        offset.WriteOffsetU24(writer, (uint)writer.Position, (ParamEntry)offset.Data!);
                        writer.Seek(-4, SeekOrigin.Current);
                    }
                    else
                        offset.WriteOffsetU24(writer, (uint)writer.Position, (ParamEntry)offset.Data!);
                }

                writer.Write(entry.Key);
                writer.Align(4);
            }

            uint DataEnd = (uint)writer.Position;
            uint StringDataStart = (uint)writer.Position;

            int stringCount = 0;
            foreach (var entry in StringValues)
            {
                foreach (var offset in entry.Value)
                {
                    offset.WriteOffsetU24(writer, (uint)writer.Position, (ParamEntry)offset.Data!);
                    stringCount++;
                }

                writer.Write(entry.Key);

                do
                    writer.Write((byte)0);
                while (writer.Position % 4 != 0);
            }
            uint StringDataEnd = (uint)writer.Position;

            //Write data size
            writer.Seek(totalCountOffset, SeekOrigin.Begin);

            writer.Write(TotalListCount);
            writer.Write(TotalObjCount);
            writer.Write(TotalParamCount);
            writer.Write(DataEnd - DataStart);
            writer.Write(StringDataEnd - StringDataStart);

            //Write end of file
            writer.Seek(sizeOffset, SeekOrigin.Begin);
            uint FileSize = (uint)writer.BaseStream.Length;
            writer.Write(FileSize);
        }

        private List<ParamList> SavedParamLists { get; set; } = new();
        private List<ObjectContext> SavedParamObjects { get; set; } = new();
        private List<ParamEntry> SavedParamEntries { get; set; } = new();
        private List<PlaceholderOffset[]> ListOffsets { get; set; } = new();
        private List<PlaceholderOffset> ObjListOffsets { get; set; } = new();

        private Dictionary<byte[], List<PlaceholderOffset>> DataValues { get; set; } = new(new ByteArrayComparer());
        private Dictionary<byte[], List<PlaceholderOffset>> StringValues { get; set; } = new(new ByteArrayComparer());

        private class ObjectContext
        {
            public PlaceholderOffset? PlaceholderOffset;
            public ParamObject? ParamObject;
        }

        public class PlaceholderOffset
        {
            public object Data { get; set; } = new();
            public long BasePosition { get; set; }
            public long OffsetPosition { get; set; }

            public static void WritePlaceholderU16(FileWriter writer) => writer.Write(ushort.MaxValue);
            public static void WritePlaceholderU24(FileWriter writer)
            {
                writer.Write(byte.MaxValue);
                writer.Write(byte.MaxValue);
                writer.Write(byte.MaxValue);
            }
            public static void WritePlaceholderU32(FileWriter writer) => writer.Write(uint.MaxValue);
            public static void WritePlaceholderU64(FileWriter writer) => writer.Write(ulong.MaxValue);

            public void WriteOffsetU16(FileWriter writer, uint Offset)
            {
                using (writer.TemporarySeek(OffsetPosition, SeekOrigin.Begin))
                {
                    writer.Write((ushort)((Offset - BasePosition) >> 2));
                }
            }

            public void WriteOffsetU24(FileWriter writer, uint Offset, ParamEntry entry)
            {
                using (writer.TemporarySeek(OffsetPosition, SeekOrigin.Begin))
                {
                    uint ValuePacked = 0;
                    writer.Write((uint)((ValuePacked << 24) | ((Offset - BasePosition) >> 2)));

                    writer.Seek(OffsetPosition + 3, SeekOrigin.Begin);
                    writer.Write((byte)entry.ParamType);
                }
            }
        }

        private static int AlignUp(int n, int align) => (n + align - 1) & -align;

        private static bool IsBuffer(ParamType type)
        {
            return type switch
            {
                ParamType.BufferUint or ParamType.BufferInt or ParamType.BufferFloat or ParamType.BufferBinary => true,
                _ => false,
            };
        }

        private static T PopAt<T>(List<T> list, int index)
        {
            T r = list[index];
            list.RemoveAt(index);
            return r;
        }

        private static PlaceholderOffset WritePlaceholderOffsetU24(FileWriter writer, long BasePosition)
        {
            PlaceholderOffset offset = new();
            offset.OffsetPosition = writer.Position;
            offset.BasePosition = BasePosition;
            PlaceholderOffset.WritePlaceholderU24(writer);
            return offset;
        }

        private static PlaceholderOffset WritePlaceholderOffsetU16(FileWriter writer, long BasePosition)
        {
            PlaceholderOffset offset = new();
            offset.OffsetPosition = writer.Position;
            offset.BasePosition = BasePosition;
            PlaceholderOffset.WritePlaceholderU16(writer);
            return offset;
        }

        private void WriteList(FileWriter writer, ParamList paramList)
        {
            TotalListCount += 1;
            SavedParamLists.Add(paramList);

            ushort ChildListCount = (ushort)paramList.ChildParams.Length;
            ushort ParamObjectCount = (ushort)paramList.ParamObjects.Length;

            long pos = writer.Position;
            writer.Write(paramList.Hash);
            var listEntry = WritePlaceholderOffsetU16(writer, pos);
            writer.Write(ChildListCount);
            var objectEntry = WritePlaceholderOffsetU16(writer, pos);
            writer.Write(ParamObjectCount);

            ListOffsets.Add(new PlaceholderOffset[] { listEntry, objectEntry });
        }

        private void WriteListData(FileWriter writer, ParamList paramList, PlaceholderOffset[] offsets)
        {
            offsets[0].WriteOffsetU16(writer, (uint)writer.Position);
            foreach (var child in paramList.ChildParams)
                WriteList(writer, child);
        }

        private void WriteObject(FileWriter writer, ParamObject paramObj)
        {
            TotalObjCount += 1;
            int EntryCount = paramObj.ParamEntries?.Length ?? 0;

            long startPosition = writer.Position;

            writer.Write(paramObj.Hash);
            var paramEntry = WritePlaceholderOffsetU16(writer, startPosition);
            writer.Write((ushort)EntryCount);

            SavedParamObjects.Add(new ObjectContext()
            {
                ParamObject = paramObj,
                PlaceholderOffset = paramEntry,
            });
        }

        public class ByteArrayComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[]? left, byte[]? right)
            {
                if (left == null || right == null)
                    return left == right;

                return left.SequenceEqual(right);
            }

            public int GetHashCode(byte[] key)
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                return key.Sum(b => b);
            }
        }

        private void WriteObjectData(FileWriter writer, ObjectContext context)
        {
            if (context.ParamObject == null || context.PlaceholderOffset == null)
                return;

            context.PlaceholderOffset.WriteOffsetU16(writer, (uint)writer.Position);

            foreach (ParamEntry entry in context.ParamObject.ParamEntries)
            {
                SavedParamEntries.Add(entry);

                long startOffset = writer.Position;
                TotalParamCount += 1;

                writer.Write(entry.Hash);
                var paramData = WritePlaceholderOffsetU24(writer, startOffset);
                paramData.Data = entry;
                writer.Write((byte)entry.ParamType);

                byte[] data = GetParamData(entry);

                if (IsString(entry.ParamType))
                {
                    //Only write string entires once if any are the same
                    //DataValues store byte arrays for the data as the key then a list of offsets pointing to it

                    if (!StringValues.ContainsKey(data))
                        StringValues.Add(data, new List<PlaceholderOffset>() { paramData, });
                    else
                        StringValues[data].Add(paramData);
                }
                else
                {
                    //Only write data entires once if any are the same
                    //DataValues store byte arrays for the data as the key then a list of offsets pointing to it
                    if (DataValues.ContainsKey(data))
                        DataValues[data].Add(paramData); //Add additional offsets
                    else
                        DataValues.Add(data, new List<PlaceholderOffset> { paramData });
                }
            }
        }

        private static bool IsString(ParamType ParamType)
        {
            return ParamType switch
            {
                ParamType.String64 or ParamType.String32 or ParamType.String256 or ParamType.StringRef => true,
                _ => false,
            };
        }

        private static byte[] GetParamData(ParamEntry entry)
        {
            MemoryStream mem = new();
            var writer = new FileWriter(mem) {
                ByteConverter = ByteConverter.Little
            };

            switch (entry.ParamType)
            {
                case ParamType.Boolean: writer.Write((bool)entry.Value == false ? (uint)0 : (uint)1); break;
                case ParamType.Float: writer.Write((float)entry.Value); break;
                case ParamType.Int: writer.Write((int)entry.Value); break;
                case ParamType.Vector2F: writer.WriteVector2F((Vector2F)entry.Value); break;
                case ParamType.Vector3F: writer.WriteVector3F((Vector3F)entry.Value); break;
                case ParamType.Vector4F: writer.WriteVector4F((Vector4F)entry.Value); break;
                case ParamType.Color4F: writer.WriteColor4F((Color4F)entry.Value); break;
                case ParamType.Quat: writer.Write((float[])entry.Value); break;
                case ParamType.Uint: writer.Write((uint)entry.Value); break;
                case ParamType.BufferUint:
                    writer.Write(((uint[])entry.Value).Length);
                    writer.Write((uint[])entry.Value); break;
                case ParamType.BufferInt:
                    writer.Write(((int[])entry.Value).Length);
                    writer.Write((int[])entry.Value); break;
                case ParamType.BufferFloat:
                    writer.Write(((float[])entry.Value).Length);
                    writer.Write((float[])entry.Value); break;
                case ParamType.BufferBinary:
                    writer.Write(((byte[])entry.Value).Length);
                    writer.Write((byte[])entry.Value); break;
                case ParamType.String64:
                case ParamType.String32:
                case ParamType.String256:
                case ParamType.StringRef:
                    writer.Write(((StringEntry)entry.Value).Data);
                    break;
                case ParamType.Curve1:
                case ParamType.Curve2:
                case ParamType.Curve3:
                case ParamType.Curve4:
                    int curveAmount = entry.ParamType - ParamType.Curve1 + 1;

                    var curves = (Curve[])entry.Value;
                    for (int i = 0; i < curveAmount; i++)
                    {
                        writer.Write(curves[i].ValueUints);
                        writer.Write(curves[i].ValueFloats);
                    }
                    break;
                default:
                    throw new Exception("Unsupported param type! " + entry.ParamType);
            }

            return mem.ToArray();
        }

        uint GetListCount(ParamList paramList, uint total = 0)
        {
            total += 1;
            foreach (var child in paramList.ChildParams)
                total += GetListCount(child, total);

            return total;
        }

        uint GetObjectCount(ParamList paramList, uint total = 0)
        {
            total += 1;
            foreach (var child in paramList.ChildParams)
                total += GetObjectCount(child, total);

            return total;
        }
    }
}
