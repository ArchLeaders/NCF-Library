#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Nintendo.Byml.Collections;
using Syroot.BinaryData;
using Syroot.BinaryData.Core;
using System.Collections;
using System.Text;

namespace Nintendo.Byml.IO
{
    internal class BymlWriter : BymlFile
    {
        public BymlWriter(BymlFile byml) => Setter(byml);

        List<string> NameArray { get; set; } = new();
        List<string> StringArray { get; set; } = new();
        List<List<BymlPathPoint>> PathArray { get; set; } = new();
        Dictionary<dynamic, uint> WrittenNodes { get; set; } = new();
        List<dynamic> ReadNodes { get; set; } = new();

        public void Write(Stream stream, Encoding encoding)
        {
            // Check if the root is of the correct type.
            if (RootNode == null)
                throw new BymlException("Root node must not be null.");
            else if (!(RootNode is IDictionary<string, dynamic> || RootNode is IEnumerable))
                throw new BymlException($"Type '{RootNode.GetType()}' is not supported as a BYAML root node.");

            // Generate the name, string and path array nodes.
            CollectNodeArrayContents(RootNode);
            NameArray.Sort(StringComparer.Ordinal);
            StringArray.Sort(StringComparer.Ordinal);

            // Open a writer on the given stream.
            using (BinaryStream writer = new(stream, encoding: encoding, leaveOpen: true))
            {
                writer.ByteConverter = ByteConverter.GetConverter(Endianness);

                // Write the header, specifying magic bytes, version and main node offsets.
                // writer.Write(Endianness == Endian.Big ? 0x4259 : 0x5942);
                // Header writing is inversed?               B Y      Y B
                writer.Write(Endianness == Endian.Little ? (ushort)0x4259 : (ushort)0x5942);
                writer.Write(Version);
                uint nameArrayOffset = writer.ReserveOffset();
                uint stringArrayOffset = writer.ReserveOffset();
                uint? pathArrayOffset = SupportPaths ? writer.ReserveOffset() : null;
                uint rootOffset = writer.ReserveOffset();

                // Write the main nodes.
                WriteEnumerableNode(writer, nameArrayOffset, NodeType.StringArray, NameArray);
                if (StringArray.Count == 0)
                    writer.Write(0);
                else
                    WriteEnumerableNode(writer, stringArrayOffset, NodeType.StringArray, StringArray);

                // Include a path array offset if requested.
                if (SupportPaths)
                {
                    if (PathArray.Count == 0)
                        writer.Write(0);
                    else
                        WriteEnumerableNode(writer, pathArrayOffset ?? 0, NodeType.PathArray, PathArray);
                }

                // Write the root node.
                WriteEnumerableNode(writer, rootOffset, NodeTypeExtension.GetNodeType(RootNode), (IEnumerable)RootNode);
            }
        }

        private void CollectNodeArrayContents(dynamic node)
        {
            if (node == null) return;

            if (ReadNodes.Contains(node))
                return;

            ReadNodes.Add(node);
            switch (node)
            {
                case string stringNode:
                    if (!StringArray.Contains(stringNode))
                        StringArray.Add(stringNode);
                    break;
                case List<BymlPathPoint> pathNode:
                    PathArray.Add(pathNode);
                    break;
                case IDictionary<string, object> dictionaryNode:
                    foreach (KeyValuePair<string, object> entry in dictionaryNode)
                    {
                        if (!NameArray.Contains(entry.Key))
                            NameArray.Add(entry.Key);
                        CollectNodeArrayContents(entry.Value);
                    }
                    break;
                case IEnumerable arrayNode:
                    foreach (object childNode in arrayNode)
                        CollectNodeArrayContents(childNode);
                    break;
            }
        }

        private uint WriteValue(BinaryStream writer, dynamic value)
        {
            // Only reserve and return an offset for the complex value contents, write simple values directly.
            NodeType type = NodeTypeExtension.GetNodeType(value);

            switch (type)
            {
                case NodeType.StringIndex:
                    WriteStringIndexNode(writer, value);
                    return 0;
                case NodeType.PathIndex:
                    if (value is BymlPathIndex index)
                        writer.Write(index.Index);
                    else
                        WritePathIndexNode(writer, value);
                    return 0;
                case NodeType.Dictionary or NodeType.Array:
                    return writer.ReserveOffset();
                case NodeType.Boolean:
                    writer.Write(value ? 1 : 0);
                    return 0;
                case NodeType.Integer or NodeType.Float or NodeType.Uinteger or NodeType.Double or NodeType.ULong or NodeType.Long:
                    writer.Write(value);
                    return 0;
                case NodeType.Null:
                    writer.Write(0x0);
                    return 0;
                default:
                    throw new BymlException($"{type} not supported as value node.");
            }
        }

