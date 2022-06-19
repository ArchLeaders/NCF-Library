using Nintendo.Aamp.IO;
using Nintendo.Aamp.Parser;
using Nintendo.Aamp.Shared;
using Newtonsoft.Json;
using Syroot.BinaryData;
using System.Text;
using System.IO;
using System;

namespace Nintendo.Aamp
{

    public class AampFile
    {
        //
        // Constructors 
        //
        #region Expand

        internal AampFile()
        {
            ParameterIOVersion = 0;
            ParameterIOType = "xml";
            Endianness = 3; // encoding is the second bit of the endianness byte
            RootNode = new() { Hash = 0xA4F6CB6C };
        }
        public AampFile(string fileName)
        {
            using FileStream stream = File.OpenRead(fileName);
            Setter(FromBinary(stream));
        }
        public AampFile(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);
            Setter(FromBinary(stream));
        }

        public AampFile(Stream stream) => Setter(FromBinary(stream));

        #endregion

        //
        // Internal functions
        //
        #region Expand

        private void Setter(AampFile aamp)
        {
            ParameterIOVersion = aamp.ParameterIOVersion;
            ParameterIOType = aamp.ParameterIOType;
            UnknownValue = aamp.UnknownValue;
            Endianness = aamp.Endianness;
            RootNode = aamp.RootNode;
            Version = aamp.Version;
        }

        #endregion

        // 
        // Parameters
        // 
        #region Expand

        public string EffectType => ParamEffect.GetEffectType(ParameterIOType);
        public uint Endianness { get; internal set; } = 0x01000000;
        public string ParameterIOType { get; set; }
        public uint ParameterIOVersion { get; internal set; }
        public ParamList RootNode { get; set; }
        public uint UnknownValue { get; set; } = 0x01000000;
        public uint Version { get; internal set; }

        #endregion

        // 
        // Local functions
        // 
        #region Expand

        public void ToVersion1() => Setter(ConvertToVersion1());
        internal AampFileV1 ConvertToVersion1()
        {
            return new AampFileV1() {
                Endianness = Endianness,
                ParameterIOType = ParameterIOType,
                ParameterIOVersion = 0,
                RootNode = RootNode,
                Version = 1,
                UnknownValue = 0,
                EffectName = Encoding.UTF8.GetBytes(ParameterIOType),
            };
        }

        public void ToVersion2() => Setter(ConvertToVersion2());
        internal AampFileV2 ConvertToVersion2()
        {
            return new AampFileV2() {
                Endianness = Endianness,
                ParameterIOType = ParameterIOType,
                ParameterIOVersion = 0, // previously 410 - why was this explicitly set to 410?
                RootNode = RootNode,
                Version = 2,
                UnknownValue = 0,
            };
        }

        public byte[] ToBinary() => ToBinary(this);
        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);
        public string ToYml() => YamlConverter.ToYaml(this);

        public void WriteBinary(string fileName) => File.WriteAllBytes(fileName, ToBinary(this));
        public void WriteJson(string fileName) => File.WriteAllText(fileName, JsonConvert.SerializeObject(this, Formatting.Indented));
        public void WriteYaml(string fileName) => File.WriteAllText(fileName, ToYml());

        #endregion

        // 
        // Static functions
        // 
        #region Expand

        public static AampFile New(int version)
        {
            return version switch
            {
                1 => new AampFileV1(),
                2 => new AampFileV2(),
                _ => throw new ArgumentException($"Invalid AampFile version {version}"),
            };
        }

        private static uint CheckVersion(Stream stream)
        {
            using FileReader reader = new(stream, true);
            reader.ByteConverter = ByteConverter.Little;
            reader.Position = 4;

            return reader.ReadUInt32();
        }

        public static AampFile FromBinary(string fileName)
        {
            using FileStream stream = File.OpenRead(fileName);
            return FromBinary(stream);
        }
        public static AampFile FromBinary(byte[] bytes)
        {
            using MemoryStream stream = new(bytes);
            return FromBinary(stream);
        }
        public static AampFile FromBinary(Stream stream)
        {
            uint version = CheckVersion(stream);

            if (version == 2)
                return new AampFileV2(stream);
            else
                return new AampFileV1(stream);
        }

        public static AampFile FromYml(string text) => YamlConverter.FromYaml(text);
        public static AampFile FromYmlFile(string fileName) => YamlConverter.FromYaml(File.ReadAllText(fileName));

        public static byte[] ToBinary(AampFile aampFile)
        {
            if (aampFile.Version == 2)
                return aampFile.ConvertToVersion2().CompileV2();
            else
                return aampFile.ConvertToVersion1().CompileV1();
        }

        #endregion
    }
}
