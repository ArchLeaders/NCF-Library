#pragma warning disable IDE0150 // Prefer 'null' check over type check
#pragma warning disable CS8604 // Possible null reference argument.

using SharpYaml.Serialization;
using System.Globalization;
using Syroot.BinaryData;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Syroot.BinaryData.Core;

namespace Nintendo.Byml.Parser
{
    public class YamlConverter
    {
        private static Dictionary<dynamic, YamlNode> NodePaths { get; set; } = new Dictionary<dynamic, YamlNode>();
        private static Dictionary<string, dynamic> ReferenceNodes { get; set; } = new Dictionary<string, dynamic>();
        static int RefNodeId { get; set; } = 0;

        public static string ToYaml(BymlFile byml)
        {
            NodePaths.Clear();
            RefNodeId = 0;

            YamlNode root = SaveNode(byml.RootNode);
            YamlMappingNode mapping = new();

            mapping.Add("Version", byml.Version.ToString());
            mapping.Add("IsBigEndian", (byml.Endianness == Endian.Big).ToString());
            mapping.Add("SupportPaths", byml.SupportPaths.ToString());
            mapping.Add("HasReferenceNodes", (RefNodeId != 0).ToString());
            mapping.Add("root", root);

            NodePaths.Clear();
            RefNodeId = 0;

            YamlStream stream = new(new YamlDocument(mapping));
            using StringWriter? writer = new(new StringBuilder());
            stream.Save(writer, true);
            return writer.ToString();
        }

        public static BymlFile FromYaml(string text)
        {
            NodePaths.Clear();
            ReferenceNodes.Clear();

            var byml = new BymlFile();
            var yaml = new YamlStream();

            yaml.Load(new StringReader(text));

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            foreach (var child in mapping.Children)
            {
                var key = ((YamlScalarNode)child.Key).Value;
                var value = child.Value.ToString();

                if (key == "Version")
                    byml.Version = ushort.Parse(value);
                if (key == "IsBigEndian")
                    byml.Endianness = bool.Parse(value) ? Endian.Big : Endian.Little;
                if (key == "SupportPaths")
                    byml.SupportPaths = bool.Parse(value);
                if (child.Value is YamlMappingNode)
                    byml.RootNode = ParseNode(child.Value);
                if (child.Value is YamlSequenceNode)
                    byml.RootNode = ParseNode(child.Value);
            }

            ReferenceNodes.Clear();
            NodePaths.Clear();

            return byml;
        }

        static dynamic? ParseNode(YamlNode node)
        {
            if (node is YamlMappingNode castMappingNode)
            {
                var values = new Dictionary<string, dynamic>();
                if (IsValidReference(node))
                    ReferenceNodes.Add(node.Tag, values);

                foreach (var child in castMappingNode.Children)
                {
                    var key = ((YamlScalarNode)child.Key).Value;
                    var tag = ((YamlScalarNode)child.Key).Tag;
                    if (tag == "!h")
                        key = Crc32.Compute(key).ToString("x");

                    values.Add(key, ParseNode(child.Value));
                }
                return values;
            }
            else if (node is YamlSequenceNode castSequenceNode) {

                var values = new List<dynamic>();
                if (IsValidReference(node))
                    ReferenceNodes.Add(node.Tag, values);

                foreach (var child in castSequenceNode.Children)
                    values.Add(ParseNode(child));

                return values;
            }
            else if (node is YamlScalarNode castScalarNode && castScalarNode.Value.Contains("!refTag=")) {

                string tag = castScalarNode.Value.Replace("!refTag=", string.Empty);
                Debug.WriteLine($"refNode {tag} {ReferenceNodes.ContainsKey(tag)}");

                if (ReferenceNodes.ContainsKey(tag))
                    return ReferenceNodes[tag];
                else {
                    Console.WriteLine("Failed to find reference node! " + tag);
                    return null;
                }
            }
            else {
                return ConvertValue(((YamlScalarNode)node).Value, ((YamlScalarNode)node).Tag);
            }
        }

        static bool IsValidReference(YamlNode node) => node.Tag != null && node.Tag.Contains("!ref") && !ReferenceNodes.ContainsKey(node.Tag);

        static dynamic? ConvertValue(string value, string tag)
        {
            if (tag == null)
                tag = "";

            if (value == "null")
                return null;
            else if (value == "true" || value == "True")
                return true;
            else if (value == "false" || value == "False")
                return false;
            else if (tag == "!u")
                return uint.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!l")
                return int.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!d")
                return double.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!ul")
                return ulong.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!ll")
                return long.Parse(value, CultureInfo.InvariantCulture);
            else if (tag == "!h")
                return Crc32.Compute(value).ToString("x");
            else if (tag == "!p")
                return new BymlPathIndex() { Index = Int32.Parse(value, CultureInfo.InvariantCulture) };
            else
            {
                bool isFloat = float.TryParse(value, out float floatValue);

                if (isFloat)
                    return floatValue;

                return value;
            }
        }

