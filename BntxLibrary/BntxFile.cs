using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using Syroot.BinaryData;
using Syroot.BinaryData.Core;
using BntxLibrary.Core;
using BntxLibrary.Common;

namespace BntxLibrary
{
    /// <summary>
    /// Represents a NintendoWare for Cafe (NW4F) graphics data archive file.
    /// </summary>
    [DebuggerDisplay(nameof(BntxFile) + " {" + nameof(Name) + "}")]
    public class BntxFile : IResData
    {
        internal uint FileSizeToRLT;
        internal byte[] OriginalRLTChunk;

        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const string _signature = "BNTX";

        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="BntxFile"/> class.
        /// </summary>
        public BntxFile()
        {
            StringTable = new StringTable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BntxFile"/> class from the given <paramref name="stream"/> which
        /// is optionally left open.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load the data from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave the stream open after reading, otherwise <c>false</c>.</param>
        public BntxFile(Stream stream, bool leaveOpen = false)
        {
            using (BntxFileLoader loader = new BntxFileLoader(this, stream, leaveOpen))
            {
                loader.Execute();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BntxFile"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to load the data from.</param>
        public BntxFile(string fileName)
        {
            using (BntxFileLoader loader = new BntxFileLoader(this, fileName))
            {
                loader.Execute();
            }
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets a name describing the contents.
        /// </summary>
        [Browsable(true)]
        [Description("The name of the file.")]
        [Category("Header")]
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the target platform as a string.
        /// </summary>
        [Browsable(true)]
        [Description("The target platform. 'NX  ' for Switch, 'Gen ' for PC.")]
        [Category("Header")]
        [DisplayName("Platform Target")]
        public string PlatformTarget
        {
            get
            {
                return new string(Target);
            }
            set
            {
                Target = value.ToCharArray();
            }
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Versions")]
        [DisplayName("Full Version")]
        public string VersionFull
        {
            get { return $"{VersionMajor}.{VersionMajor2}.{VersionMinor}.{VersionMinor2}"; }
        }

        /// <summary>
        /// Gets or sets the major revision of the BNTX structure formats.
        /// </summary>
        [Browsable(true)]
        [Category("Versions")]
        [DisplayName("Version Major 1")]
        public uint VersionMajor { get; set; }
        /// <summary>
        /// Gets or sets the second major revision of the BNTX structure formats.
        /// </summary>
        [Browsable(true)]
        [Category("Versions")]
        [DisplayName("Version Major 2")]
        public uint VersionMajor2 { get; set; }
        /// <summary>
        /// Gets or sets the minor revision of the BNTX structure formats.
        /// </summary>
        [Browsable(true)]
        [Category("Versions")]
        [DisplayName("Version Minor 1")]
        public uint VersionMinor { get; set; }
        /// <summary>
        /// Gets or sets the second minor revision of the BNTX structure formats.
        /// </summary>
        [Browsable(true)]
        [Category("Versions")]
        [DisplayName("Version Minor 2")]
        public uint VersionMinor2 { get; set; }

        /// <summary>
        /// Gets the byte order in which data is stored. Must be the endianness of the target platform.
        /// </summary>
        [Browsable(false)]
        public Syroot.BinaryData.ByteConverter ByteConverter { get; private set; }

        /// <summary>
        /// Gets or sets the alignment to use for raw data blocks in the file.
        /// </summary>
        [Browsable(false)]
        public uint Alignment { get; set; }

        /// <summary>
        /// Gets or sets the data alignment to use for raw data blocks in the file.
        /// </summary>
        [ReadOnly(true)]
        [Description("The name of the file.")]
        [Category("Header")]
        [DisplayName("Alignment")]
        public int DataAlignment
        {
            get
            {
                return (1 << (int)Alignment);
            }
        }

        /// <summary>
        /// Gets or sets the target adress size to use for raw data blocks in the file.
        /// </summary>
        [Description("The target adress size to use for raw data blocks in the file.")]
        [Category("Header")]
        [DisplayName("Target Address Size")]
        public uint TargetAddressSize { get; set; }

        /// <summary>
        /// Gets or sets the flag. Unknown purpose.
        /// </summary>
        [Category("Header")]
        [DisplayName("Flag")]
        public uint Flag { get; set; }

        /// <summary>
        /// Gets or sets the BlockOffset. 
        /// </summary>
        [Browsable(false)]
        public uint BlockOffset { get; set; }

        /// <summary>
        /// Gets or sets the stored <see cref="RelocationTable"/> (_RLT) instance.
        /// </summary>
        [Browsable(false)]
        public RelocationTable RelocationTable { get; set; }

        /// <summary>
        /// Gets or sets the target platform.
        /// </summary>
        [Browsable(false)]
        public char[] Target { get; set; }

        /// <summary>
        /// Gets or sets a list of the stored <see cref="TextureInfo"/> instances.
        /// </summary>
        [Browsable(false)]
        public IList<Texture> Textures { get; set; }

        [Browsable(false)]
        public ResDict TextureDict { get; set; }

        [Browsable(false)]
        public StringTable StringTable { get; set; }

        internal bool WriteOriginalRLT()
        {
            if (originalFileData == null)
            {
                System.Console.WriteLine("Texture original data not set ");
                return false;
            }

            if (Name != originalFileName)
                return false;

            if (Textures.Count != originalFileData.Data.Count)
            {
                System.Console.WriteLine("Texture count not matched " + Textures.Count);
                return false;
            }

            foreach (var tex in Textures)
            {
                if (originalFileData.Data.ContainsKey(tex.Name))
                {
                    var ogImage = originalFileData.Data[tex.Name];
                    if (ogImage.ImageSize != tex.ImageSize)
                    {
                        System.Console.WriteLine("Image Size not matched " + tex.Name);
                        return false;
                    }
                    if (tex.UserData?.Count > 0)
                    {
                        return false;
                    }
                    if (ogImage.MipMapCount != tex.MipCount)
                    {
                        System.Console.WriteLine("Mip Count not matched " + tex.Name);
                        return false;
                    }
                    if (ogImage.UserDataCount != tex.UserData.Count)
                    {
                        System.Console.WriteLine("User Data not matched " + tex.Name);
                        return false;
                    }
                }
                else
                {
                    System.Console.WriteLine("Texture not matched " + tex.Name);
                    return false;
                }
            }

            return true;
        }

        private string originalFileName = "";

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Saves the contents in the given <paramref name="stream"/> and optionally leaves it open
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save the contents into.</param>
        /// <param name="leaveOpen"><c>true</c> to leave the stream open after writing, otherwise <c>false</c>.</param>
        public void Save(Stream stream, bool leaveOpen = false)
        {
            using (BntxFileWriter writer = new(this, stream, leaveOpen))
            {
                writer.Execute();
            }
        }

        /// <summary>
        /// Saves the contents in the file with the given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to save the contents into.</param>
        public void Save(string fileName)
        {
            using (BntxFileWriter writer = new(this, fileName))
            {
                writer.Execute();
            }
        }

        internal uint SaveVersion()
        {
            return VersionMajor << 24 | VersionMajor2 << 16 | VersionMinor << 8 | VersionMinor2;
        }

        private void SetVersionInfo(uint Version)
        {
            VersionMajor = Version >> 24;
            VersionMajor2 = Version >> 16 & 0xFF;
            VersionMinor = Version >> 8 & 0xFF;
            VersionMinor2 = Version & 0xFF;
        }

        private OriginalFileData originalFileData;

        private class OriginalFileData
        {
            internal Dictionary<string, OriginalImageData> Data = new Dictionary<string, OriginalImageData>();

            internal class OriginalImageData
            {
                public string Name;
                public uint MipMapCount;
                public uint ArrayCount;
                public uint ImageSize;
                public uint UserDataCount;
                public long[] MipOffsets;
            }
        }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IResData.Load(BntxFileLoader loader)
        {
            originalFileData = new OriginalFileData();
            StringTable = new StringTable();

            loader.CheckSignature(_signature);
            uint padding = loader.ReadUInt32();
            uint Version = loader.ReadUInt32();
            SetVersionInfo(Version);
            ByteConverter = Syroot.BinaryData.ByteConverter.GetConverter(loader.ReadEnum<Endian>(false));
            Alignment = (byte)loader.ReadByte();
            TargetAddressSize = (byte)loader.ReadByte(); //Thanks MasterF0X for pointing out the layout of the these
            uint OffsetToFileName = loader.ReadUInt32();
            Flag = loader.ReadUInt16();
            BlockOffset = loader.ReadUInt16();
            uint RelocationTableOffset = loader.ReadUInt32();
            uint sizFile = loader.ReadUInt32();
            Target = System.Text.Encoding.ASCII.GetString(loader.ReadBytes(4)).ToCharArray();
            int textureCount = loader.ReadInt32();
            long TextureArrayOffset = loader.ReadInt64();
            long TextureData = loader.ReadInt64();
            TextureDict = loader.LoadDict();
            Name = loader.LoadString(null, OffsetToFileName - 2);

            FileSizeToRLT = RelocationTableOffset;
            OriginalRLTChunk = loader.LoadCustom(() =>
            {
                return loader.ReadBytes((int)(loader.BaseStream.Length - FileSizeToRLT));
            }, RelocationTableOffset);

            Textures = loader.LoadCustom(() =>
            {
                IList<Texture> texList = new List<Texture>();
                for (int i = 0; i < textureCount; i++)
                {
                    texList.Add(loader.Load<Texture>());
                }
                return texList;
            }, TextureArrayOffset);

            originalFileName = Name;

            loader.Seek(TextureArrayOffset + (textureCount * sizeof(long)), SeekOrigin.Begin);

            StringTable.Load(loader);

            foreach (Texture tex in Textures)
            {
                var originalImage = new OriginalFileData.OriginalImageData();
                originalImage.Name = tex.Name;
                originalImage.MipMapCount = tex.MipCount;
                originalImage.ArrayCount = tex.ArrayLength;
                originalImage.ImageSize = tex.ImageSize;
                originalImage.UserDataCount =(uint)tex.UserData.Count;
                originalImage.MipOffsets = tex.MipOffsets;

                originalFileData.Data.Add(tex.Name, originalImage);
            }
        }

        internal long TextureDictOffset;
        internal long TextureArrayOffset;

        void IResData.Save(BntxFileWriter writer)
        {
            PreSave(); 

            writer.WriteSignature(_signature);
            writer.Write(0);
            writer.Write(SaveVersion());
            if (ByteConverter.Endian == Endian.None)
                ByteConverter = Syroot.BinaryData.ByteConverter.Big;

            writer.WriteEnum(ByteConverter.Endian, true);
            writer.Write((byte)Alignment);
            writer.Write((byte)TargetAddressSize);
            writer.SaveFileNameString(Name);
            writer.Write((ushort)Flag);
            writer.SaveHeaderBlock(true);
            writer.SaveRelocationTable();
            writer.SaveFieldFileSize();

            //Start of container info
            writer.Write(System.Text.Encoding.ASCII.GetBytes(Target));
            writer.Write(TextureDict.Count);
            writer.SaveRelocateEntryToSection(writer.Position, 1, 2, 1, BntxFileWriter.Section1, "Texture Array Offset"); //      <------------ Entry Set
            TextureArrayOffset = writer.SaveOffset();
            writer.SaveRelocateEntryToSection(writer.Position, 1, 1, 0, BntxFileWriter.Section2, "Texture Block Array"); //      <------------ Entry Set
            writer.SaveTextureDataBlocks();
            TextureDictOffset = writer.SaveOffset();
            writer.SaveRelocateEntryToSection(writer.Position, 1, 1, 0, BntxFileWriter.Section1, "Memory Pool"); //      <------------ Entry Set
            writer.Write((ulong)0x58);
            writer.Write(0L);
            writer.Write(0L);
            writer.Write(new byte[0x140]); //Space for memory ppool
            writer.SaveRelocateEntryToSection(writer.Position, (uint)TextureDict.Count, 1, 0, BntxFileWriter.Section1, "Texture Array"); //      <------------ Entry Set
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private void PreSave()
        {
            // Update Texture dictionary.
            TextureDict.Clear();
            foreach (Texture tex in Textures)
                TextureDict.Add(tex.Name);
        }
    }
}
