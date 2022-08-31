using static BntxLibrary.Shared.Extensions;

namespace BntxLibrary.Shared.AddressLibrary
{
    internal class Parser
    {
        internal uint[] BCnFormats = new uint[]
        {
            0x31, 0x431, 0x32, 0x432,
            0x33, 0x433, 0x34, 0x234,
            0x35, 0x235,
        };
        internal List<byte> FormatHwInfo = new() {
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
        internal List<byte> FormatExInfo = new() {
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

        internal uint expPitch = 0;
        internal uint expHeight = 0;
        internal uint expNumSlices = 0;
        internal byte[] bankSwapOrder = new byte[] { 0, 1, 3, 2, 6, 7, 5, 4, 0, 0 };

        internal SurfaceIn pIn = new();
        internal SurfaceOut pOut = new();

        internal uint GetDefaultGX2TileMode(uint dim, uint width, uint height, uint depth, uint format, uint aa, uint use)
        {
            uint tileMode = 1;
            bool isDepthBuffer = Convert.ToBoolean(use & 4);
            bool isColorBuffer = Convert.ToBoolean(use & 2);

            if (dim.ToBool() || aa.ToBool() || isDepthBuffer)
            {
                if (dim != 2 || isColorBuffer)
                {
                    tileMode = 4;
                }
                else
                {
                    tileMode = 7;
                }

                var surfOut = GetSurfaceInfo(format, width, height, depth, dim, tileMode, aa, 0);
                if (width < surfOut.PitchAlign && height < surfOut.HeightAlign)
                {
                    if (tileMode == 7)
                    {
                        tileMode = 3;
                    }
                    else
                    {
                        tileMode = 2;
                    }
                }
            }

            return tileMode;
        }

        internal uint GX2TileModeToAddrTileMode(uint tileMode)
        {
            if (tileMode == 16)
            {
                return 16;
            }

            return tileMode;
        }

        internal byte[] SwizzleSurf(uint width, uint height, uint depth, uint format_, uint aa, uint use, uint tileMode, uint swizzle_, uint pitch, uint bitsPerPixel, uint slice, uint sample, byte[] data, bool swizzle)
        {
            uint bytesPerPixel = Floor(bitsPerPixel / 8);
            byte[] result = new byte[data.Length];
            if (BCnFormats.Contains(format_))
            {
                width = Floor((width + 3) / 4);
                height = Floor((height + 3) / 4);
            }

            uint pipeSwizzle = (swizzle_ >> 8) & 1;
            uint bankSwizzle = (swizzle_ >> 9) & 3;
            uint pos;

            tileMode = GX2TileModeToAddrTileMode(tileMode);

            for (uint y = 0; y < height; y++)
            {
                for (uint x = 0; x < width; x++)
                {
                    if (tileMode == 0 || tileMode == 1)
                    {
                        pos = (uint)ComputeSurfaceAddrFromCoordLinear(x, y, slice, sample, bytesPerPixel, pitch, height, depth);
                    }
                    else if (tileMode == 2 || tileMode == 3)
                    {
                        pos = (uint)ComputeSurfaceAddrFromCoordMicroTiled(x, y, slice, sample, bitsPerPixel, pitch, height, Convert.ToBoolean(use & 4));
                    }
                    else
                    {
                        pos = (uint)ComputeSurfaceAddrFromCoordMacroTiled(x, y, slice, sample, bitsPerPixel, pitch, height, (uint)(1 << (int)aa), tileMode, Convert.ToBoolean(use & 4), pipeSwizzle, bankSwizzle);
                    }

                    var _pos = (y * width + x) * bytesPerPixel;

                    if (_pos + bytesPerPixel <= data.Length && pos + bytesPerPixel <= data.Length)
                    {
                        if (swizzle)
                        {
                            result[(int)_pos..(int)(_pos + bytesPerPixel)].SetValue(data, pos, pos + bytesPerPixel);
                        }
                        else
                        {
                            result[(int)pos..(int)(pos + bytesPerPixel)].SetValue(data, _pos, _pos + bytesPerPixel);
                        }
                    }
                }
            }

            return result;
        }

        internal byte[] Deswizzle(uint width, uint height, uint depth, uint format_, uint aa, uint use, uint tileMode, uint swizzle_, uint pitch, uint bitsPerPixel, uint slice, uint sample, byte[] data)
        {
            return SwizzleSurf(width, height, depth, format_, aa, use, tileMode, swizzle_, pitch, bitsPerPixel, slice, sample, data, false);
        }

        internal byte[] Swizzle(uint width, uint height, uint depth, uint format_, uint aa, uint use, uint tileMode, uint swizzle_, uint pitch, uint bitsPerPixel, uint slice, uint sample, byte[] data)
        {
            return SwizzleSurf(width, height, depth, format_, aa, use, tileMode, swizzle_, pitch, bitsPerPixel, slice, sample, data, true);
        }

        internal uint SurfaceGetBitsPerPixel(uint surfaceFormat)
        {
            return FormatHwInfo[(int)(surfaceFormat & 0x3F) * 4];
        }

        internal uint ComputeSurfaceThickness(uint tileMode)
        {
            if (tileMode.In(3, 7, 11, 13, 15))
            {
                return 4;
            }
            else if (tileMode.In(16, 17))
            {
                return 8;
            }

            return 1;
        }

        internal uint ComputePixelIndexWithinMicroTile(uint x, uint y, uint z, uint bitsPerPixel, uint tileMode, bool isDepth)
        {
            uint pixelBit0, pixelBit1, pixelBit2;
            uint pixelBit3, pixelBit4, pixelBit5;
            uint pixelBit6 = 0;
            uint pixelBit7 = 0;
            uint pixelBit8 = 0;

            uint thickness = ComputeSurfaceThickness(tileMode);

            if (isDepth)
            {
                pixelBit0 = x & 1;
                pixelBit1 = y & 1;
                pixelBit2 = (x & 2) >> 1;
                pixelBit3 = (y & 2) >> 1;
                pixelBit4 = (x & 4) >> 2;
                pixelBit5 = (y & 4) >> 2;
            }
            else
            {
                if (bitsPerPixel == 8)
                {
                    pixelBit0 = x & 1;
                    pixelBit1 = (x & 2) >> 1;
                    pixelBit2 = (x & 4) >> 2;
                    pixelBit3 = (y & 2) >> 1;
                    pixelBit4 = y & 1;
                    pixelBit5 = (y & 4) >> 2;
                }

                else if (bitsPerPixel == 0x10)
                {
                    pixelBit0 = x & 1;
                    pixelBit1 = (x & 2) >> 1;
                    pixelBit2 = (x & 4) >> 2;
                    pixelBit3 = y & 1;
                    pixelBit4 = (y & 2) >> 1;
                    pixelBit5 = (y & 4) >> 2;
                }

                else if (bitsPerPixel == 0x20 || bitsPerPixel == 0x60)
                {
                    pixelBit0 = x & 1;
                    pixelBit1 = (x & 2) >> 1;
                    pixelBit2 = y & 1;
                    pixelBit3 = (x & 4) >> 2;
                    pixelBit4 = (y & 2) >> 1;
                    pixelBit5 = (y & 4) >> 2;
                }

                else if (bitsPerPixel == 0x40)
                {
                    pixelBit0 = x & 1;
                    pixelBit1 = y & 1;
                    pixelBit2 = (x & 2) >> 1;
                    pixelBit3 = (x & 4) >> 2;
                    pixelBit4 = (y & 2) >> 1;
                    pixelBit5 = (y & 4) >> 2;
                }

                else if (bitsPerPixel == 0x80)
                {
                    pixelBit0 = y & 1;
                    pixelBit1 = x & 1;
                    pixelBit2 = (x & 2) >> 1;
                    pixelBit3 = (x & 4) >> 2;
                    pixelBit4 = (y & 2) >> 1;
                    pixelBit5 = (y & 4) >> 2;
                }

                else
                {
                    pixelBit0 = x & 1;
                    pixelBit1 = (x & 2) >> 1;
                    pixelBit2 = y & 1;
                    pixelBit3 = (x & 4) >> 2;
                    pixelBit4 = (y & 2) >> 1;
                    pixelBit5 = (y & 4) >> 2;
                }
            }

            if (thickness > 1)
            {
                pixelBit6 = z & 1;
                pixelBit7 = (z & 2) >> 1;
            }

            if (thickness == 8)
            {
                pixelBit8 = (z & 4) >> 2;
            }

            return (pixelBit8 << 8) | (pixelBit7 << 7) | (pixelBit6 << 6) | 32 * pixelBit5 | 16 * pixelBit4 | 8 * pixelBit3 | 4 * pixelBit2 | pixelBit0 | 2 * pixelBit1;
        }

        internal uint ComputePipeFromCoordWoRotation(uint x, uint y)
        {
            return ((y >> 3) ^ (x >> 3)) & 1;
        }

        internal uint ComputeBankFromCoordWoRotation(uint x, uint y)
        {
            return ((y >> 5) ^ (x >> 3)) & 1 | 2 * (((y >> 4) ^ (x >> 4)) & 1);
        }

        internal uint ComputeSurfaceRotationFromTileMode(uint tileMode)
        {
            if (tileMode.In(4, 5, 6, 7, 8, 9, 10, 11))
            {
                return 2;
            }
            else if (tileMode.In(12, 13, 14, 15))
            {
                return 1;
            }

            return 0;
        }

        internal bool IsThickMacroTiled(uint tileMode)
        {
            return tileMode.In(7, 11, 13, 15);
        }

        internal bool IsBankSwappedTileMode(uint tileMode)
        {
            return tileMode.In(8, 9, 10, 11, 14, 15);
        }

        internal uint ComputeMacroTileAspectRatio(uint tileMode)
        {
            var _2 = new uint[] { 5, 9 };
            var _4 = new uint[] { 6, 10 };

            if (tileMode.In(5, 9))
            {
                return 2;
            }
            else if (tileMode.In(6, 10))
            {
                return 4;
            }

            return 1;
        }

        internal uint ComputeSurfaceBankSwappedWidth(uint tileMode, uint bitsPerPixel, uint numSamples, uint pitch)
        {
            if (!IsBankSwappedTileMode(tileMode))
            {
                return 0;
            }

            uint slicesPerTile;
            uint bytesPerSample = 8 * bitsPerPixel;

            if (bytesPerSample != 0)
            {
                uint samplesPerTile = Floor(2048 / bytesPerSample);
                slicesPerTile = Math.Max(samplesPerTile, Floor(numSamples / samplesPerTile));
            }
            else
            {
                slicesPerTile = 1;
            }

            if (IsThickMacroTiled(tileMode))
            {
                numSamples = 4;
            }

            uint bytesPerTileSlice = numSamples * Floor(bytesPerSample / slicesPerTile);

            uint factor = ComputeMacroTileAspectRatio(tileMode);
            uint swapTiles = Math.Max(1, Floor(128 / bitsPerPixel));

            uint swapWidth = swapTiles * 32;
            uint heightBytes = numSamples * factor * bitsPerPixel * Floor(2 / slicesPerTile);
            uint swapMax = Floor(0x4000 / heightBytes);
            uint swapMin = Floor(256 / bytesPerTileSlice);

            uint bankSwapWidth = Math.Min(swapMax, Math.Max(swapMin, swapWidth));

            while (bankSwapWidth >= 2 * pitch)
            {
                bankSwapWidth >>= 1;
            }

            return bankSwapWidth;
        }

        internal ulong ComputeSurfaceAddrFromCoordLinear(uint x, uint y, uint slice, uint sample, uint bytesPerPixel, uint pitch, uint height, uint numSlices)
        {
            ulong sliceOffset = pitch * height * (slice + sample * numSlices);
            return (y * pitch + x + sliceOffset) * bytesPerPixel;
        }

        internal ulong ComputeSurfaceAddrFromCoordMicroTiled(uint x, uint y, uint slice, uint bitsPerPixel, uint pitch, uint height, uint tileMode, bool isDepth)
        {
            ulong microTileThickness = (ulong)(tileMode == 3 ? 4 : 1);
            ulong microTileBytes = Floor((64 * microTileThickness * bitsPerPixel + 7) / 8);
            ulong microTilesPerRow = pitch >> 3;
            ulong microTileIndexX = x >> 3;
            ulong microTileIndexY = y >> 3;
            ulong microTileIndexZ = Floor(slice / microTileThickness);
            ulong microTileOffset = microTileBytes * (microTileIndexX + microTileIndexY * microTilesPerRow);

            ulong sliceBytes = Floor((pitch * height * microTileThickness * bitsPerPixel + 7) / 8);
            ulong sliceOffset = microTileIndexZ * sliceBytes;

            ulong pixelIndex = ComputePixelIndexWithinMicroTile(x, y, slice, bitsPerPixel, tileMode, isDepth);
            ulong pixelOffset = (bitsPerPixel * pixelIndex) >> 3;

            return pixelOffset + microTileOffset + sliceOffset;
        }

        internal ulong ComputeSurfaceAddrFromCoordMacroTiled(uint x, uint y, uint slice, uint sample, uint bitsPerPixel, uint pitch, uint height, uint numSamples, uint tileMode, bool isDepth, uint pipeSwizzle, uint bankSwizzle)
        {
            ulong microTileThickness = ComputeSurfaceThickness(tileMode);

            ulong microTileBits = numSamples * bitsPerPixel * (microTileThickness * 64);
            ulong microTileBytes = Floor((microTileBits + 7) / 8);
            ulong bytesPerSample = Floor(microTileBytes / numSamples);

            ulong pixelIndex = ComputePixelIndexWithinMicroTile(x, y, slice, bitsPerPixel, tileMode, isDepth);

            ulong sampleOffset, pixelOffset;

            if (isDepth)
            {
                sampleOffset = bitsPerPixel * sample;
                pixelOffset = bitsPerPixel * pixelIndex;
            }
            else
            {
                sampleOffset = sample * Floor(microTileBits / numSamples);
                pixelOffset = bitsPerPixel * pixelIndex;
            }

            ulong elemOffset = pixelOffset + sampleOffset;
            ulong samplesPerSlice, numSampleSplits, sampleSlice;

            if (numSamples <= 1 || microTileBytes <= 2048)
            {
                samplesPerSlice = numSamples;
                numSampleSplits = 1;
                sampleSlice = 0;
            }
            else
            {
                samplesPerSlice = Floor(2048 / bytesPerSample);
                numSampleSplits = Floor(numSamples / samplesPerSlice);
                numSamples = (uint)samplesPerSlice;

                ulong tileSliceBits = Floor(microTileBits / numSampleSplits);
                sampleSlice = Floor(elemOffset / tileSliceBits);
                elemOffset %= tileSliceBits;
            }

            elemOffset = Floor((elemOffset + 7) / 8);

            ulong pipe = ComputePipeFromCoordWoRotation(x, y);
            ulong bank = ComputeBankFromCoordWoRotation(x, y);

            ulong swizzle_ = pipeSwizzle + 2 * bankSwizzle;
            ulong bankPipe = pipe + 2 * bank;
            ulong rotation = ComputeSurfaceRotationFromTileMode(tileMode);
            ulong sliceIn = slice;

            if (IsThickMacroTiled(tileMode))
            {
                sliceIn >>= 2;
            }

            bankPipe ^= 2 * sampleSlice * 3 ^ (swizzle_ + sliceIn * rotation);
            bankPipe %= 8;

            pipe = bankPipe % 2;
            bank = Floor(bankPipe / 2);

            ulong sliceBytes = Floor((height * pitch * microTileThickness * bitsPerPixel * numSamples + 7) / 8);
            ulong sliceOffset = sliceBytes * Floor(((sampleSlice + numSampleSplits * slice) / microTileThickness));
            ulong macroTilePitch = 32;
            ulong macroTileHeight = 16;

            if (tileMode == 5 || tileMode == 9)
            {
                macroTilePitch = 16;
                macroTileHeight = 32;
            }
            else if (tileMode == 6 || tileMode == 10)
            {
                macroTilePitch = 8;
                macroTileHeight = 64;
            }

            ulong macroTilesPerRow = Floor(pitch / macroTilePitch);
            ulong macroTileBytes = Floor((numSamples * microTileThickness * bitsPerPixel * macroTileHeight * macroTilePitch + 7) / 8);
            ulong macroTileIndexX = Floor(x / macroTilePitch);
            ulong macroTileIndexY = Floor(y / macroTileHeight);
            ulong macroTileOffset = (macroTileIndexX + macroTilesPerRow * macroTileIndexY) * macroTileBytes;

            if (IsBankSwappedTileMode(tileMode))
            {
                ulong bankSwapWidth = ComputeSurfaceBankSwappedWidth(tileMode, bitsPerPixel, numSamples, pitch);
                ulong swapIndex = macroTilePitch * Floor(macroTileIndexX / bankSwapWidth);
                bank ^= bankSwapOrder[swapIndex & 3];
            }

            ulong totalOffset = elemOffset + ((macroTileOffset + sliceOffset) >> 3);
            return bank << 9 | pipe << 8 | totalOffset & 255 | (ulong)((int)totalOffset & -256) << 3;
        }

        internal uint PowTwoAlign(uint x, uint align)
        {
            return ~(align - 1) & (x + align - 1);
        }

        internal uint NextPow2(uint dim)
        {
            uint newDim = 1;
            if (dim <= 0x7FFFFFFF)
            {
                while (newDim < dim)
                {
                    newDim *= 2;
                }
            }
            else
            {
                newDim = 0x80000000;
            }

            return newDim;
        }

        internal (uint, uint, uint, uint) GetBitsPerPixel(uint format_)
        {
            int fmtIdx = (int)format_ * 4;
            return (FormatExInfo[fmtIdx], FormatExInfo[fmtIdx + 1], FormatExInfo[fmtIdx + 2], FormatExInfo[fmtIdx + 3]);
        }

        internal uint AdjustSurfaceInfo(uint elemMode, uint expandX, uint expandY, uint bitsPerPixel, uint width, uint height)
        {
            uint BCNFormat = (uint)(bitsPerPixel.ToBool() && elemMode.In(9, 10, 11, 12, 13) ? 1 : 0);

            if (width.ToBool() && height.ToBool())
            {
                if (expandX > 1 || expandY > 1)
                {
                    uint _width, _height;

                    if (elemMode == 4)
                    {
                        _width = expandX * width;
                        _height = expandY * height;
                    }
                    else if (BCNFormat.ToBool())
                    {
                        _width = Floor(width / expandX);
                        _height = Floor(height / expandY);
                    }
                    else
                    {
                        _width = Floor((width + expandX - 1) / expandX);
                        _height = Floor((height + expandY - 1) / expandY);
                    }

                    pIn.Width = Math.Max(1, _width);
                    pIn.Height = Math.Max(1, _height);
                }
            }

            if (bitsPerPixel.ToBool())
            {
                if (elemMode == 4)
                {
                    pIn.Bpp = Floor(Floor(bitsPerPixel / expandX) / expandY);
                }
                else if (elemMode.In(5, 6))
                {
                    pIn.Bpp = expandY * expandX * bitsPerPixel;
                }
                else if (elemMode.In(7, 8))
                {
                    pIn.Bpp = bitsPerPixel;
                }
                else if (elemMode.In(10, 11, 13))
                {
                    pIn.Bpp = 128;
                }
                else
                {
                    pIn.Bpp = bitsPerPixel;
                }

                return pIn.Bpp;
            }

            return 0;
        }

        internal bool HwlComputeMipLevel()
        {
            bool handled = false;
            if (49 <= pIn.Format && pIn.Format <= 55)
            {
                if (pIn.MipLevel.ToBool())
                {
                    uint width = pIn.Width;
                    uint height = pIn.Height;
                    uint slices = pIn.NumSlices;

                    if (CheckInt(pIn.Flags.Value >> 12, 1))
                    {
                        uint _width = width >> pIn.MipLevel;
                        uint _height = height >> pIn.MipLevel;

                        if (!CheckInt(pIn.Flags.Value >> 4, 1))
                        {
                            slices >>= pIn.MipLevel;
                        }

                        width = Math.Max(1, _width);
                        height = Math.Max(1, _height);
                        slices = Math.Max(1, slices);
                    }

                    pIn.Width = NextPow2(width);
                    pIn.Height = NextPow2(height);
                    pIn.NumSlices = slices;
                }

                handled = true;
            }

            return handled;
        }

        internal void ComputeMipLevel()
        {
            uint slices, height, width = 0;
            bool handled = false;

            if (49 <= pIn.Format && pIn.Format <= 55 && (!(pIn.MipLevel.ToBool()) || ((pIn.Flags.Value) >> 12).ToBool()))
            {
                pIn.Width = PowTwoAlign(pIn.Width, 4);
                pIn.Height = PowTwoAlign(pIn.Height, 4);
            }

            handled = HwlComputeMipLevel();
            if (!handled && pIn.MipLevel.ToBool() && CheckInt(pIn.Flags.Value >> 12, 1))
            {
                width = Math.Max(1, pIn.Width >> pIn.MipLevel);
                height = Math.Max(1, pIn.Height >> pIn.MipLevel);
                slices = Math.Max(1, pIn.NumSlices);

                if (!CheckInt(pIn.Flags.Value >> 4, 1))
                {
                    slices = Math.Max(1, slices >> pIn.MipLevel);
                }

                if (pIn.Format.In(47, 48))
                {
                    width = NextPow2(width);
                    height = NextPow2(height);
                    slices = NextPow2(slices);
                }

                pIn.Width = width;
                pIn.Height = height;
                pIn.NumSlices = slices;
            }
        }

        internal uint ConvertToNonBankSwappedMode(uint tileMode)
        {
            if (tileMode.In(8, 9, 10, 11, 14, 15))
            {
                return tileMode - 2;
            }

            return tileMode;
        }

        internal uint ComputeSurfaceTileSlices(uint tileMode, uint bitsPerPixel, uint numSamples)
        {
            uint bytePerSample = ((bitsPerPixel << 6) + 7) >> 3;
            uint tileSlices = 1;
            uint samplePerTile;

            if (ComputeSurfaceThickness(tileMode) > 1)
            {
                numSamples = 4;
            }

            if (bytePerSample.ToBool())
            {
                samplePerTile = Floor(2048 / bytePerSample);
                if (samplePerTile < numSamples)
                {
                    tileSlices = Math.Max(1, Floor(numSamples / samplePerTile));
                }
            }

            return tileSlices;
        }

        internal uint ComputeSurfaceMipLevelTileMode(uint baseTileMode, uint bitsPerPixel, int mipLevel, uint width, uint height, uint numSlices, uint numSamples, bool isDepth, bool noRecursive)
        {
            uint widthAlignFactor = 1;
            uint macroTileWidth = 32;
            uint macroTileHeight = 16;
            uint tileSlices = ComputeSurfaceTileSlices(baseTileMode, bitsPerPixel, numSamples);
            uint expTileMode = baseTileMode;

            uint _width, _height, _numSlices, thickness, microTileBytes;

            if (numSamples > 1 || tileSlices > 1 || isDepth)
            {
                if (baseTileMode == 7)
                {
                    expTileMode = 4;
                }
                else if (baseTileMode == 13)
                {
                    expTileMode = 12;
                }
                else if (baseTileMode == 11)
                {
                    expTileMode = 8;
                }
                else if (baseTileMode == 15)
                {
                    expTileMode = 14;
                }
            }

            if (baseTileMode == 2 && numSamples > 1)
            {
                expTileMode = 14;
            }
            else if (baseTileMode == 3)
            {
                if (numSamples > 1 || isDepth)
                {
                    expTileMode = 2;
                }

                if (numSamples.In(2, 4))
                {
                    expTileMode = 7;
                }
            }

            if (noRecursive || !(mipLevel.ToBool()))
            {
                return expTileMode;
            }

            if (bitsPerPixel.In(24, 48, 96))
            {
                bitsPerPixel = Floor(bitsPerPixel / 3);
            }

            _width = NextPow2(width);
            _height = NextPow2(height);
            _numSlices = NextPow2(numSlices);

            expTileMode = ConvertToNonBankSwappedMode(expTileMode);
            thickness = ComputeSurfaceThickness(expTileMode);
            microTileBytes = (numSamples * bitsPerPixel * (thickness << 6)) >> 3;

            if (microTileBytes < 256)
            {
                widthAlignFactor = Math.Max(1, Floor(256 / microTileBytes));
            }

            if (expHeight.In(4, 12))
            {
                if ((_width < widthAlignFactor * macroTileWidth) || _height < macroTileHeight)
                {
                    expTileMode = 2;
                }
            }
            else if (expTileMode == 5)
            {
                macroTileWidth = 16;
                macroTileHeight = 32;

                if ((_width < widthAlignFactor * macroTileWidth) || _height < macroTileHeight)
                {
                    expTileMode = 2;
                }
            }
            else if (expTileMode == 6)
            {
                macroTileWidth = 8;
                macroTileHeight = 64;

                if ((_width < widthAlignFactor * macroTileWidth) || _height < macroTileHeight)
                {
                    expTileMode = 2;
                }
            }

            if (expTileMode.In(7, 13))
            {
                if ((_width < widthAlignFactor * macroTileWidth) || _height < macroTileHeight)
                {
                    expTileMode = 3;
                }
            }

            if (_numSlices < 4)
            {
                if (expTileMode == 3)
                {
                    expTileMode = 2;
                }
                else if (expTileMode == 7)
                {
                    expTileMode = 4;
                }
                else if (expTileMode == 13)
                {
                    expTileMode = 12;
                }
            }

            return ComputeSurfaceMipLevelTileMode(expTileMode, bitsPerPixel, mipLevel, _width, _height, _numSlices, numSamples, isDepth, true);
        }

        internal (uint, uint, uint) PadDimensions(uint tileMode, uint padDims, bool isCube, uint pitchAlign, uint heightAlign, uint sliceAlign)
        {
            uint thickness = ComputeSurfaceThickness(tileMode);
            if (!padDims.ToBool())
            {
                padDims = 3;
            }

            if (!((pitchAlign & (pitchAlign - 1)).ToBool()))
            {
                expPitch = PowTwoAlign(expPitch, pitchAlign);
            }
            else
            {
                expPitch += pitchAlign - 1;
                expPitch = Floor(expPitch / pitchAlign);
                expPitch *= pitchAlign;
            }

            if (padDims > 1)
            {
                expHeight = PowTwoAlign(expHeight, heightAlign);
            }

            if (padDims > 2 || thickness > 1)
            {
                if (isCube)
                {
                    expNumSlices = NextPow2(expNumSlices);
                }

                if (thickness > 1)
                {
                    expNumSlices = PowTwoAlign(expNumSlices, sliceAlign);
                }
            }

            return (expPitch, expHeight, expNumSlices);
        }

        internal uint AdjustPitchAlignment(Flags flags, uint pitchAlign)
        {
            if (CheckInt(pIn.Flags.Value >> 13, 1))
            {
                pitchAlign = PowTwoAlign(pitchAlign, 0x20);
            }

            return pitchAlign;
        }

        internal (uint, uint, uint) ComputeSurfaceAlignmentsLinear(uint tileMode, uint bitsPerPixel, Flags flags)
        {
            uint baseAlign, pitchAlign, heightAlign;

            if (!(tileMode.ToBool()))
            {
                baseAlign = 1;
                pitchAlign = (uint)(bitsPerPixel != 1 ? 1 : 8);
                heightAlign = 1;
            }
            else if (tileMode == 1)
            {
                uint pixelsPerPipeInterleave = Floor(2048 / bitsPerPixel);
                baseAlign = 256;
                pitchAlign = Math.Max(0x40, pixelsPerPipeInterleave);
                heightAlign = 1;
            }
            else
            {
                baseAlign = 1;
                pitchAlign = 1;
                heightAlign = 1;
            }

            pitchAlign = AdjustPitchAlignment(flags, pitchAlign);
            return (baseAlign, pitchAlign, heightAlign);
        }

        internal bool ComputeSurfaceInfoLinear(uint tileMode, uint bitsPerPixel, uint numSamples, uint pitch, uint height, uint numSlices, int mipLevel, uint padDims, Flags flags)
        {
            expPitch = pitch;
            expHeight = height;
            expNumSlices = numSlices;

            uint thickness = ComputeSurfaceThickness(tileMode);

            (uint baseAlign, uint pitchAlign, uint heightAlign) = ComputeSurfaceAlignmentsLinear(tileMode, bitsPerPixel, flags);

            if (CheckInt(pIn.Flags.Value >> 9, 1) && !(mipLevel.ToBool()))
            {
                expPitch = Floor(expPitch / 3);
                expPitch = NextPow2(expPitch);
            }

            if (mipLevel.ToBool())
            {
                expPitch = NextPow2(expPitch);
                expHeight = NextPow2(expHeight);

                if (CheckInt(pIn.Flags.Value >> 4, 1))
                {
                    expNumSlices = numSlices;

                    if (numSlices <= 1)
                    {
                        padDims = 2;
                    }
                    else
                    {
                        padDims = 0;
                    }
                }
                else
                {
                    expNumSlices = NextPow2(numSlices);
                }
            }

            (expPitch, expHeight, expNumSlices) = PadDimensions(tileMode, padDims, CheckInt(pIn.Flags.Value >> 9, 1), pitchAlign, heightAlign, thickness);

            if (CheckInt(pIn.Flags.Value >> 6, 1) && !(mipLevel.ToBool()))
            {
                expPitch *= 3;
            }

            uint slices = expNumSlices * Floor(numSamples / thickness);
            pOut.Pitch = expPitch;
            pOut.Height = expHeight;
            pOut.Depth = expNumSlices;
            pOut.SurfSize = Floor((expHeight * expPitch * slices * bitsPerPixel * numSamples + 7) / 8);
            pOut.BaseAlign = baseAlign;
            pOut.PitchAlign = pitchAlign;
            pOut.HeightAlign = heightAlign;
            pOut.DepthAlign = thickness;

            return true;
        }

        internal (uint, uint, uint) ComputeSurfaceAlignmentsMicroTiled(uint tileMode, uint bitsPerPixel, Flags flags, uint numSamples)
        {
            if (bitsPerPixel.In(24, 48, 96))
            {
                bitsPerPixel = Floor(bitsPerPixel / 3);
            }

            uint thicknes = ComputeSurfaceThickness(tileMode);
            uint baseAlign = 256;
            uint pitchAlign = Math.Max(8, Floor(Floor(Floor(256 / bitsPerPixel) / numSamples) / thicknes));
            uint heightAlign = 8;

            pitchAlign = AdjustPitchAlignment(flags, pitchAlign);
            return (baseAlign, pitchAlign, heightAlign);
        }

        internal bool ComputeSurfaceInfoMicroTiled(uint tileMode, uint bitsPerPixel, uint numSamples, uint pitch, uint height, uint numSlices, int mipLevel, uint padDims, Flags flags)
        {
            uint expTileMode = tileMode;
            expPitch = pitch;
            expHeight = height;
            expNumSlices = numSlices;

            uint thickness = ComputeSurfaceThickness(tileMode);

            if (mipLevel != 0)
            {
                expPitch = NextPow2(pitch);
                expHeight = NextPow2(height);

                if (CheckInt(flags.Value >> 4, 1))
                {
                    expNumSlices = numSlices;

                    if (numSlices <= 1)
                    {
                        padDims = 2;
                    }
                    else
                    {
                        padDims = 0;
                    }
                }
                else
                {
                    expNumSlices = NextPow2(numSlices);
                }

                if (expTileMode == 3 && expNumSlices < 4)
                {
                    expTileMode = 2;
                    thickness = 1;
                }
            }

            (uint baseAlign, uint pitchAlign, uint heightAlign) = ComputeSurfaceAlignmentsMicroTiled(expTileMode, bitsPerPixel, flags, numSamples);
            (expPitch, expHeight, expNumSlices) = PadDimensions(expTileMode, padDims, CheckInt(flags.Value >> 4, 1), pitchAlign, heightAlign, thickness);

            pOut.Pitch = expPitch;
            pOut.Height = expHeight;
            pOut.Depth = expNumSlices;
            pOut.SurfSize = Floor((expHeight * expPitch * expNumSlices * bitsPerPixel * numSamples + 7) / 8);
            pOut.TileMode = expTileMode;
            pOut.BaseAlign = baseAlign;
            pOut.PitchAlign = pitchAlign;
            pOut.HeightAlign = heightAlign;
            pOut.DepthAlign = thickness;

            return true;
        }

        internal (uint, uint, uint, uint, uint) ComputeSurfaceAlignmentsMacroTiled(uint tileMode, uint bitsPerPixel, Flags flags, uint numSamples)
        {
            uint aspectRatio = ComputeMacroTileAspectRatio(tileMode);
            uint thickness = ComputeSurfaceThickness(tileMode);

            if (bitsPerPixel.In(24, 48, 96))
            {
                bitsPerPixel = Floor(bitsPerPixel / 3);
            }

            if (bitsPerPixel == 3)
            {
                bitsPerPixel = 1;
            }

            uint macroTileWidth = Floor(32 / aspectRatio);
            uint macroTileHeight = aspectRatio * 16;

            uint pitchAlign = Math.Max(macroTileWidth, macroTileWidth * Floor(Floor(Floor(256 / bitsPerPixel) / (8 * thickness)) / numSamples));
            pitchAlign = AdjustPitchAlignment(flags, pitchAlign);

            uint baseAlign;
            uint heightAlign = macroTileHeight;
            uint macroTileBytes = numSamples * ((bitsPerPixel * macroTileHeight * macroTileWidth + 7) >> 3);

            if (thickness == 1)
            {
                baseAlign = Math.Max(macroTileBytes, (numSamples * heightAlign * bitsPerPixel * pitchAlign + 7) >> 3);
            }
            else
            {
                baseAlign = Math.Max(256, (4 * heightAlign * bitsPerPixel * pitchAlign + 7) >> 3);
            }

            uint microTileBytes = (thickness * numSamples * (bitsPerPixel << 6) + 7) >> 3;
            uint numSlicesPerMicroTile = microTileBytes < 2048 ? 1 : Floor(microTileBytes / 2048);
            baseAlign = Floor(baseAlign / numSlicesPerMicroTile);

            return (baseAlign, pitchAlign, heightAlign, macroTileWidth, macroTileHeight);
        }

        internal bool ComputeSurfaceInfoMacroTiled(uint tileMode, uint baseTileMode, uint bitsPerPixel, uint numSamples, uint pitch, uint height, uint numSlices, int mipLevel, uint padDims, Flags flags)
        {
            expPitch = pitch;
            expHeight = height;
            expNumSlices = numSlices;

            bool result;
            uint expTileMode = tileMode;
            uint thickness = ComputeSurfaceThickness(tileMode);

            if (mipLevel != 0)
            {
                expPitch = NextPow2(pitch);
                expHeight = NextPow2(height);

                if (CheckInt(flags.Value >> 4, 1))
                {
                    expNumSlices = numSlices;
                    padDims = (uint)(numSlices <= 1 ? 2 : 0);
                }
                else
                {
                    expNumSlices = NextPow2(numSlices);
                }

                if (expTileMode == 7 && expNumSlices < 4)
                {
                    expTileMode = 4;
                    thickness = 1;
                }
            }

            if (tileMode == baseTileMode || mipLevel == 0 || !IsThickMacroTiled(baseTileMode) || IsThickMacroTiled(tileMode))
            {
                (uint baseAlign, uint pitchAlign, uint heightAlign, uint _macroWidth, uint _macroHeight) = ComputeSurfaceAlignmentsMacroTiled(tileMode, bitsPerPixel, flags, numSamples);
                uint bankSwappedWidth = ComputeSurfaceBankSwappedWidth(tileMode, bitsPerPixel, numSamples, pitch);

                if (bankSwappedWidth > pitchAlign)
                {
                    pitch = bankSwappedWidth;
                }

                (expPitch, expHeight, expNumSlices) = PadDimensions(tileMode, padDims, CheckInt(flags.Value >> 4, 1), pitchAlign, heightAlign, thickness);

                pOut.Pitch = expPitch;
                pOut.Height = expHeight;
                pOut.Depth = expNumSlices;
                pOut.SurfSize = Floor((expHeight * expPitch * expNumSlices * bitsPerPixel * numSamples + 7) / 8);
                pOut.TileMode = expTileMode;
                pOut.BaseAlign = baseAlign;
                pOut.PitchAlign = pitchAlign;
                pOut.HeightAlign = heightAlign;
                pOut.DepthAlign = thickness;
                result = true;
            }
            else
            {
                (uint baseAlign, uint pitchAlign, uint heightAlign, uint _macroWidth, uint _macroHeight) = ComputeSurfaceAlignmentsMacroTiled(baseTileMode, bitsPerPixel, flags, numSamples);
                uint pitchAlignFactor = Math.Max(1, Floor(32 / bitsPerPixel));

                if (expPitch < pitchAlign * pitchAlignFactor || expHeight < heightAlign)
                {
                    result = ComputeSurfaceInfoMicroTiled(2, bitsPerPixel, numSamples, pitch, height, numSlices, mipLevel, padDims, flags);
                }
                else
                {
                    (baseAlign, pitchAlign, heightAlign, _macroWidth, _macroHeight) = ComputeSurfaceAlignmentsMacroTiled(tileMode, bitsPerPixel, flags, numSamples);
                    uint bankSwappedWidth = ComputeSurfaceBankSwappedWidth(tileMode, bitsPerPixel, numSamples, pitch);

                    if (bankSwappedWidth > pitchAlign)
                    {
                        pitchAlign = bankSwappedWidth;
                    }

                    (expPitch, expHeight, expNumSlices) = PadDimensions(tileMode, padDims, CheckInt(flags.Value >> 4, 1), pitchAlign, heightAlign, thickness);

                    pOut.Pitch = expPitch;
                    pOut.Height = expHeight;
                    pOut.Depth = expNumSlices;
                    pOut.SurfSize = Floor((expHeight * expPitch * expNumSlices * bitsPerPixel * numSamples + 7) / 8);
                    pOut.TileMode = expTileMode;
                    pOut.BaseAlign = baseAlign;
                    pOut.PitchAlign = pitchAlign;
                    pOut.HeightAlign = heightAlign;
                    pOut.DepthAlign = thickness;
                    result = true;
                }
            }

            return result;
        }

        internal uint ComputeSurfaceInfoEx()
        {
            bool result = false;

            uint tileMode = pIn.TileMode;
            uint bitsPerPixel = pIn.Bpp;
            uint numSamples = Math.Max(1, pIn.NumSamples);
            uint pitch = pIn.Width;
            uint height = pIn.Height;
            uint numSlices = pIn.NumSlices;
            int mipLevel = pIn.MipLevel;
            Flags flags = new() { Value = pIn.Flags.Value };
            uint padDims = 0;
            uint baseTileMode = tileMode;

            if (CheckInt(flags.Value >> 4, 1) && mipLevel == 0)
            {
                padDims = 2;
            }

            if (CheckInt(flags.Value >> 6, 1))
            {
                tileMode = ConvertToNonBankSwappedMode(tileMode);
            }
            else
            {
                tileMode = ComputeSurfaceMipLevelTileMode(tileMode, bitsPerPixel, mipLevel, pitch, height, numSlices, numSamples, CheckInt(flags.Value >> 1, 1), false);
            }

            if (tileMode.In(0, 1))
            {
                result = ComputeSurfaceInfoLinear(tileMode, bitsPerPixel, numSamples, pitch, height, numSlices, mipLevel, padDims, flags);
                pOut.TileMode = tileMode;
            }
            else if (tileMode.In(2, 3))
            {
                result = ComputeSurfaceInfoMicroTiled(tileMode, bitsPerPixel, numSamples, pitch, height, numSlices, mipLevel, padDims, flags);
            }
            else if (tileMode.In(4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15))
            {
                result = ComputeSurfaceInfoMacroTiled(tileMode, baseTileMode, bitsPerPixel, numSamples, pitch, height, numSlices, mipLevel, padDims, flags);
            }

            if (!result)
            {
                return 3;
            }

            return 0;
        }

        internal uint RestoreSurfaceInfo(uint elemMode, uint expandX, uint expandY, uint bitsPerPixel)
        {
            if (pOut.PixelPitch.ToBool() && pOut.PixelHeight.ToBool())
            {
                uint width = pOut.PixelPitch;
                uint height = pOut.PixelHeight;

                if (expandX > 1 || expandY > 1)
                {
                    if (elemMode == 4)
                    {
                        width = Floor(width / expandX);
                        height = Floor(height / expandY);
                    }
                    else
                    {
                        width *= expandX;
                        height *= expandY;
                    }
                }

                pOut.PixelPitch = Math.Max(1, width);
                pOut.PixelHeight = Math.Max(1, height);
            }

            if (bitsPerPixel.ToBool())
            {
                if (elemMode == 4)
                {
                    return expandY * expandX * bitsPerPixel;
                }
                else if (elemMode.In(5, 6))
                {
                    return Floor(Floor(bitsPerPixel / expandX) / expandY);
                }
                else if (elemMode.In(9, 12))
                {
                    return 64;
                }
                else if (elemMode.In(10, 11, 13))
                {
                    return 128;
                }

                return bitsPerPixel;
            }

            return 0;
        }

        internal void ComputeSurfaceInfo(SurfaceIn _surfIn, SurfaceOut _surfOut)
        {
            pIn = _surfIn;
            pOut = _surfOut;

            uint returnCode = 0;
            uint elemMode = 0;

            if (pIn.Bpp > 0x80)
            {
                returnCode = 3;
            }

            if (returnCode == 0)
            {
                ComputeMipLevel();

                uint width = pIn.Width;
                uint height = pIn.Height;
                uint bpp = pIn.Bpp;
                uint expandX = 1;
                uint expandY = 1;

                pOut.PixelBits = pIn.Bpp;

                if (pIn.Format.ToBool())
                {
                    (bpp, expandX, expandY, elemMode) = GetBitsPerPixel(pIn.Format);

                    if (elemMode == 4 && expandX == 3 && pIn.TileMode == 1)
                    {
                        pIn.Flags.Value |= 0x200;
                    }

                    bpp = AdjustSurfaceInfo(elemMode, expandX, expandY, bpp, width, height);
                }
                else if (bpp.ToBool())
                {
                    pIn.Width = Math.Max(1, pIn.Width);
                    pIn.Height = Math.Max(1, pIn.Height);
                }
                else
                {
                    returnCode = 3;
                }

                if (returnCode == 0)
                {
                    returnCode = ComputeSurfaceInfoEx();
                }

                if (returnCode == 0)
                {
                    pOut.Bpp = pIn.Bpp;
                    pOut.PixelPitch = pOut.Pitch;
                    pOut.PixelHeight = pOut.Height;

                    if (pIn.Format.ToBool() && !CheckInt(pIn.Flags.Value >> 9, 1) || !pIn.MipLevel.ToBool())
                    {
                        bpp = RestoreSurfaceInfo(elemMode, expandX, expandY, bpp);
                    }

                    if (CheckInt(pIn.Flags.Value >> 5, 1))
                    {
                        pOut.SliceSize = pOut.SurfSize;
                    }
                    else
                    {
                        pOut.SliceSize = Floor(pOut.SurfSize / pOut.Depth);

                        if (pIn.Slice == (pIn.NumSlices - 1) && pIn.NumSlices > 1)
                        {
                            pOut.SliceSize += pOut.SliceSize * (pOut.Depth - pIn.NumSlices);
                        }
                    }

                    pOut.PitchTileMax = (pOut.Pitch >> 3) - 1;
                    pOut.HeightTileMax = (pOut.Height >> 3) - 1;
                    pOut.SliceTileMax = (pOut.Height * pOut.Pitch >> 6) - 1;
                }
            }
        }

        internal SurfaceOut GetSurfaceInfo(uint surfaceFormat, uint surfaceWidth, uint surfaceHeight, uint surfaceDepth, uint surfaceDim, uint surfaceTileMode, uint surfaceAA, int mipLevel)
        {
            uint dim, width, blockSize, numSamples = 0;

            SurfaceIn surfIn = new();
            SurfaceOut surfOut = new();

            uint hwFormat = surfaceFormat & 0x3F;

            if (surfaceTileMode == 16)
            {
                numSamples = (uint)(1 << (int)surfaceAA);

                if (hwFormat < 0x31 || hwFormat > 0x35)
                {
                    blockSize = 1;
                }
                else
                {
                    blockSize = 4;
                }

                width = ~(blockSize - 1) & (Math.Max(1, surfaceWidth >> mipLevel) + blockSize - 1);

                surfOut.Bpp = FormatHwInfo[(int)(hwFormat * 4)];
                surfOut.Size = 96;
                surfOut.Pitch = Floor(width / blockSize);
                surfOut.PixelBits = FormatHwInfo[(int)(hwFormat * 4)];
                surfOut.BaseAlign = 1;
                surfOut.PitchAlign = 1;
                surfOut.HeightAlign = 1;
                surfOut.DepthAlign = 1;
                dim = surfaceDim;

                if (dim == 0)
                {
                    surfOut.Height = Math.Max(1, surfaceHeight >> mipLevel);
                    surfOut.Depth = 1;
                }
                else if (dim == 2)
                {
                    surfOut.Height = Math.Max(1, surfaceHeight >> mipLevel);
                    surfOut.Depth = Math.Max(1, surfaceDepth >> mipLevel);
                }
                else if (dim == 3)
                {
                    surfOut.Height = Math.Max(1, surfaceHeight >> mipLevel);
                    surfOut.Depth = Math.Max(6, surfaceDepth);
                }
                else if (dim == 4)
                {
                    surfOut.Height = 1;
                    surfOut.Depth = surfaceDepth;
                }
                else if (dim.In(5, 7))
                {
                    surfOut.Height = Math.Max(1, surfaceHeight >> mipLevel);
                    surfOut.Depth = surfaceDepth;
                }

                surfOut.PixelPitch = width;
                surfOut.PixelHeight = ~(blockSize - 1) & (surfOut.Height + blockSize - 1);
                surfOut.Height = Floor(surfOut.PixelHeight / blockSize);
                surfOut.SurfSize = surfOut.Bpp * numSamples * surfOut.Depth * surfOut.Height * surfOut.Pitch >> 3;

                if (surfaceDim == 2)
                {
                    surfOut.SliceSize = surfOut.SurfSize;
                }
                else
                {
                    surfOut.SliceSize = Floor(surfOut.SurfSize / surfOut.Depth);
                }

                surfOut.PitchTileMax = (surfOut.Pitch >> 3) - 1;
                surfOut.HeightTileMax = (surfOut.Height >> 3) - 1;
                surfOut.SliceTileMax = (surfOut.Height * surfOut.Pitch >> 6) - 1;
            }
            else
            {
                surfIn.Size = 60;
                surfIn.TileMode = surfaceTileMode & 0xF;
                surfIn.Format = hwFormat;
                surfIn.Bpp = FormatHwInfo[(int)(hwFormat * 4)];
                surfIn.NumSamples = (uint)(1 << (int)surfaceAA);
                surfIn.NumFrags = surfIn.NumSamples;
                surfIn.Width = Math.Max(1, surfaceWidth >> mipLevel);
                dim = surfaceDim;

                if (dim == 0)
                {
                    surfIn.Height = 1;
                    surfIn.NumSlices = 1;
                }
                else if (dim.In(1, 6))
                {
                    surfIn.Height = Math.Max(1, surfaceHeight >> mipLevel);
                    surfIn.NumSlices = 1;
                }
                else if (dim == 2)
                {
                    surfIn.Height = Math.Max(1, surfaceHeight >> mipLevel);
                    surfIn.NumSlices = Math.Max(1, surfaceDepth >> mipLevel);
                }
                else if (dim == 3)
                {
                    surfIn.Height = Math.Max(1, surfaceHeight >> mipLevel);
                    surfIn.NumSlices = Math.Max(6, surfaceDepth);
                    surfIn.Flags.Value |= 0x10;
                }
                else if (dim == 4)
                {
                    surfIn.Height = 1;
                    surfIn.NumSlices = surfaceDepth;
                }
                else if (dim.In(5, 7))
                {
                    surfIn.Height = Math.Max(1, surfaceHeight >> mipLevel);
                    surfIn.NumSlices = surfaceDepth;
                }

                surfIn.Slice = 0;
                surfIn.MipLevel = mipLevel;

                if (surfaceDim == 2)
                {
                    surfIn.Flags.Value |= 0x20;
                }
                if (mipLevel == 0)
                {
                    surfIn.Flags.Value = 1 << 12 | surfIn.Flags.Value & 0xFFFFEFFF;
                }
                else
                {
                    surfIn.Flags.Value = surfIn.Flags.Value & 0xFFFFEFFF;
                }

                surfOut.Size = 96;
                ComputeSurfaceInfo(surfIn, surfOut);

                surfOut = pOut;
            }

            if (!surfOut.TileMode.ToBool())
            {
                surfOut.TileMode = 16;
            }

            return surfOut;
        }
    }
}
