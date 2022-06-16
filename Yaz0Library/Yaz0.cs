using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Nintendo.Yaz0
{
    /// <summary>
    /// <para><u>Note:</u> to use the fast compressor/decompressor you must have the compiled C library <b><a href="https://github.com/ArchLeaders/NCF-Library/raw/master/Yaz0Library/Yaz0.dll">Yaz0.dll</a></b> in the root build folder.</para>
    /// <para><i>C# implementation of Yaz0 copied from KillzXGaming's <a href="https://github.com/KillzXGaming/BfresPlatformConverter/blob/master/YAZ0.cs">BfresPLatformConverter</a></i></para>
    /// </summary>
    public class Yaz0
    {
        //
        // C entry points
        #region Expand

        [DllImport("Lib\\Yaz0.dll", EntryPoint = "decompress", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* C_Decompress(byte* src, uint srcLen, uint* destLen);

        [DllImport("Lib\\Yaz0.dll", EntryPoint = "compress", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe byte* C_Compress(byte* src, uint srcLen, uint* destLen, byte optCompr);

        [DllImport("Lib\\Yaz0.dll", EntryPoint = "freePtr", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void C_FreePtr(void* ptr);

        #endregion

        //
        // C wrapper
        #region Expand

        public static unsafe byte[] CompressFast(string fileName, int level = 7) => CompressFast(File.ReadAllBytes(fileName), level);
        public static unsafe byte[] CompressFast(byte[] data, int level = 7)
        {
            uint srcLen = (uint)data.Length;
            uint destLen;
            fixed (byte* inputPtr = data) {

                // Compress byte ptr
                byte* outputPtr = C_Compress(inputPtr, srcLen, &destLen, (byte)level);

                // Write header
                byte[] comp = new byte[] {
                    (byte)'Y',
                    (byte)'a',
                    (byte)'z',
                    (byte)'0',
                    (byte)((data.Length >> 24) & 0xFF),
                    (byte)((data.Length >> 16) & 0xFF),
                    (byte)((data.Length >> 8) & 0xFF),
                    (byte)((data.Length >> 0) & 0xFF),
                };

                // Copy to a byte[]
                Array.Resize(ref comp, (int)destLen + 16);
                Marshal.Copy((IntPtr)outputPtr, comp, 16, (int)destLen);
                C_FreePtr(outputPtr);
                return comp;
            }
        }


        public static unsafe byte[] DecompressFast(string file) => DecompressFast(File.ReadAllBytes(file));
        public static unsafe byte[] DecompressFast(byte[] data)
        {
            uint srcLen = (uint)data.Length;
            uint destLen;
            fixed (byte* inputPtr = data) {
                byte* outputPtr = C_Decompress(inputPtr, srcLen, &destLen);
                byte[] decomp = new byte[destLen];
                Marshal.Copy((IntPtr)outputPtr, decomp, 0, (int)destLen);
                C_FreePtr(outputPtr);
                return decomp;
            }
        }

        #endregion

        //
        // C# implementation
        #region Expand

        public static unsafe byte[] Compress(string FileName, int level = 7, uint res1 = 0, uint res2 = 0) => Compress(File.ReadAllBytes(FileName), level, res1, res2);
        public static unsafe byte[] Compress(byte[] Data, int level = 7, uint reserved1 = 0, uint reserved2 = 0)
        {
            int maxBackLevel = (int)(0x10e0 * (level / 9.0) - 0x0e0);

            byte* dataptr = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(Data, 0);

            byte[] result = new byte[Data.Length + Data.Length / 8 + 0x10];
            byte* resultptr = (byte*)Marshal.UnsafeAddrOfPinnedArrayElement(result, 0);
            *resultptr++ = (byte)'Y';
            *resultptr++ = (byte)'a';
            *resultptr++ = (byte)'z';
            *resultptr++ = (byte)'0';
            *resultptr++ = (byte)((Data.Length >> 24) & 0xFF);
            *resultptr++ = (byte)((Data.Length >> 16) & 0xFF);
            *resultptr++ = (byte)((Data.Length >> 8) & 0xFF);
            *resultptr++ = (byte)((Data.Length >> 0) & 0xFF);
            {
                var res1 = BitConverter.GetBytes(reserved1);
                var res2 = BitConverter.GetBytes(reserved2);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(res1);
                    Array.Reverse(res2);
                }
                *resultptr++ = res1[0];
                *resultptr++ = res1[1];
                *resultptr++ = res1[2];
                *resultptr++ = res1[3];
                *resultptr++ = res2[0];
                *resultptr++ = res2[1];
                *resultptr++ = res2[2];
                *resultptr++ = res2[3];
            }
            int length = Data.Length;
            int dstoffs = 16;
            int Offs = 0;
            while (true)
            {
                int headeroffs = dstoffs++;
                resultptr++;
                byte header = 0;
                for (int i = 0; i < 8; i++)
                {
                    int comp = 0;
                    int back = 1;
                    int nr = 2;
                    {
                        byte* ptr = dataptr - 1;
                        int maxnum = 0x111;
                        if (length - Offs < maxnum) maxnum = length - Offs;
                        //Use a smaller amount of bytes back to decrease time
                        int maxback = maxBackLevel;//0x1000;
                        if (Offs < maxback) maxback = Offs;
                        maxback = (int)dataptr - maxback;
                        int tmpnr;
                        while (maxback <= (int)ptr)
                        {
                            if (*(ushort*)ptr == *(ushort*)dataptr && ptr[2] == dataptr[2])
                            {
                                tmpnr = 3;
                                while (tmpnr < maxnum && ptr[tmpnr] == dataptr[tmpnr]) tmpnr++;
                                if (tmpnr > nr)
                                {
                                    if (Offs + tmpnr > length)
                                    {
                                        nr = length - Offs;
                                        back = (int)(dataptr - ptr);
                                        break;
                                    }
                                    nr = tmpnr;
                                    back = (int)(dataptr - ptr);
                                    if (nr == maxnum) break;
                                }
                            }
                            --ptr;
                        }
                    }
                    if (nr > 2)
                    {
                        Offs += nr;
                        dataptr += nr;
                        if (nr >= 0x12)
                        {
                            *resultptr++ = (byte)(((back - 1) >> 8) & 0xF);
                            *resultptr++ = (byte)((back - 1) & 0xFF);
                            *resultptr++ = (byte)((nr - 0x12) & 0xFF);
                            dstoffs += 3;
                        }
                        else
                        {
                            *resultptr++ = (byte)((((back - 1) >> 8) & 0xF) | (((nr - 2) & 0xF) << 4));
                            *resultptr++ = (byte)((back - 1) & 0xFF);
                            dstoffs += 2;
                        }
                        comp = 1;
                    }
                    else
                    {
                        *resultptr++ = *dataptr++;
                        dstoffs++;
                        Offs++;
                    }
                    header = (byte)((header << 1) | ((comp == 1) ? 0 : 1));
                    if (Offs >= length)
                    {
                        header = (byte)(header << (7 - i));
                        break;
                    }
                }
                result[headeroffs] = header;
                if (Offs >= length) break;
            }
            while ((dstoffs % 4) != 0) dstoffs++;
            byte[] realresult = new byte[dstoffs];
            Array.Copy(result, realresult, dstoffs);
            return realresult;
        }

        public static byte[] Decompress(string file) => Decompress(File.ReadAllBytes(file));
        public static byte[] Decompress(byte[] Data)
        {
            uint leng = (uint)(Data[4] << 24 | Data[5] << 16 | Data[6] << 8 | Data[7]);
            byte[] Result = new byte[leng];
            int Offs = 16;
            int dstoffs = 0;
            while (true)
            {
                byte header = Data[Offs++];
                for (int i = 0; i < 8; i++)
                {
                    if ((header & 0x80) != 0) Result[dstoffs++] = Data[Offs++];
                    else
                    {
                        byte b = Data[Offs++];
                        int offs = ((b & 0xF) << 8 | Data[Offs++]) + 1;
                        int length = (b >> 4) + 2;
                        if (length == 2) length = Data[Offs++] + 0x12;
                        for (int j = 0; j < length; j++)
                        {
                            Result[dstoffs] = Result[dstoffs - offs];
                            dstoffs++;
                        }
                    }
                    if (dstoffs >= leng) return Result;
                    header <<= 1;
                }
            }
        }

        #endregion
    }
}
