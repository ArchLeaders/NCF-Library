namespace BntxLibrary.Shared.AddressLibrary
{
    internal class SurfaceOut
    {
        internal int Size { get; set; } = 0;
        internal int Pitch { get; set; } = 0;
        internal int Height { get; set; } = 0;
        internal int Depth { get; set; } = 0;
        internal int SurfSize { get; set; } = 0;
        internal int TileMode { get; set; } = 0;
        internal int BaseAlign { get; set; } = 0;
        internal int PitchAlign { get; set; } = 0;
        internal int HeightAlign { get; set; } = 0;
        internal int DepthAlign { get; set; } = 0;
        internal int Bpp { get; set; } = 0;
        internal int PixelPitch { get; set; } = 0;
        internal int PixelHeight { get; set; } = 0;
        internal int PixelBits { get; set; } = 0;
        internal int SliceSize { get; set; } = 0;
        internal int PitchTileMax { get; set; } = 0;
        internal int HeightTileMax { get; set; } = 0;
        internal int SliceTileMax { get; set; } = 0;
        internal TileInfo TileInfo { get; set; } = new();
        internal int TileType { get; set; } = 0;
        internal int TileIndex { get; set; } = 0;
    }
}
