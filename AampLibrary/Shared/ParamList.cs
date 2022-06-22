#pragma warning disable CS8603 // Possible null reference return.

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
        internal ParamList[] childLists { get; set; } = Array.Empty<ParamList>();
        internal NodeMap listMap = new();
        public ParamList Lists(uint hash) => childLists[listMap[hash]];
        public ParamList Lists(string hashString) => childLists[listMap[hashString]];


        /// <summary>
        /// Gets the param object list>
        /// </summary>
        internal ParamObject[] childObjects { get; set; } = Array.Empty<ParamObject>();
        internal NodeMap objectMap = new();
        public ParamObject Objects(uint hash) => childObjects[objectMap[hash]];
        public ParamObject Objects(string hashString) => childObjects[objectMap[hashString]];
    }
}
