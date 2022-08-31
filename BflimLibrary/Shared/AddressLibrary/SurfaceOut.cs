namespace BntxLibrary.Shared.AddressLibrary
{
    internal class SurfaceOut
    {
        internal uint Size { get; set; } = 0;
        internal uint Pitch { get; set; } = 0;
        internal uint Height { get; set; } = 0;
        internal uint Depth { get; set; } = 0;
        internal uint SurfSize { get; set; } = 0;
        internal uint TileMode { get; set; } = 0;
        internal uint BaseAlign { get; set; } = 0;
        internal uint PitchAlign { get; set; } = 0;
        internal uint HeightAlign { get; set; } = 0;
        internal uint DepthAlign { get; set; } = 0;
        internal uint Bpp { get; set; } = 0;
        internal uint PixelPitch { get; set; } = 0;
        internal uint PixelHeight { get; set; } = 0;
        internal uint PixelBits { get; set; } = 0;
        internal uint SliceSize { get; set; } = 0;
        internal uint PitchTileMax { get; set; } = 0;
        internal uint HeightTileMax { get; set; } = 0;
        internal uint SliceTileMax { get; set; } = 0;
        internal TileInfo TileInfo { get; set; } = new();
        internal uint TileType { get; set; } = 0;
        internal int TileIndex { get; set; } = 0;
    }
}
