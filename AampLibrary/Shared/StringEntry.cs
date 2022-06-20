using System;
using System.Text;

namespace Nintendo.Aamp
{
    public class StringEntry
    {
        public StringEntry(byte[] data, int maxValue)
        {
            string data_string = EncodeType.GetString(data);
            Data = (data_string == "''" || data_string == "\"\"") ? Array.Empty<byte>() : data;
            MaxValue = maxValue;
        }
        public StringEntry(byte[] data) => Data = data;
        public StringEntry(string text) => Data = (text == "''" || text == "\"\"") ? Array.Empty<byte>() : EncodeType.GetBytes(text);

        public Encoding EncodeType { get; set; } = Encoding.UTF8;
        private int MaxValue { get; set; } = -1;
        public byte[] Data { get; set; }

        public override string ToString() => Data.Length == 0 ? "''" : EncodeType.GetString(Data);
    }
}
