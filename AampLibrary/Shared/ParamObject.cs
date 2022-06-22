using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Nintendo.Aamp
{
    public class ParamObject
    {
        public T GetEntryValue<T>(string hashName) where T : new()
        {
            T instance = new();
            foreach (var entry in ParamEntries) {
                if (entry.HashString == hashName) {
                    if (entry.Value.GetType() == instance.GetType())
                        return (T)entry.Value;
                }
            }
            return instance;
        }

        public void SetEntryValue(string hashName, object value)
        {
            ParamEntry? entry = ParamEntries.FirstOrDefault(x => x.HashString == hashName);
            if (entry != null) {
                entry.Value = value;
            }
            else {
                entry = new ParamEntry {
                    HashString = hashName,
                    Value = value
                };
            }
        }

        /// <summary>
        /// Gets the hash of the object name>
        /// </summary>
        [JsonIgnore]
        public uint Hash;

        /// <summary>
        /// Gets the hash of the group name>
        /// </summary>
        public uint GroupHash = 0xCDCDCDCD;

        /// <summary>
        /// Gets the hash converted string of the object name>
        /// </summary>
        public string HashString
        {
            get => Hashes.GetName(Hash);
            set => Hash = Hashes.SetName(value);
        }

        /// <summary>
        /// Gets the param object list>
        /// </summary>
        public ParamEntry[] ParamEntries { get; set; } = Array.Empty<ParamEntry>();
        internal NodeMap paramMap = new();
        public ParamEntry? Params(uint hash)
        {
            int? index = paramMap[hash];
            return index != null ? ParamEntries[(int)index] : null;
        }
        public ParamEntry? Params(string hashString)
        {
            int? index = paramMap[hashString];
            return index != null ? ParamEntries[(int)index] : null;
        }
    }
}
