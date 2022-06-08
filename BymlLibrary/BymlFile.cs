using Nintendo.Byml.IO;
using Nintendo.Byml.Parser;
using Newtonsoft.Json;
using Syroot.BinaryData.Core;
using System.Text;
using System.IO;

namespace Nintendo.Byml
{
    public class BymlFile
    {
        //
        // Constructors 
        //
        #region Expand

        internal BymlFile() { }
        public BymlFile(string fileName) => Setter(FromBinary(File.OpenRead(fileName)));
        public BymlFile(byte[] bytes) => Setter(FromBinary(new MemoryStream(bytes)));
        public BymlFile(Stream stream) => Setter(FromBinary(stream));

        #endregion

        //
        // Internal functions
        //
        #region Expand

        internal void Setter(BymlFile byml)
        {
            SupportPaths = byml.SupportPaths;
            Endianness = byml.Endianness;
            RootNode = byml.RootNode;
            Version = byml.Version;
        }

        #endregion

        // 
        // Parameters
        // 
        #region Expand

        public Endian Endianness { get; set; } = Endian.Little;
        public dynamic? RootNode { get; set; }
        public bool SupportPaths { get; set; } = false;
        public ushort Version { get; set; } = 2;

        #endregion

        // 
        // Local functions
        // 
        #region Expand

        public byte[] ToBinary() => ToBinary(this);
        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);
        public string ToXml() => XmlConverter.ToXml(this);
        public string ToYaml() => YamlConverter.ToYaml(this);

        public void WriteBinary(string fileName) => File.WriteAllBytes(fileName, ToBinary());
        public void WriteJson(string fileName) => File.WriteAllText(fileName, ToXml());
        public void WriteXml(string fileName) => File.WriteAllText(fileName, ToXml());
        public void WriteYaml(string fileName) => File.WriteAllText(fileName, ToYaml());

        #endregion

        // 
        // Static functions
        // 
        #region Expand

        public static BymlFile FromBinary(string fileName) => new BymlReader().Read(File.OpenRead(fileName), Encoding.UTF8);
        public static BymlFile FromBinary(byte[] bytes) => new BymlReader().Read(new MemoryStream(bytes), Encoding.UTF8);
        public static BymlFile FromBinary(Stream stream) => new BymlReader().Read(stream, Encoding.UTF8);

        public static BymlFile FromYaml(string text) => YamlConverter.FromYaml(text);
        public static BymlFile FromYamlFile(string fileName) => YamlConverter.FromYaml(File.ReadAllText(fileName));

        public static BymlFile FromXml(string text) => XmlConverter.FromXml(text);
        public static BymlFile FromXmlFile(string fileName) => XmlConverter.FromXml(File.ReadAllText(fileName));

        public static void ToBinary(BymlFile byml, Stream stream) => new BymlWriter(byml).Write(stream, Encoding.UTF8);
        public static byte[] ToBinary(BymlFile byml)
        {
            MemoryStream ms = new();
            ToBinary(byml, ms);
            return ms.ToArray();
        }

        #endregion
    }
}
