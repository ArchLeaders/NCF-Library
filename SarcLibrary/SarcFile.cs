using Nintendo.Sarc.Parser;
using Syroot.BinaryData.Core;

namespace Nintendo.Sarc
{
    public class SarcFile
    {
        internal SarcFile() { }
        public SarcFile(Stream stream) => Setter(SARC.DecompileSarc(stream));
        public SarcFile(string file) => Setter(SARC.DecompileSarc(File.OpenRead(file)));
        public SarcFile(byte[] bytes) => Setter(SARC.DecompileSarc(new MemoryStream(bytes)));
        public SarcFile(Dictionary<string, byte[]> files, Endian endianness = Endian.Little, bool hashOnly = false)
        {
            Files = files;
            Endianness = endianness;
            HashOnly = hashOnly;
        }

        internal void Setter(SarcFile sarc)
        {
            Files = sarc.Files;
            Endianness = sarc.Endianness;
            HashOnly = sarc.HashOnly;
        }

        /// <summary>
        /// Gets or sets the archived file dictionary.
        /// </summary>
        public Dictionary<string, byte[]> Files { get; set; } = new();

        /// <summary>
        /// Gets or sets the SARC endianness.
        /// </summary>
        public Endian Endianness { get; set; } = Endian.Little;

        /// <summary>
        /// Gets or sets whether or not to only use hashed file names.
        /// </summary>
        public bool HashOnly { get; set; }

        /// <summary>
        /// Compile to an array of bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBinary() => SARC.CompileSarc(this).Value;

        /// <summary>
        /// Save to a file.
        /// </summary>
        /// <param name="fileName"></param>
        public void Write(string fileName) => File.WriteAllBytes(fileName, SARC.CompileSarc(this).Value);

        public static SarcFile FromBinary(string fileName) => SARC.DecompileSarc(File.OpenRead(fileName));
        public static SarcFile FromBinary(byte[] bytes) => SARC.DecompileSarc(new MemoryStream(bytes));
        public static SarcFile FromBinary(Stream stream) => SARC.DecompileSarc(stream);

        public static byte[] ToBinary(SarcFile sarc) => SARC.CompileSarc(sarc).Value;
    }
}
