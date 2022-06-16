﻿using System.Collections.Generic;
using System.IO;
using Syroot.BinaryData;
using Nintendo.Bfres.Core;
using Nintendo.Bfres.Helpers;
using Syroot.Maths;
using Syroot.BinaryData.Core;

namespace Nintendo.Bfres
{
    /// <summary>
    /// Represents a data buffer holding vertices for a <see cref="Model"/> subfile.
    /// </summary>
    public class VertexBuffer : IResData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class.
        /// </summary>
        public VertexBuffer()
        {
            VertexSkinCount = 0;

            Attributes = new ResDict<VertexAttrib>();
            Buffers = new List<Buffer>();
        }

        public void CreateEmptyVertexBuffer()
        {
            VertexBufferHelper helper = new VertexBufferHelper(new VertexBuffer(), Endian.Big);
            List<VertexBufferHelperAttrib> atrib = new List<VertexBufferHelperAttrib>();

            var positions = new Vector4F[4];
            positions[0] = new Vector4F(-1, 0, 1, 0);
            positions[1] = new Vector4F(-1, 0, 1, 0);
            positions[2] = new Vector4F(-1, 0, 1, 0);
            positions[3] = new Vector4F(-1, 0, 1, 0);
            var normals = new Vector4F[4];
            normals[0] = new Vector4F(0, -1, 0, 0);
            normals[1] = new Vector4F(0, -1, 0, 0);
            normals[2] = new Vector4F(0, -1, 0, 0);
            normals[3] = new Vector4F(0, -1, 0, 0);

            VertexBufferHelperAttrib position = new VertexBufferHelperAttrib();
            position.Name = "_p0";
            position.Data = positions;
            position.Format = GX2.GX2AttribFormat.Format_32_32_32_Single;
            atrib.Add(position);

            VertexBufferHelperAttrib normal = new VertexBufferHelperAttrib();
            normal.Name = "_n0";
            normal.Data = normals;
            normal.Format = GX2.GX2AttribFormat.Format_10_10_10_2_SNorm;
            atrib.Add(normal);

            helper.Attributes = atrib;
            var VertexBuffer = helper.ToVertexBuffer();
            VertexSkinCount = VertexBuffer.VertexSkinCount;
            Attributes = VertexBuffer.Attributes;
            Buffers = VertexBuffer.Buffers;
        }

        // ---- CONSTANTS ----------------------------------------------------------------------------------------------

        private const string _signature = "FVTX";
        
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the number of bones influencing the vertices stored in this buffer. 0 influences equal
        /// rigidbodies (no skinning), 1 equal rigid skinning and 2 or more smooth skinning.
        /// </summary>
        public byte VertexSkinCount { get; set; }

        /// <summary>
        /// Gets the number of vertices stored by the <see cref="Buffers"/>. It is calculated from the size of the first
        /// <see cref="Buffer"/> in bytes divided by the <see cref="Buffer.Stride"/>.
        /// </summary>
        public uint VertexCount
        {
            get
            {
                Buffer firstBuffer = Buffers[0];
                int dataSize = firstBuffer.Data[0].Length;
                
                // Throw an exception if the stride does not yield complete elements.
                if (dataSize % firstBuffer.Stride != 0)
                {
                    throw new InvalidDataException($"Stride of {firstBuffer} does not yield complete elements."); 
                }

                return (uint)(dataSize / firstBuffer.Stride);
            }
        }

        /// <summary>
        /// Gets or sets the dictionary of <see cref="VertexAttrib"/> instances describing how to interprete data in the
        /// <see cref="Buffers"/>.
        /// </summary>
        public ResDict<VertexAttrib> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Buffer"/> instances storing raw unformatted vertex data.
        /// </summary>
        public IList<Buffer> Buffers { get; set; }

        internal MemoryPool MemoryPool = new MemoryPool();

        internal uint Flags;

        // ---- METHODS ------------------------------------------------------------------------------------------------

        public VertexBuffer ShallowCopy()
        {
            return (VertexBuffer)this.MemberwiseClone();
        }

        void IResData.Load(ResFileLoader loader)
        {
            loader.CheckSignature(_signature);
            if (loader.IsSwitch) {
                Switch.VertexBufferParser.Load((Switch.Core.ResFileSwitchLoader)loader, this);
            }
            else
            {
                byte numVertexAttrib = (byte)loader.ReadByte();
                byte numBuffer = (byte)loader.ReadByte();
                ushort idx = loader.ReadUInt16();
                uint vertexCount = loader.ReadUInt32();
                VertexSkinCount = (byte)loader.ReadByte();
                loader.Seek(3);
                uint ofsVertexAttribList = loader.ReadOffset(); // Only load dict.
                Attributes = loader.LoadDict<VertexAttrib>();
                Buffers = loader.LoadList<Buffer>(numBuffer);
                uint userPointer = loader.ReadUInt32();
            }
        }

        void IResData.Save(ResFileSaver saver)
        {
            Position = saver.Position;
            saver.WriteSignature(_signature);
            if (saver.IsSwitch) {
                Switch.VertexBufferParser.Save((Switch.Core.ResFileSwitchSaver)saver, this);
            }
            else
            {
                saver.Write((byte)Attributes.Count);
                saver.Write((byte)Buffers.Count);
                saver.Write((ushort)saver.CurrentIndex);
                saver.Write(VertexCount);
                saver.Write(VertexSkinCount);
                saver.Seek(3);
                saver.SaveList(Attributes.Values);
                saver.SaveDict(Attributes);
                saver.SaveList(Buffers);
                saver.Write(0); // UserPointer
            }
        }

        public void UpdateVertexBufferEndian(Endian endian, Endian target)
        {
            //Swap existing byte orders
            VertexBufferHelper helper = new VertexBufferHelper(this, endian);
            helper.endian = target;

            var newBuffer = helper.ToVertexBuffer();
            this.Buffers = new List<Buffer>();
            for (int i = 0; i < newBuffer.Buffers.Count; i++)
                this.Buffers.Add(newBuffer.Buffers[i]);

            for (int i = 0; i < newBuffer.Attributes.Count; i++)
            {
                Attributes[i].Offset = newBuffer.Attributes[i].Offset;
                Attributes[i].BufferIndex = newBuffer.Attributes[i].BufferIndex;
                Attributes[i].Format = newBuffer.Attributes[i].Format;
                Attributes[i].Name = newBuffer.Attributes[i].Name;
            }
        }

        internal long AttributeOffset;
        internal long AttributeDictOffset;
        internal long UnkBufferOffset;
        internal long UnkBuffer2Offset;
        internal long BufferSizeArrayOffset;
        internal long StideArrayOffset;
        internal long Position;
    }
}
