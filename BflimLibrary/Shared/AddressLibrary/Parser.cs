namespace BntxLibrary.Shared.AddressLibrary
{
    internal class Parser
    {
        internal List<uint> BCnFormats = new()
        {
            0x31, 0x431, 0x32, 0x432,
            0x33, 0x433, 0x34, 0x234,
            0x35, 0x235,
        };
        internal List<uint> FormatHwInfo = new() {
            0x00, 0x00, 0x00, 0x01, 0x08, 0x03, 0x00, 0x01, 0x08, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x01, 0x10, 0x07, 0x00, 0x00, 0x10, 0x03, 0x00, 0x01, 0x10, 0x03, 0x00, 0x01,
            0x10, 0x0B, 0x00, 0x01, 0x10, 0x01, 0x00, 0x01, 0x10, 0x03, 0x00, 0x01, 0x10, 0x03, 0x00, 0x01,
            0x10, 0x03, 0x00, 0x01, 0x20, 0x03, 0x00, 0x00, 0x20, 0x07, 0x00, 0x00, 0x20, 0x03, 0x00, 0x00,
            0x20, 0x03, 0x00, 0x01, 0x20, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x03, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x01, 0x20, 0x0B, 0x00, 0x01, 0x20, 0x0B, 0x00, 0x01, 0x20, 0x0B, 0x00, 0x01,
            0x40, 0x05, 0x00, 0x00, 0x40, 0x03, 0x00, 0x00, 0x40, 0x03, 0x00, 0x00, 0x40, 0x03, 0x00, 0x00,
            0x40, 0x03, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x80, 0x03, 0x00, 0x00, 0x80, 0x03, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x10, 0x01, 0x00, 0x00,
            0x10, 0x01, 0x00, 0x00, 0x20, 0x01, 0x00, 0x00, 0x20, 0x01, 0x00, 0x00, 0x20, 0x01, 0x00, 0x00,
            0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x60, 0x01, 0x00, 0x00,
            0x60, 0x01, 0x00, 0x00, 0x40, 0x01, 0x00, 0x01, 0x80, 0x01, 0x00, 0x01, 0x80, 0x01, 0x00, 0x01,
            0x40, 0x01, 0x00, 0x01, 0x80, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        };
        internal List<uint> FormatExInfo = new() {
            0x00, 0x01, 0x01, 0x03, 0x08, 0x01, 0x01, 0x03, 0x08, 0x01, 0x01, 0x03, 0x08, 0x01, 0x01, 0x03,
            0x00, 0x01, 0x01, 0x03, 0x10, 0x01, 0x01, 0x03, 0x10, 0x01, 0x01, 0x03, 0x10, 0x01, 0x01, 0x03,
            0x10, 0x01, 0x01, 0x03, 0x10, 0x01, 0x01, 0x03, 0x10, 0x01, 0x01, 0x03, 0x10, 0x01, 0x01, 0x03,
            0x10, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03,
            0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03,
            0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03,
            0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03,
            0x40, 0x01, 0x01, 0x03, 0x40, 0x01, 0x01, 0x03, 0x40, 0x01, 0x01, 0x03, 0x40, 0x01, 0x01, 0x03,
            0x40, 0x01, 0x01, 0x03, 0x00, 0x01, 0x01, 0x03, 0x80, 0x01, 0x01, 0x03, 0x80, 0x01, 0x01, 0x03,
            0x00, 0x01, 0x01, 0x03, 0x01, 0x08, 0x01, 0x05, 0x01, 0x08, 0x01, 0x06, 0x10, 0x01, 0x01, 0x07,
            0x10, 0x01, 0x01, 0x08, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03, 0x20, 0x01, 0x01, 0x03,
            0x18, 0x03, 0x01, 0x04, 0x30, 0x03, 0x01, 0x04, 0x30, 0x03, 0x01, 0x04, 0x60, 0x03, 0x01, 0x04,
            0x60, 0x03, 0x01, 0x04, 0x40, 0x04, 0x04, 0x09, 0x80, 0x04, 0x04, 0x0A, 0x80, 0x04, 0x04, 0x0B,
            0x40, 0x04, 0x04, 0x0C, 0x40, 0x04, 0x04, 0x0D, 0x40, 0x04, 0x04, 0x0D, 0x40, 0x04, 0x04, 0x0D,
            0x00, 0x01, 0x01, 0x03, 0x00, 0x01, 0x01, 0x03, 0x00, 0x01, 0x01, 0x03, 0x00, 0x01, 0x01, 0x03,
            0x00, 0x01, 0x01, 0x03, 0x00, 0x01, 0x01, 0x03, 0x40, 0x01, 0x01, 0x03, 0x00, 0x01, 0x01, 0x03,
        };

        internal int expPitch = 0;
        internal int expHeight = 0;
        internal int expNumSlices = 0;

        internal SurfaceIn pIn = new();
        internal SurfaceOut pOut = new();

        internal dynamic? GetDefaultGX2TileMode(dynamic dim, dynamic width, dynamic height, dynamic depth, dynamic format_, dynamic aa, dynamic use)
        {
            throw new NotImplementedException();
        }

        internal dynamic? GX2TileModeToAddrTileMode(dynamic tileMode)
        {
            throw new NotImplementedException();
        }

        internal dynamic? SwizzleSurf(dynamic width, dynamic height, dynamic depth, dynamic format_, dynamic aa, dynamic use, dynamic tileMode, dynamic swizzle_, dynamic pitch, dynamic bitsPerPixel, dynamic slice, dynamic sample, dynamic data, dynamic dataSize, dynamic swizzle)
        {
            throw new NotImplementedException();
        }

        internal dynamic? Deswizzle(dynamic width, dynamic height, dynamic depth, dynamic format_, dynamic aa, dynamic use, dynamic tileMode, dynamic swizzle_, dynamic pitch, dynamic bpp, dynamic slice, dynamic sample, dynamic data)
        {
            throw new NotImplementedException();
        }

        internal dynamic? Swizzle(dynamic width, dynamic height, dynamic depth, dynamic format_, dynamic aa, dynamic use, dynamic tileMode, dynamic swizzle_, dynamic pitch, dynamic bpp, dynamic slice, dynamic sample, dynamic data)
        {
            throw new NotImplementedException();
        }

        internal dynamic? SurfaceGetBitsPerPixel(dynamic surfaceFormat)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceThickness(dynamic tileMode)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputePixelIndexWithinMicroTile(dynamic x, dynamic y, dynamic z, dynamic bpp, dynamic tileMode, dynamic isDepth)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputePipeFromCoordWoRotation(dynamic x, dynamic y)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeBankFromCoordWoRotation(dynamic x, dynamic y)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceRotationFromTileMode(dynamic tileMode)
        {
            throw new NotImplementedException();
        }

        internal dynamic? IsThickMacroTiled(dynamic tileMode)
        {
            throw new NotImplementedException();
        }

        internal dynamic? IsBankSwappedTileMode(dynamic tileMode)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeMacroTileAspectRatio(dynamic tileMode)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceBankSwappedWidth(dynamic tileMode, dynamic bpp, dynamic numSamples, dynamic pitch)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceAddrFromCoordLinear(dynamic x, dynamic y, dynamic slice, dynamic sample, dynamic bpp, dynamic pitch, dynamic height, dynamic numSlices)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceAddrFromCoordMicroTiled(dynamic x, dynamic y, dynamic slice, dynamic bpp, dynamic pitch, dynamic height, dynamic tileMode, dynamic isDepth)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceAddrFromCoordMacroTiled(dynamic x, dynamic y, dynamic slice, dynamic sample, dynamic bpp, dynamic pitch, dynamic height, dynamic numSamples, dynamic tileMode, dynamic isDepth, dynamic pipeSwizzle, dynamic bankSwizzle)
        {
            throw new NotImplementedException();
        }

        internal dynamic? PowTwoAlign(dynamic x, dynamic align)
        {
            throw new NotImplementedException();
        }

        internal dynamic? NextPow2(dynamic dim)
        {
            throw new NotImplementedException();
        }

        internal dynamic? GetBitsPerPixel(dynamic format_)
        {
            throw new NotImplementedException();
        }

        internal dynamic? AdjustSurfaceInfo(dynamic elemMode, dynamic expandX, dynamic expandY, dynamic bpp, dynamic width, dynamic height)
        {
            throw new NotImplementedException();
        }

        internal dynamic? HwlComputeMipLevel()
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeMipLevel()
        {
            throw new NotImplementedException();
        }

        internal dynamic? ConvertToNonBankSwappedMode(dynamic tileMode)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceTileSlices(dynamic tileMode, dynamic bpp, dynamic numSamples)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceMipLevelTileMode(dynamic baseTileMode, dynamic bpp, dynamic level, dynamic width, dynamic height, dynamic numSlices, dynamic numSamples, dynamic isDepth, dynamic noRecursive)
        {
            throw new NotImplementedException();
        }

        internal dynamic? PadDimensions(dynamic tileMode, dynamic padDims, dynamic isCube, dynamic pitchAlign, dynamic heightAlign, dynamic sliceAlign)
        {
            throw new NotImplementedException();
        }

        internal dynamic? AdjustPitchAlignment(dynamic flags, dynamic pitchAlign)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceAlignmentsLinear(dynamic tileMode, dynamic bpp, dynamic flags)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceInfoLinear(dynamic tileMode, dynamic bpp, dynamic numSamples, dynamic pitch, dynamic height, dynamic numSlices, dynamic mipLevel, dynamic padDims, dynamic flags)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceAlignmentsMicroTiled(dynamic tileMode, dynamic bpp, dynamic flags, dynamic numSamples)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceInfoMicroTiled(dynamic tileMode, dynamic bpp, dynamic numSamples, dynamic pitch, dynamic height, dynamic numSlices, dynamic mipLevel, dynamic padDims, dynamic flags)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceAlignmentsMacroTiled(dynamic tileMode, dynamic bpp, dynamic flags, dynamic numSamples)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceInfoMacroTiled(dynamic tileMode, dynamic baseTileMode, dynamic bpp, dynamic numSamples, dynamic pitch, dynamic height, dynamic numSlices, dynamic mipLevel, dynamic padDims, dynamic flags)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceInfoEx()
        {
            throw new NotImplementedException();
        }

        internal dynamic? RestoreSurfaceInfo(dynamic elemMode, dynamic expandX, dynamic expandY, dynamic bpp)
        {
            throw new NotImplementedException();
        }

        internal dynamic? ComputeSurfaceInfo(dynamic aSurfIn, dynamic pSurfOut)
        {
            throw new NotImplementedException();
        }

        internal dynamic? GetSurfaceInfo(dynamic surfaceFormat, dynamic surfaceWidth, dynamic surfaceHeight, dynamic surfaceDepth, dynamic surfaceDim, dynamic surfaceTileMode, dynamic surfaceAA, dynamic level)
        {
            throw new NotImplementedException();
        }
    }
}