        static YamlNode SaveNode(dynamic node)
        {
            if (node == null)
                return new YamlScalarNode("null");
            else if (node is IList<dynamic> castNode)
            {
                var yamlNode = new YamlSequenceNode();
                // NodePaths.Add(node, yamlNode);

                if (!HasEnumerables(castNode) &&
                    castNode.Count < 6)
                    yamlNode.Style = SharpYaml.YamlStyle.Flow;

                foreach (var item in castNode)
                    yamlNode.Add(SaveNode(item));

                return yamlNode;
            }
            else if (node is IDictionary<string, dynamic> nodeDict)
            {
                var yamlNode = new YamlMappingNode();
              //  NodePaths.Add(node, yamlNode);

                if (!HasEnumerables(nodeDict) && nodeDict.Count < 6)
                    yamlNode.Style = SharpYaml.YamlStyle.Flow;

                foreach (var item in nodeDict)
                {
                    string key = item.Key;
                    YamlNode keyNode = new YamlScalarNode(key);
                    if (IsHash(key))
                    {
                        uint hash = Convert.ToUInt32(key, 16);
                        if (Hashes.ContainsKey(hash))
                            key = $"{Hashes[hash]}";

                        keyNode = new YamlScalarNode(key) {
                            Tag = "!h"
                        };
                    }
                    yamlNode.Add(keyNode, SaveNode(item.Value));
                }
                return yamlNode;
            }
            else if (node is BymlPathPoint castBymlPoint)
                return ConvertPathPoint(castBymlPoint);
            else if (node is List<BymlPathPoint> castList)
            {
                var yamlNode = new YamlSequenceNode();
                foreach (var pt in castList)
                    yamlNode.Add(ConvertPathPoint(pt));
                return yamlNode;
            }
            else
            {
                string? tag = null;
                if (node is int) tag = "!l";
                else if (node is uint) tag = "!u";
                else if (node is long) tag = "!ul";
                else if (node is double) tag = "!d";
                else if (node is BymlPathIndex) tag = "!p";

                var yamlNode = new YamlScalarNode(ConvertValue(node));
                if (tag != null) yamlNode.Tag = tag;
                return yamlNode;
            }
        }

        private static YamlMappingNode ConvertPathPoint(BymlPathPoint point)
        {
            YamlMappingNode node = new() {
                Style = SharpYaml.YamlStyle.Flow
            };
            node.Add("X", point.Position.X.ToString());
            node.Add("Y", point.Position.Y.ToString());
            node.Add("Z", point.Position.Z.ToString());
            node.Add("NX", point.Normal.X.ToString());
            node.Add("NY", point.Normal.Y.ToString());
            node.Add("NZ", point.Normal.Z.ToString());
            node.Add("Value", point.Unknown.ToString());
            return node;
        }

        private static bool HasEnumerables(IDictionary<string, dynamic> node)
        {
            foreach (var item in node.Values)
            {
                if (item == null)
                    continue;
                if (item is IList<dynamic>)
                    return true;
                else if (item is IDictionary<string, dynamic>)
                    return true;
            }
            return false;
        }

        private static bool HasEnumerables(IList<dynamic> node)
        {
            foreach (var _ in node)
            {
                if (node == null)
                    continue;

                if (node is IList<dynamic>)
                    return true;
                else if (node is IDictionary<string, dynamic>)
                    return true;
            }

            return false;
        }

        private static string ConvertValue(dynamic node)
        {
            if (node is bool castBoolNode) return castBoolNode ? "true" : "false";
            else if (node is BymlPathIndex castPathNode) return castPathNode.Index.ToString();
            else if (node is float) return string.Format("{0:0.000.00}", node);
            else return node.ToString();
        }

        private static Dictionary<uint, string> Hashes => CreateHashList();
        private static Dictionary<uint, string> CreateHashList()
        {
            List<string> hashLists = new()
            {
                "AcnhByml",
                "AcnhHeaders",
                "AcnhValues"
            };

            Dictionary<uint, string> hashes = new();

            foreach (var list in hashLists)
            {
                string hashList = new Resource($"Data.{list}").ToString();
                foreach (string hashStr in hashList.Split('\n'))
                    CheckHash(ref hashes, hashStr);
            }

            return hashes;
        }

        private static void CheckHash(ref Dictionary<uint, string> hashes, string hashStr)
        {
            uint hash = Crc32.Compute(hashStr);
            if (!hashes.ContainsKey(hash))
                hashes.Add(hash, hashStr);
        }

        public static bool IsHash(string k) => k != null && IsHex(k.ToArray());
        private static bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;

            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }

            return true;
        }
    }
}
