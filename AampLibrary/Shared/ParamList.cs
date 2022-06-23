using System;
using System.Text.Json.Serialization;

namespace Nintendo.Aamp
{
    public class ParamList
    {
        /// <summary>
        /// Gets the hash of the param list>
        /// </summary>
        [JsonIgnore]
        public uint Hash;

        /// <summary>
        /// Gets the hash converted string of the param list>
        /// </summary>
        public string HashString
        {
            get => Hashes.GetName(Hash);
            set => Hash = Hashes.SetName(value);
        }

        /// <summary>
        /// Gets the child param list>
        /// </summary>
        public ParamList[] ChildParams { get; set; } = Array.Empty<ParamList>();
        internal NodeMap listMap = new();
        public ParamList? Lists(uint hash)
        {
            int? index = listMap[hash];
            return index != null ? ChildParams[(int)index] : null;
        }
        public ParamList? Lists(string hashString)
        {
            int? index = listMap[hashString];
            return index != null ? ChildParams[(int)index] : null;
        }


        /// <summary>
        /// Gets the param object list>
        /// </summary>
        public ParamObject[] ParamObjects { get; set; } = Array.Empty<ParamObject>();
        internal NodeMap objectMap = new();
        public ParamObject? Objects(uint hash)
        {
            int? index = objectMap[hash];
            return index != null ? ParamObjects[(int)index] : null;
        }
        public ParamObject? Objects(string hashString)
        {
            int? index = objectMap[hashString];
            return index != null ? ParamObjects[(int)index] : null;
        }
    }
}
