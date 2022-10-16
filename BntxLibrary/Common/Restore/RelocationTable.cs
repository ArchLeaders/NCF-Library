#pragma warning disable IDE0059 // Unnecessary assignment of a value

using BntxLibrary.Core;

namespace BntxLibrary.Common.Restore
{
    /// Has 5 sections which branch off to multiple entries
    /// If a section is unused it'll use the same offset and data from previous section
    /// First section stores all the data from start of file to the end string table.
    /// Index buffer gets stored in second section
    /// Vertex buffer gets stored in third section
    /// Memory Pool gets stored in fourth section
    /// All Embedded files stored in last section (points to RLT itself if not used)
    /// Entries sometimes will reference structs like the FMDL and will skip the block and magic data
    /// Entries sometimes will reference dicts aswell
    /// The way I am handling it is by booleans for ReadOffset, ReadDict, and Load methods. Those set to true will be relocated

    /// <summary>
    /// Represents an _RLT section in a <see cref="BntxFile"/> subfile, storing pointers to sections in a Bntx.
    /// </summary>  
    public class RelocationTable : IResData
    {
        private const string Signature = "_RLT";

        /// <summary>
        /// Gets or sets the <see cref="VertexBuffer"/> instance storing the data which forms the shape's surface. Saved
        /// depending on <see cref="VertexBufferIndex"/>.
        /// </summary>
        internal uint Position { get; set; }

        void IResData.Load(BntxFileLoader loader)
        {
            Position = (uint)loader.Position;

            loader.CheckSignature(Signature);
            int pos = loader.ReadInt32();
            int sectionCount = loader.ReadInt32();
            loader.Seek(4); // Padding
        }

        void IResData.Save(BntxFileWriter writer)
        {
            writer.WriteSignature(Signature);
            writer.Write(Position);
            writer.Write(5);
            writer.Write(0);
        }
    }
}
