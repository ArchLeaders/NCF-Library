using System.Collections.Generic;

namespace Nintendo.Byml
{
    public class BymlNode
    {
        private NodeType type;
        private readonly List<BymlNode> array;
        private readonly Dictionary<string, BymlNode> dict;
        private readonly List<string> str_array;
        public NodeType Type { get => type; }
        public string String { get; set; }
        public byte[] Binary { get; set; }
        public List<BymlNode> Array { get => array; }
        public Dictionary<string, BymlNode> Hash { get => dict; }
        public List<string> StringArray { get => str_array; }
        public bool Bool { get; set; }
        public int Int { get; set; }
        public float Float { get; set; }
        public uint UInt { get; set; }
        public long Int64 { get; set; }
        public ulong UInt64 { get; set; }
        public double Double { get; set; }

        public BymlNode()
        {
            type = NodeType.Null;
        }
        public BymlNode(string str)
        {
            String = str;
            type = NodeType.String;
        }
        public BymlNode(byte[] bytes)
        {
            Binary = bytes;
            type = NodeType.Binary;
        }
        public BymlNode(List<BymlNode> array)
        {
            this.array = array;
            type = NodeType.Array;
        }
        public BymlNode(Dictionary<string, BymlNode> dict)
        {
            this.dict = dict;
            type = NodeType.Hash;
        }
        public BymlNode(List<string> str_array)
        {
            this.str_array = str_array;
            type = NodeType.StringArray;
        }
        public BymlNode(bool b)
        {
            Bool = b;
            type = NodeType.Bool;
        }
        public BymlNode(int i)
        {
            Int = i;
            type = NodeType.Int;
        }
        public BymlNode(float f)
        {
            Float = f;
            type = NodeType.Float;
        }
        public BymlNode(uint u)
        {
            UInt = u;
            type = NodeType.UInt;
        }
        public BymlNode(long l)
        {
            Int64 = l;
            type = NodeType.Int64;
        }
        public BymlNode(ulong ul)
        {
            UInt64 = ul;
            type = NodeType.UInt64;
        }
        public BymlNode(double d)
        {
            Double = d;
            type = NodeType.Double;
        }
    }
}
