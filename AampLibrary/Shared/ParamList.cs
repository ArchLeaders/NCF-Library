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
        public ParamList[] ChildParams { get; set; } = Array.Empty<ParamList>();

        /// <summary>
        /// Gets the param object list>
        /// </summary>
        public ParamObject[] ParamObjects { get; set; } = Array.Empty<ParamObject>();
    }
}
