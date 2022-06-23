using System.Collections.Generic;

namespace Nintendo.Byml
{
    public class BymlNode
    {
        private NodeType type;
        private readonly List<BymlNode> array;
        private readonly Dictionary<string, BymlNode> dict;
        private readonly List<string> str_array;
        private BymlUnion union;
        public NodeType Type { get => type; }
        public List<BymlNode> Array { get => array; }
        public Dictionary<string, BymlNode> Hash { get => dict; }
        public List<string> StringArray { get => str_array; }
        public string String { get => union.String; set => union.String = value; }
        public byte[] Binary { get => union.Binary; set => union.Binary = value; }
        public bool Bool { get => union.Bool; set => union.Bool = value; }
        public int Int { get => union.Int; set => union.Int = value; }
        public float Float { get => union.Float; set => union.Float = value; }
        public uint UInt { get => union.UInt; set => union.UInt = value; }
        public long Int64 { get => union.Int64; set => union.Int64 = value; }
        public ulong UInt64 { get => union.UInt64; set => union.UInt64 = value; }
        public double Double { get => union.Double; set => union.Double = value; }

        public BymlNode()
        {
            type = NodeType.Null;
        }
        public BymlNode(string str)
        {
            union.String = str;
            type = NodeType.String;
        }
        public BymlNode(byte[] bytes)
        {
            union.Binary = bytes;
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
            union.Bool = b;
            type = NodeType.Bool;
        }
        public BymlNode(int i)
        {
            union.Int = i;
            type = NodeType.Int;
        }
        public BymlNode(float f)
        {
            union.Float = f;
            type = NodeType.Float;
        }
        public BymlNode(uint u)
        {
            union.UInt = u;
            type = NodeType.UInt;
        }
        public BymlNode(long l)
        {
            union.Int64 = l;
            type = NodeType.Int64;
        }
        public BymlNode(ulong ul)
        {
            union.UInt64 = ul;
            type = NodeType.UInt64;
        }
        public BymlNode(double d)
        {
            union.Double = d;
            type = NodeType.Double;
        }
    }
}
