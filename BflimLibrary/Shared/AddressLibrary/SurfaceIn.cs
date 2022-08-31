namespace BntxLibrary.Shared.AddressLibrary
{
    public class SurfaceIn
    {
        internal uint Size { get; set; } = 0;
        internal uint TileMode { get; set; } = 0;
        internal uint Format { get; set; } = 0;
        internal uint Bpp { get; set; } = 0;
        internal uint NumSamples { get; set; } = 0;
        internal uint Width { get; set; } = 0;
        internal uint Height { get; set; } = 0;
        internal uint NumSlices { get; set; } = 0;
        internal uint Slice { get; set; } = 0;
        internal int MipLevel { get; set; } = 0;
        internal Flags Flags { get; set; } = new();
        internal uint NumFrags { get; set; } = 0;
        internal int TileIndex { get; set; } = 0;
        internal TileInfo TileInfo { get; set; } = new();
    }
}