        private void WriteEnumerableNode(BinaryStream writer, uint offset, NodeType type, IEnumerable node)
        {
            if (WrittenNodes.TryGetValue(node, out uint position))
            {
                writer.SatisfyOffset(offset, position);
                return;
            }
            else
            {
                // Satisfy the offset to the complex node value which must be 4-byte aligned.
                position = (uint)writer.Position;
                writer.SatisfyOffset(offset, position);
                WriteTypeAndLength(writer, type, node);

                WrittenNodes.Add(node, position);

                // Write the value contents.
                switch (type)
                {
                    case NodeType.Array:
                        WriteArrayNode(writer, node);
                        break;
                    case NodeType.Dictionary:
                        WriteDictionaryNode(writer, (IDictionary<string, object>)node);
                        break;
                    case NodeType.StringArray:
                        WriteStringArrayNode(writer, (List<string>)node);
                        break;
                    case NodeType.PathArray:
                        WritePathArrayNode(writer, (List<List<BymlPathPoint>>)node);
                        break;
                    default:
                        throw new BymlException($"{type} not supported as complex node.");
                }
            }
        }

        private void WriteTypeAndLength(BinaryStream writer, NodeType type, dynamic node) =>
            writer.Write(Endianness == Endian.Big ? (uint)type << 24 | (uint)Enumerable.Count(node) : (uint)type | (uint)Enumerable.Count(node) << 8);
        private void WriteStringIndexNode(BinaryStream writer, string node) =>writer.Write((uint)StringArray.IndexOf(node));
        private void WritePathIndexNode(BinaryStream writer, List<BymlPathPoint> node) => writer.Write(PathArray.IndexOf(node));

        private void WriteArrayNode(BinaryStream writer, IEnumerable node)
        {
            // Write the element types.
            foreach (dynamic element in node)
                writer.Write((byte)NodeTypeExtension.GetNodeType(element));

            // Write the elements, which begin after a padding to the next 4 bytes.
            writer.Align(4);

            List<IEnumerable> enumerables = new();
            List<uint> offsets = new();
            foreach (dynamic element in node)
            {
                var off = WriteValue(writer, element);
                if (off > 0)
                {
                    offsets.Add(off);
                    enumerables.Add((IEnumerable)element);
                }
            }

            // Write the contents of complex nodes and satisfy the offsets.
            for (int i = 0; i < enumerables.Count; i++)
            {
                IEnumerable enumerable = enumerables[i];
                WriteEnumerableNode(writer, offsets[i], NodeTypeExtension.GetNodeType(enumerable), enumerable);
            }
        }

        private void WriteDictionaryNode(BinaryStream writer, IDictionary<string, dynamic> node)
        {
            List<EnumerableNode> enumerables = node
               .Where(x => (NodeTypeExtension.GetNodeType(x.Value) >= NodeType.Array && NodeTypeExtension.GetNodeType(x.Value) <= NodeType.PathArray))
               .Select(x => new EnumerableNode((IEnumerable)x.Value))
               .ToList();

            // Write the key-value pairs.
            foreach (KeyValuePair<string, object> element in node.OrderBy(x => x.Key, StringComparer.Ordinal))
            {
                // Get the index of the key string in the file's name array and write it together with the type.
                uint keyIndex = (uint)NameArray.IndexOf(element.Key);
                if (Endianness == Endian.Big)
                    writer.Write(keyIndex << 8 | (uint)NodeTypeExtension.GetNodeType(element.Value));
                else
                    writer.Write(keyIndex | (uint)NodeTypeExtension.GetNodeType(element.Value) << 24);

                // Write the elements.
                var offset = WriteValue(writer, element.Value);
                if (offset > 0)
                    enumerables.Where(x => x.Node == element.Value && x.Offset == 0).First().Offset = offset;
            }

            // Write the value contents.
            foreach (EnumerableNode enumerable in enumerables)
                WriteEnumerableNode(writer, enumerable.Offset, NodeTypeExtension.GetNodeType(enumerable.Node), enumerable.Node);
        }

        private static void WriteStringArrayNode(BinaryStream writer, List<string> node)
        {
            // Write the offsets to the strings, where the last one points to the end of the last string.
            long offset = sizeof(uint) + sizeof(uint) * (node.Count + 1); // Relative to node start + all uint32 offsets.
            foreach (string str in node)
            {
                writer.Write((uint)offset);
                offset += writer.Encoding.GetByteCount(str) + 1;
            }
            writer.Write((uint)offset);

            foreach (string str in node)
                writer.Write(str, StringCoding.ZeroTerminated);
            writer.Align(4);
        }

        private static void WritePathArrayNode(BinaryStream writer, IEnumerable<List<BymlPathPoint>> node)
        {
            // Write the offsets to the paths, where the last one points to the end of the last path.
            long offset = 4 + 4 * (node.Count() + 1); // Relative to node start + all uint32 offsets.
            foreach (List<BymlPathPoint> path in node)
            {
                writer.Write((uint)offset);
                offset += path.Count * 28; // 28 bytes are required for a single point.
            }
            writer.Write((uint)offset);

            // Write the paths.
            foreach (List<BymlPathPoint> path in node)
                WritePathNode(writer, path);
        }

        private static void WritePathNode(BinaryStream writer, List<BymlPathPoint> node)
        {
            foreach (BymlPathPoint point in node)
                WritePathPoint(writer, point);
        }

        private static void WritePathPoint(BinaryStream writer, BymlPathPoint point)
        {
            writer.Write(point.Position.X);
            writer.Write(point.Position.Y);
            writer.Write(point.Position.Z);
            writer.Write(point.Normal.X);
            writer.Write(point.Normal.Y);
            writer.Write(point.Normal.Z);
            writer.Write(point.Unknown);
        }
    }
}
