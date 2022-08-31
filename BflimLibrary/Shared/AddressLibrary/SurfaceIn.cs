namespace BntxLibrary.Shared.AddressLibrary
{
    public class SurfaceIn
    {
        internal int Size { get; set; } = 0;
        internal int TileMode { get; set; } = 0;
        internal int Format { get; set; } = 0;
        internal int Bpp { get; set; } = 0;
        internal int NumSamples { get; set; } = 0;
        internal int Width { get; set; } = 0;
        internal int Height { get; set; } = 0;
        internal int NumSlices { get; set; } = 0;
        internal int Slice { get; set; } = 0;
        internal int MipLevel { get; set; } = 0;
        internal Flags Flags { get; set; } = new();
        internal int NumFrags { get; set; } = 0;
        internal int TileIndex { get; set; } = 0;
        internal TileInfo TileInfo { get; set; } = new();
    }
}
