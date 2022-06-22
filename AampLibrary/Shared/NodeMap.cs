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
        public int? this[uint hash]
        {
            get
            {
                if (hash_map.TryGetValue(hash, out int index))
                    return index;
                return null;
            }
        }
        public int? this[string hashString]
        {
            get
            {
                uint hash = Crc32.Compute(hashString);
                if (hash_map.TryGetValue(hash, out int index))
                    return index;
                return null;
            }
}
    }
}
