using System.Text;

namespace Nintendo.Aamp
{
    public class StringEntry
    {
        public StringEntry(byte[] data, int maxValue)
        {
            Data = data;
            MaxValue = maxValue;
        }
        public StringEntry(byte[] data) => Data = data;
        public StringEntry(string text) => Data = EncodeType.GetBytes(text);

        public Encoding EncodeType { get; set; } = Encoding.UTF8;
        private int MaxValue { get; set; } = -1;
        public byte[] Data { get; set; }

        public override string ToString() => EncodeType.GetString(Data);
    }
}
