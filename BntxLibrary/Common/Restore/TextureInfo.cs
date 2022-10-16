using System.IO;
using System.Diagnostics;
using BntxLibrary.Core;
using BntxLibrary.GFX;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.InteropServices;

namespace BntxLibrary.Common.Restore
{
    /// <summary>
    /// Represents an FMDL subfile in a <see cref="BntxFile"/>, storing multi-dimensional texture data.
    /// </summary>
    [DebuggerDisplay(nameof(TextureInfo) + " {" + nameof(Name) + "}")]
    public class TextureInfo : IResData
    {
        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const string _signature = "BRTI";


        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class from the given <paramref name="stream"/> which
        /// is optionally left open.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load the data from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave the stream open after reading, otherwise <c>false</c>.</param>
        public void Import(Stream stream, bool leaveOpen = false)
        {
            using (var loader = new BntxFileLoader(this, stream, leaveOpen))
            {
                loader.ImportTexture();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResFile"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to load the data from.</param>
        public void Import(string fileName)
        {
            using (var loader = new BntxFileLoader(this, fileName))
            {
                loader.ImportTexture();
            }
        }

        /// <summary>
        /// Saves the contents in the given <paramref name="stream"/> and optionally leaves it open
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to save the contents into.</param>
        /// <param name="leaveOpen"><c>true</c> to leave the stream open after writing, otherwise <c>false</c>.</param>
        public void Export(Stream stream, BntxFile BntxFile, bool leaveOpen = false)
        {
            using (var saver = new BntxFileWriter(this, BntxFile, stream, leaveOpen))
            {
                saver.ExportTexture();
            }
        }

        /// <summary>
        /// Saves the contents in the file with the given <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The name of the file to save the contents into.</param>
        public void Export(string fileName, BntxFile BntxFile)
        {
            using (var saver = new BntxFileWriter(this, BntxFile, fileName))
            {
                saver.ExportTexture();
            }
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the source channel to map to the R (red) channel.
        /// </summary>
        [Browsable(true)]
        [Description("The source channel to map to the R (red) channel.")]
        [Category("Channels")]
        [DisplayName("Red Channel")]
        public ChannelType ChannelRed { get; set; }

        /// <summary>
        /// Gets or sets the source channel to map to the G (green) channel.
        /// </summary>
        [Browsable(true)]
        [Description("The source channel to map to the G (green) channel.")]
        [Category("Channels")]
        [DisplayName("Green Channel")]
        public ChannelType ChannelGreen { get; set; }

        /// <summary>
        /// Gets or sets the source channel to map to the B (blue) channel.
        /// </summary>
        [Browsable(true)]
        [Description("The source channel to map to the B (blue) channel.")]
        [Category("Channels")]
        [DisplayName("Blue Channel")]
        public ChannelType ChannelBlue { get; set; }

        /// <summary>
        /// Gets or sets the source channel to map to the A (alpha) channel.
        /// </summary>
        [Browsable(true)]
        [Description("The source channel to map to the A (alpha) channel.")]
        [Category("Channels")]
        [DisplayName("Alpha Channel")]
        public ChannelType ChannelAlpha { get; set; }

        /// <summary>
        /// Gets or sets the width of the texture.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Width of the image")]
        [Category("Image Info")]
        [DisplayName("Width")]
        public uint Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the texture.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Height of the image")]
        [Category("Image Info")]
        [DisplayName("Height")]
        public uint Height { get; set; }

        /// <summary>
        /// Gets or sets the number of mipmaps stored in the <see cref="MipData"/>.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Number of mip maps")]
        [Category("Image Info")]
        [DisplayName("Mip Count")]
        public uint MipCount { get; set; }

        /// <summary>
        /// Gets or sets the desired texture data buffer format.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Format")]
        [Category("Image Info")]
        [DisplayName("Format")]
        public SurfaceFormat Format { get; set; }



        [Browsable(true)]
        [Description("Change the format to enable or disable SRGB")]
        [Category("Image Info")]
        [DisplayName("Use SRGB")]
        public bool UseSRGB
        {
            get
            {
                var DataType = (byte)((int)Format >> 0 & 0xff);
                if (DataType == 0x06)
                    return true;
                else
                    return false;
            }
            set
            {
                var format = (SurfaceFormat)((int)Format >> 8 & 0xff);

                if (value == true)
                {
                    Format = (SurfaceFormat)((int)format << 8 | 0x06 << 0);
                }
                else
                {
                    Format = (SurfaceFormat)((int)format << 8 | 0x01 << 0);
                }
            }
        }

        /// <summary>
        /// Gets or sets the name with which the instance can be referenced uniquely in <see cref="ResDict{Texture}"/>
        /// instances.
        /// </summary>
        [Browsable(true)]
        [Description("Name")]
        [Category("Image Info")]
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path of the file which originally supplied the data of this instance.
        /// </summary>
        [Browsable(true)]
        [Description("The path the file was originally located.")]
        [Category("Image Info")]
        [DisplayName("Path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the depth of the texture.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Depth")]
        [DisplayName("Depth")]
        public uint Depth { get; set; }

        /// <summary>
        /// Gets or sets the tiling mode.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Tiling mode")]
        [DisplayName("Tile Mode")]
        public TileMode TileMode { get; set; }


        /// <summary>
        /// Gets or sets the swizzling value.
        /// </summary>
        [Browsable(true)]
        [Description("Swizzle")]
        [DisplayName("Swizzle")]
        public uint Swizzle { get; set; }

        /// <summary>
        /// Gets or sets the swizzling alignment.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Alignment")]
        [DisplayName("Alignment")]
        public int Alignment { get; set; }

        /// <summary>
        /// Gets or sets the pixel swizzling stride.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("The pixel swizzling stride")]
        [DisplayName("Pitch")]
        public uint Pitch { get; set; }

        /// <summary>
        /// Gets or sets the dims of the texture.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Dims of the texture")]
        [DisplayName("Dims")]
        public Dim Dim { get; set; }

        /// <summary>
        /// Gets or sets the shape of the texture.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Shape of texture")]
        [DisplayName("Surface Shape")]
        public SurfaceDim SurfaceDim { get; set; }

        /// <summary>
        /// Gets or sets the offsets in the <see cref="MipData"/> array to the data of the mipmap level corresponding
        /// to the array index.
        /// </summary>
        [Browsable(false)]
        public long[] MipOffsets { get; set; }

        /// <summary>
        /// The raw bytes of texture data stored for each mip map
        /// </summary>
        [Browsable(false)]
        public List<List<byte[]>> TextureData { get; set; }

        [Browsable(false)]
        public uint textureLayout { get; set; }

        [Browsable(false)]
        public uint textureLayout2 { get; set; }

        [Description("GPU access flags")]
        [Category("Image Info")]
        [DisplayName("Access Flags")]
        public AccessFlags AccessFlags { get; set; }

        [Browsable(false)]
        public uint[] Regs { get; set; }

        [Browsable(false)]
        public uint ArrayLength { get; set; }

        /// <summary>
        /// Gets or sets info flags
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Flags")]
        [DisplayName("Flags")]
        public byte Flags { get; set; }


        /// <summary>
        /// Gets or sets the image size
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("size of image")]
        [DisplayName("Image Size")]
        public uint ImageSize { get; set; }

        /// <summary>
        /// Gets or sets sample amount
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Sample count")]
        [DisplayName("Sample Count")]
        public uint SampleCount { get; set; }

        [Browsable(false)]
        public ResDict UserDataDict { get; set; }

        [Browsable(false)]
        public IList<UserData> UserData { get; set; }

        [Browsable(false)]
        public int ReadTextureLayout { get; set; }
        [Browsable(false)]
        public int sparseBinding { get; set; }
        [Browsable(false)]
        public int sparseResidency { get; set; }
        [Browsable(false)]
        public uint BlockHeightLog2 { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        int GetTotalSize()
        {
            return TextureData.Sum(o => o[0].Length);
        }

        void IResData.Load(BntxFileLoader loader)
        {
            loader.CheckSignature(_signature);
            loader.LoadHeaderBlock();
            Flags = (byte)loader.ReadByte();
            Dim = loader.ReadEnum<Dim>(true);
            TileMode = loader.ReadEnum<TileMode>(true);
            Swizzle = loader.ReadUInt16();
            MipCount = loader.ReadUInt16();
            SampleCount = loader.ReadUInt32();
            Format = loader.ReadEnum<SurfaceFormat>(true);

            AccessFlags = loader.ReadEnum<AccessFlags>(false);
            Width = loader.ReadUInt32();
            Height = loader.ReadUInt32();
            Depth = loader.ReadUInt32();
            ArrayLength = loader.ReadUInt32();
            textureLayout = loader.ReadUInt32();
            textureLayout2 = loader.ReadUInt32();
            var reserved = loader.ReadBytes(20);
            ImageSize = loader.ReadUInt32();

            if (ImageSize == 0)
                throw new Exception("Empty image size!");

            Alignment = loader.ReadInt32();
            var ChannelType = loader.ReadUInt32();
            SurfaceDim = loader.ReadEnum<SurfaceDim>(true);
            Name = loader.LoadString();
            var ParentOffset = loader.ReadInt64();
            var PtrOffset = loader.ReadInt64();
            var UserDataOffset = loader.ReadInt64();
            var TexPtr = loader.ReadInt64();
            var TexView = loader.ReadInt64();
            var descSlotDataOffset = loader.ReadInt64();
            UserDataDict = loader.LoadDict();

            UserData = loader.LoadList<UserData>(UserDataDict.Count, UserDataOffset);
            MipOffsets = loader.LoadCustom(() => loader.ReadInt64s((int)MipCount), PtrOffset);

            ChannelRed = (ChannelType)(ChannelType >> 0 & 0xff);
            ChannelGreen = (ChannelType)(ChannelType >> 8 & 0xff);
            ChannelBlue = (ChannelType)(ChannelType >> 16 & 0xff);
            ChannelAlpha = (ChannelType)(ChannelType >> 24 & 0xff);
            TextureData = new List<List<byte[]>>();

            ReadTextureLayout = Flags & 1;
            sparseBinding = Flags >> 1;
            sparseResidency = Flags >> 2;
            BlockHeightLog2 = textureLayout & 7;

            var ArrayOffset = 0;
            for (var a = 0; a < ArrayLength; a++)
            {
                var mips = new List<byte[]>();
                for (var i = 0; i < MipCount; i++)
                {
                    var size = (int)((MipOffsets[0] + ImageSize - MipOffsets[i]) / ArrayLength);
                    using (loader.TemporarySeek(ArrayOffset + MipOffsets[i], SeekOrigin.Begin))
                    {
                        mips.Add(loader.ReadBytes(size));
                    }
                    if (mips[i].Length == 0)
                        throw new Exception($"Empty mip size! Texture {Name} ImageSize {ImageSize} mips level {i} sizee {size} ArrayLength {ArrayLength}");
                }
                TextureData.Add(mips);

                ArrayOffset += mips[0].Length;
            }

            var mip = 0;
            var StartMip = MipOffsets[0];
            foreach (var offset in MipOffsets)
                MipOffsets[mip++] = offset - StartMip;
        }
        internal long PosUserDataOffset;
        internal long PosUserDataDictOffset;

        void IResData.Save(BntxFileWriter writer)
        {
            var Channels = (int)ChannelAlpha << 24 | (int)ChannelBlue << 16 | (int)ChannelGreen << 8 | (int)ChannelRed;

            if (ReadTextureLayout != 1)
                textureLayout = 0;
            else if (writer.BntxFile.VersionMajor2 == 4 && writer.BntxFile.VersionMinor >= 1)
                textureLayout = BlockHeightLog2;
            else
                textureLayout = (uint)(sparseResidency << 5 | sparseBinding << 4 | (int)BlockHeightLog2);

            Console.WriteLine($"sparseResidency {sparseResidency} sparseBinding {sparseBinding} BlockHeightLog2 {BlockHeightLog2}");

            Flags = (byte)(sparseResidency << 2 | sparseBinding << 1 | ReadTextureLayout);

            writer.WriteSignature(_signature);
            writer.SaveHeaderBlock();
            var TexturePos = writer.Position;
            writer.Write(Flags);
            writer.WriteEnum(Dim, true);
            writer.WriteEnum(TileMode, true);
            writer.Write((ushort)Swizzle);
            writer.Write((ushort)TextureData[0].Count);
            writer.Write(SampleCount);
            writer.WriteEnum(Format, true);
            writer.WriteEnum(AccessFlags, false);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(Depth);
            writer.Write(TextureData.Count);
            writer.Write(textureLayout);
            writer.Write(textureLayout2);
            writer.Seek(20); //reserved
            writer.Write(GetTotalSize());
            writer.Write(Alignment);
            writer.Write((uint)Channels);
            writer.WriteEnum(SurfaceDim, true);
            writer.SaveRelocateEntryToSection(writer.Position, 3, 1, 0, BntxFileWriter.Section1, "Texture Info"); //      <------------ Entry Set
            writer.SaveString(Name);
            writer.Write((long)0x20); //ParentOffset
            var PosDataOffsets = writer.SaveOffset();
            writer.SaveRelocateEntryToSection(writer.Position, 1, 1, 0, BntxFileWriter.Section1, "User Data"); //      <------------ Entry Set
            PosUserDataOffset = writer.SaveOffset();
            writer.SaveRelocateEntryToSection(writer.Position, 2, 1, 0, BntxFileWriter.Section1, "Texture Info"); //      <------------ Entry Set
            writer.Write(TexturePos + 0x90); //TexPtr
            writer.Write(TexturePos + 0x190); //TexView
            writer.Write(0L); //descSlotDataOffset
            writer.SaveRelocateEntryToSection(writer.Position, 1, 1, 0, BntxFileWriter.Section1, "User Data"); //      <------------ Entry Set
            PosUserDataDictOffset = writer.SaveOffset();// userDictOffset

            writer.Write(new byte[512]);

            writer.Align(8);
            var _ofsDataOffsets = writer.Position;
            writer.SaveRelocateEntryToSection(_ofsDataOffsets, (uint)TextureData[0].Count, 1, 0, BntxFileWriter.Section2, "TextureBlocks");

            foreach (var mipmap in MipOffsets)
            {
                writer.SaveMipMapOffsets();
            }

            if (UserData.Count > 0)
            {
                writer.SaveUserData(UserData, PosUserDataOffset);
            }
            if (UserData.Count > 0)
            {
                writer.SaveUserDataData(UserData);
                writer.Align(8);
                writer.WriteOffset(PosUserDataDictOffset);
                ((IResData)UserDataDict).Save(writer);
                writer.Align(8);
            }

            using (writer.TemporarySeek(PosDataOffsets, SeekOrigin.Begin))
            {
                writer.Write(_ofsDataOffsets);
            }
        }
    }
}
