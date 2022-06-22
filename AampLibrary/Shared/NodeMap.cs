using System.Collections.Generic;
using Aamp.Security.Cryptography;

namespace Nintendo.Aamp
{
    public class NodeMap
    {
        private readonly Dictionary<uint, int> hash_map = new();
        public void Add(uint hash, int index)
        {
            hash_map[hash] = index;
        }
        public int this[uint hash] => hash_map[hash];
        public int this[string hashString] => hash_map[Crc32.Compute(hashString)];
    }
}
