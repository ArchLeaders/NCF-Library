using System.Text.Json.Serialization;

namespace Nintendo.Aamp
{
    public class ParamEntry
    {
        /// <summary>
        /// Gets the hash of the object name>
        /// </summary>
        [JsonIgnore]
        public uint Hash { get; set; }

        /// <summary>
        /// Gets the ParamType of the entry>
        /// </summary>
        [JsonIgnore]
        public ParamType ParamType { get; set; }

        /// <summary>
        /// Gets the hash converted string of the object name>
        /// </summary>
        public string HashString
        {
            get => Hashes.GetName(Hash);
            set => Hash = Hashes.SetName(value);
        }

        /// <summary>
        /// Gets or sets the value of the data>
        /// </summary>
        public object Value { get; set; } = new();
    }
}
