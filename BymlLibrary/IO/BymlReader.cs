using Nintendo.Byml.Collections;
using Syroot.BinaryData;
using Syroot.BinaryData.Core;
using Syroot.Maths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nintendo.Byml.IO
{
    internal class BymlReader : BymlFile
    {
        private List<string> NameArray { get; set; } = new();
        List<string> StringArray { get; set; } = new();
        private Dictionary<uint, dynamic> ReadNodes { get; set; } = new();
        List<List<BymlPathPoint>> PathArray { get; set; } = new();

        public BymlFile Read(Stream stream, Encoding encoding)
        {
            // Open a reader on the given stream.
            using (BinaryStream reader = new(stream, encoding: encoding, leaveOpen: true))
            {
                ushort magic = reader.ReadUInt16();

                if (magic == 0x4259)
                    Endianness = Endian.Big;
                else if (magic == 0x5942)
                    Endianness = Endian.Little;
                else
                    throw new BymlException($"Could not decompile BYML. Invalid header '{magic}'.");

                if (reader.ReadUInt16() != 0x4259)
                {
                    Endianness = Endianness == Endian.Little ? Endian.Big : Endian.Little;
                    reader.ByteConverter = ByteConverter.GetConverter(Endianness);
                    reader.BaseStream.Position = 0;
                    if (reader.ReadUInt16() != 0x4259) throw new Exception("Header mismatch");
                }

                // reader.BaseStream.Position = 0;
                // reader.ByteConverter = ByteConverter.GetConverter(Endianness);

                // Get BYML version
                Version = reader.ReadUInt16();

                // Read the name array, holding strings referenced by index for the names of other nodes.
                NameArray = ReadEnumerableNode(reader, reader.ReadUInt32());
                StringArray = ReadEnumerableNode(reader, reader.ReadUInt32());

                if (reader.BaseStream.Length <= 16)
                    RootNode = new List<dynamic>();

                using (reader.TemporarySeek())
                {
                    //Thanks to Syroot https://gitlab.com/Syroot/NintenTools.Byaml/blob/master/src/Syroot.NintenTools.Byaml/DynamicLoader.cs
                    // Paths are supported if the third offset is a path array (or null) and the fourth a root.
                    NodeType thirdNodeType = reader.PeekNodeType();
                    reader.Seek(sizeof(uint));
                    NodeType fourthNodeType = reader.PeekNodeType();

                    SupportPaths = (thirdNodeType == NodeType.None || thirdNodeType == NodeType.PathArray)
                         && (fourthNodeType == NodeType.Array || fourthNodeType == NodeType.Dictionary);

                }

                if (SupportPaths)
                    PathArray = ReadEnumerableNode(reader, reader.ReadUInt32());

                uint rootNodeOffset = reader.ReadUInt32();

                if (rootNodeOffset == 0)
                    RootNode = new List<dynamic>();
                else
                    RootNode = ReadEnumerableNode(reader, rootNodeOffset);
            }

            stream.Dispose();
            return this;
        }

        private uint Get1MsbByte(uint value) => Endianness == Endian.Big ? value & 0x000000FF : value >> 24;
        private uint Get3LsbBytes(uint value) => Endianness == Endian.Big ? value & 0x00FFFFFF : value >> 8;
        private uint Get3MsbBytes(uint value) => Endianness == Endian.Little ? value & 0x00FFFFFF : value >> 8;

        private dynamic ReadEnumerableNode(BinaryStream reader, uint offset)
        {
            if (ReadNodes.ContainsKey(offset))
                return ReadNodes[offset];

            object node = null;
            if (offset > 0 && !ReadNodes.TryGetValue(offset, out node))
            {
                using (reader.TemporarySeek(offset, SeekOrigin.Begin))
                {
                    NodeType type = (NodeType)reader.ReadByte();
                    reader.Seek(-1);
                    int length = (int)Get3LsbBytes(reader.ReadUInt32());
                    node = type switch
                    {
                        NodeType.Array => ReadArrayNode(reader, length, offset),
                        NodeType.Dictionary => ReadDictionaryNode(reader, length, offset),
                        NodeType.StringArray => ReadStringArrayNode(reader, length),
                        NodeType.PathArray => ReadPathArrayNode(reader, length),
                        _ => throw new BymlException($"Invalid enumerable node type {type}."),
                    };
                }
            }

            return node;
        }

        public List<dynamic> ReadArrayNode(BinaryStream reader, int length, uint offset = 0)
        {
            List<dynamic> array = new(length);
            if (offset != 0) ReadNodes.Add(offset, array);

            IEnumerable<NodeType> types = reader.ReadBytes(length).Select(x => (NodeType)x);
            reader.Align(4);

            foreach (NodeType type in types)
            {
                dynamic value = ReadNode(reader, type);
                if (type.IsEnumerable())
                    array.Add(ReadEnumerableNode(reader, value));
                else
                    array.Add(value);
            }

            return array;
        }

        private Dictionary<string, dynamic> ReadDictionaryNode(BinaryStream reader, int length, uint offset = 0)
        {
            Dictionary<string, dynamic> dictionary = new(length);
            SortedList<uint, string> enumerables = new(new DuplicateKeyComparer<uint>());
            if (offset != 0) ReadNodes.Add(offset, dictionary);

            // Read the elements of the dictionary.
            for (int i = 0; i < length; i++)
            {
                uint indexAndType = reader.ReadUInt32();
                int nodeNameIndex = (int)Get3MsbBytes(indexAndType);
                NodeType type = (NodeType)Get1MsbByte(indexAndType);

                string name = NameArray[nodeNameIndex];
                dynamic value = ReadNode(reader, type);

                if (type.IsEnumerable())
                    dictionary.Add(name, ReadEnumerableNode(reader, value));
                else
                    dictionary.Add(name, value);
            }

            // Read the offset enumerable nodes in the order of how they appear in the file.
            foreach (KeyValuePair<uint, string> enumerable in enumerables)
                dictionary.Add(enumerable.Value, ReadEnumerableNode(reader, enumerable.Key));

            return dictionary;
        }

        private static List<string> ReadStringArrayNode(BinaryStream reader, int length)
        {
            // Element offsets are relative to the start of node.
            long nodeOffset = reader.Position - sizeof(uint);
            List<string> stringArray = new(length);

            // Read the element offsets.
            uint[] offsets = reader.ReadUInt32s(length);

            // Read the elements.
            foreach (uint offset in offsets)
            {
                reader.Position = nodeOffset + offset;
                stringArray.Add(reader.ReadString(StringCoding.ZeroTerminated));
            }

            return stringArray;
        }

        private static List<List<BymlPathPoint>> ReadPathArrayNode(BinaryStream reader, int length)
        {
            long nodeOffset = reader.Position - sizeof(uint); // Element offsets are relative to the start of node.
            List<List<BymlPathPoint>> pathArray = new(length);

            // Read the element offsets.
            uint[] offsets = reader.ReadUInt32s(length + 1);

            // Read the elements.
            for (int i = 0; i < length; i++)
            {
                reader.Position = nodeOffset + offsets[i];
                int pointCount = (int)((offsets[i + 1] - offsets[i]) / BymlPathPoint.SizeInBytes);
                pathArray.Add(ReadPath(reader, pointCount));
            }

            return pathArray;
        }

        private dynamic ReadNode(BinaryStream reader, NodeType nodeType)
        {
            // Read the following UInt32 which is representing the value directly.
            return nodeType switch
            {
                NodeType.Array or NodeType.Dictionary or NodeType.StringArray or NodeType.PathArray => reader.ReadUInt32(),// offset
                NodeType.StringIndex => StringArray[reader.ReadInt32()],
                NodeType.PathIndex => HandlePathIndexInline(),
                NodeType.Boolean => reader.ReadInt32() != 0,
                NodeType.Integer => reader.ReadInt32(),
                NodeType.Float => reader.ReadSingle(),
                NodeType.Uinteger => reader.ReadUInt32(),
                NodeType.Long or NodeType.ULong or NodeType.Double => ReadNodeFromOffset(reader, nodeType),
                NodeType.Null => HandleNullInline(),
                _ => throw new BymlException($"Unknown node type '{nodeType:X}'."),
            };

            dynamic HandlePathIndexInline()
            {
                int index = reader.ReadInt32();
                return PathArray != null && PathArray.Count > index ? PathArray[index] : new BymlPathIndex();
            }

            dynamic HandleNullInline()
            {
                reader.Seek(0x4);
                return null;
            }
        }

        private static dynamic ReadNodeFromOffset(BinaryStream reader, NodeType nodeType)
        {
            // Set position
            var pos = reader.Position;
            reader.Position = reader.ReadUInt32();

            // Get node value
            dynamic value = nodeType switch
            {
                NodeType.Long => reader.ReadInt64(),
                NodeType.ULong => reader.ReadUInt64(),
                NodeType.Double => reader.ReadDouble(),
                _ => throw new BymlException($"Unknown node type '{nodeType}'."),
            };

            // Reset position
            reader.Position = pos + 4;

            return value;
        }

        private static List<BymlPathPoint> ReadPath(BinaryStream reader, int length)
        {
            List<BymlPathPoint> byamlPath = new();
            for (int j = 0; j < length; j++)
                byamlPath.Add(ReadPathPoint(reader));

            return byamlPath;
        }

        private static BymlPathPoint ReadPathPoint(BinaryStream reader)
        {
            BymlPathPoint point = new();
            point.Position = new Vector3F(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            point.Normal = new Vector3F(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            point.Unknown = reader.ReadUInt32();
            return point;
        }
    }
}
