﻿using System;
using System.Collections.Generic;
using Syroot.BinaryData;
using Syroot.Maths;
using Nintendo.Bfres;
using Nintendo.Bfres.GX2;

namespace Nintendo.Bfres.Core
{
    /// <summary>
    /// Represents extension methods for the <see cref="BinaryStream"/> class.
    /// </summary>
    public static class BinaryDataReaderExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Reads a <see cref="AnimConstant"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="AnimConstant"/> instance.</returns>
        public static AnimConstant ReadAnimConstant(this BinaryStream self)
        {
            return new AnimConstant()
            {
                AnimDataOffset = self.ReadUInt32(),
                Value = self.ReadInt32()
            };
        }

        /// <summary>
        /// Reads a <see cref="AnimConstant"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="AnimConstant"/> instance.</returns>
        public static AnimConstant[] ReadAnimConstants(this BinaryStream self, int count)
        {
            AnimConstant[] values = new AnimConstant[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadAnimConstant();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Bounding"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Bounding"/> instance.</returns>
        public static Bounding ReadBounding(this BinaryStream self)
        {
            return new Bounding()
            {
                Center = self.ReadVector3F(),
                Extent = self.ReadVector3F()
            };
        }

        /// <summary>
        /// Reads <see cref="Bounding"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Bounding"/> instances.</returns>
        public static IList<Bounding> ReadBoundings(this BinaryStream self, int count)
        {
            Bounding[] values = new Bounding[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadBounding();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Decimal10x5"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Decimal10x5"/> instance.</returns>
        public static Decimal10x5 ReadDecimal10x5(this BinaryStream self) => new(self.ReadUInt16());

        /// <summary>
        /// Reads <see cref="Decimal10x5"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Decimal10x5"/> instances.</returns>
        public static IList<Decimal10x5> ReadDecimal10x5s(this BinaryStream self, int count)
        {
            Decimal10x5[] values = new Decimal10x5[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadDecimal10x5();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Half"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Half"/> instance.</returns>
        public static Half ReadHalf(this BinaryStream self) => new(self.ReadUInt16());

        /// <summary>
        /// Reads <see cref="Half"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Half"/> instances.</returns>
        public static IList<Half> ReadHalfs(this BinaryStream self, int count)
        {
            Half[] values = new Half[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadHalf();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Matrix3x4"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Matrix3x4"/> instance.</returns>
        public static Matrix3x4 ReadMatrix3x4(this BinaryStream self)
        {
            return new Matrix3x4(
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle(),
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle(),
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle());
        }

        /// <summary>
        /// Reads <see cref="Matrix3x4"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Matrix3x4"/> instances.</returns>
        public static IList<Matrix3x4> ReadMatrix3x4s(this BinaryStream self, int count)
        {
            Matrix3x4[] values = new Matrix3x4[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadMatrix3x4();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector2"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector2"/> instance.</returns>
        public static Vector2 ReadVector2(this BinaryStream self) => new(self.ReadInt32(), self.ReadInt32());

        /// <summary>
        /// Reads <see cref="Vector2"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector2"/> instances.</returns>
        public static IList<Vector2> ReadVector2s(this BinaryStream self, int count)
        {
            Vector2[] values = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector2();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector2Bool"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="format">The <see cref="BooleanCoding"/> in which values are stored.</param>
        /// <returns>The <see cref="Vector2Bool"/> instance.</returns>
        public static Vector2Bool ReadVector2Bool(this BinaryStream self, BooleanCoding format = BooleanCoding.Byte) =>
            new Vector2Bool(self.ReadBoolean(format), self.ReadBoolean(format));

        /// <summary>
        /// Reads <see cref="Vector2Bool"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="format">The <see cref="BooleanCoding"/> in which values are stored.</param>
        /// <returns>The <see cref="Vector2Bool"/> instances.</returns>
        public static IList<Vector2Bool> ReadVector2Bools(this BinaryStream self, int count, BooleanCoding format = BooleanCoding.Byte)
        {
            Vector2Bool[] values = new Vector2Bool[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector2Bool(format);
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector2F"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector2F"/> instance.</returns>
        public static Vector2F ReadVector2F(this BinaryStream self) => new(self.ReadSingle(), self.ReadSingle());

        /// <summary>
        /// Reads <see cref="Vector2F"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector2F"/> instances.</returns>
        public static IList<Vector2F> ReadVector2Fs(this BinaryStream self, int count)
        {
            Vector2F[] values = new Vector2F[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector2F();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector2U"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector2U"/> instance.</returns>
        public static Vector2U ReadVector2U(this BinaryStream self) => new(self.ReadUInt32(), self.ReadUInt32());

        /// <summary>
        /// Reads <see cref="Vector2U"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector2U"/> instances.</returns>
        public static IList<Vector2U> ReadVector2Us(this BinaryStream self, int count)
        {
            Vector2U[] values = new Vector2U[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector2U();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector3"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector3"/> instance.</returns>
        public static Vector3 ReadVector3(this BinaryStream self) => new Vector3(self.ReadInt32(), self.ReadInt32(), self.ReadInt32());

        /// <summary>
        /// Reads <see cref="Vector3"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector3"/> instances.</returns>
        public static IList<Vector3> ReadVector3s(this BinaryStream self, int count)
        {
            Vector3[] values = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector3();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector3Bool"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="format">The <see cref="BooleanCoding"/> in which values are stored.</param>
        /// <returns>The <see cref="Vector3Bool"/> instance.</returns>
        public static Vector3Bool ReadVector3Bool(this BinaryStream self, BooleanCoding format = BooleanCoding.Byte)
            => new Vector3Bool(self.ReadBoolean(format), self.ReadBoolean(format), self.ReadBoolean(format));

        /// <summary>
        /// Reads <see cref="Vector3Bool"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="format">The <see cref="BooleanCoding"/> in which values are stored.</param>
        /// <returns>The <see cref="Vector3Bool"/> instances.</returns>
        public static IList<Vector3Bool> ReadVector3Bools(this BinaryStream self, int count, BooleanCoding format = BooleanCoding.Byte)
        {
            Vector3Bool[] values = new Vector3Bool[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector3Bool(format);
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector3F"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector3F"/> instance.</returns>
        public static Vector3F ReadVector3F(this BinaryStream self) => new(self.ReadSingle(), self.ReadSingle(), self.ReadSingle());

        /// <summary>
        /// Reads <see cref="Vector3F"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector3F"/> instances.</returns>
        public static IList<Vector3F> ReadVector3Fs(this BinaryStream self, int count)
        {
            Vector3F[] values = new Vector3F[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector3F();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector3U"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector3U"/> instance.</returns>
        public static Vector3U ReadVector3U(this BinaryStream self) => new(self.ReadUInt32(), self.ReadUInt32(), self.ReadUInt32());

        /// <summary>
        /// Reads <see cref="Vector3U"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector3U"/> instances.</returns>
        public static IList<Vector3U> ReadVector3Us(this BinaryStream self, int count)
        {
            Vector3U[] values = new Vector3U[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector3U();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector4"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector4"/> instance.</returns>
        public static Vector4 ReadVector4(this BinaryStream self) => new(self.ReadInt32(), self.ReadInt32(), self.ReadInt32(), self.ReadInt32());

        /// <summary>
        /// Reads <see cref="Vector4"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector4"/> instances.</returns>
        public static IList<Vector4> ReadVector4s(this BinaryStream self, int count)
        {
            Vector4[] values = new Vector4[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector4();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector4Bool"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="format">The <see cref="BooleanCoding"/> in which values are stored.</param>
        /// <returns>The <see cref="Vector4Bool"/> instance.</returns>
        public static Vector4Bool ReadVector4Bool(this BinaryStream self, BooleanCoding format = BooleanCoding.Byte)
            => new(self.ReadBoolean(format), self.ReadBoolean(format), self.ReadBoolean(format), self.ReadBoolean(format));

        /// <summary>
        /// Reads <see cref="Vector4Bool"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="format">The <see cref="BooleanCoding"/> in which values are stored.</param>
        /// <returns>The <see cref="Vector4Bool"/> instances.</returns>
        public static IList<Vector4Bool> ReadVector4Bools(this BinaryStream self, int count, BooleanCoding format = BooleanCoding.Byte)
        {
            Vector4Bool[] values = new Vector4Bool[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector4Bool(format);
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector4F"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector4F"/> instance.</returns>
        public static Vector4F ReadVector4F(this BinaryStream self) => new(self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle());

        /// <summary>
        /// Reads <see cref="Vector4F"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector4F"/> instances.</returns>
        public static IList<Vector4F> ReadVector4Fs(this BinaryStream self, int count)
        {
            Vector4F[] values = new Vector4F[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector4F();
            }
            return values;
        }


        /// <summary>
        /// Reads a <see cref="Vector4U"/> instance from the current stream and returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <returns>The <see cref="Vector4U"/> instance.</returns>
        public static Vector4U ReadVector4U(this BinaryStream self) => new(self.ReadUInt32(), self.ReadUInt32(), self.ReadUInt32(), self.ReadUInt32());

        /// <summary>
        /// Reads <see cref="Vector4U"/> instances from the current stream and returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <returns>The <see cref="Vector4U"/> instances.</returns>
        public static IList<Vector4U> ReadVector4Us(this BinaryStream self, int count)
        {
            Vector4U[] values = new Vector4U[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = self.ReadVector4U();
            }
            return values;
        }


        /// <summary>
        /// Returns the conversion delegate for converting data available in the given <paramref name="attribFormat"/>
        /// into a <see cref="Vector4F"/> instance. Useful to prevent repetitive lookup for multiple values.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="attribFormat">The <see cref="GX2AttribFormat"/> of the data.</param>
        /// <returns>A conversion delegate for the data.</returns>
        public static Func<BinaryStream, Vector4F> ReadGX2AttribCallback(this BinaryStream self, GX2AttribFormat attribFormat)
        {
            return attribFormat switch
            {
                // 8-bit (8 x 1)
                GX2AttribFormat.Format_8_UNorm => Read_8_UNorm,
                GX2AttribFormat.Format_8_UInt => Read_8_UInt,
                GX2AttribFormat.Format_8_SNorm => Read_8_SNorm,
                GX2AttribFormat.Format_8_SInt => Read_8_SInt,
                GX2AttribFormat.Format_8_UIntToSingle => Read_8_UIntToSingle,
                GX2AttribFormat.Format_8_SIntToSingle => Read_8_SIntToSingle,
                // 8-bit (4 x 2)
                GX2AttribFormat.Format_4_4_UNorm => Read_4_4_UNorm,
                // 16-bit (16 x 1)
                GX2AttribFormat.Format_16_UNorm => Read_16_UNorm,
                GX2AttribFormat.Format_16_UInt => Read_16_UInt,
                GX2AttribFormat.Format_16_SNorm => Read_16_SNorm,
                GX2AttribFormat.Format_16_SInt => Read_16_SInt,
                GX2AttribFormat.Format_16_Single => Read_16_Single,
                GX2AttribFormat.Format_16_UIntToSingle => Read_16_UIntToSingle,
                GX2AttribFormat.Format_16_SIntToSingle => Read_16_SIntToSingle,
                // 16-bit (8 x 2)
                GX2AttribFormat.Format_8_8_UNorm => Read_8_8_UNorm,
                GX2AttribFormat.Format_8_8_UInt => Read_8_8_UInt,
                GX2AttribFormat.Format_8_8_SNorm => Read_8_8_SNorm,
                GX2AttribFormat.Format_8_8_SInt => Read_8_8_SInt,
                GX2AttribFormat.Format_8_8_UIntToSingle => Read_8_8_UIntToSingle,
                GX2AttribFormat.Format_8_8_SIntToSingle => Read_8_8_SIntToSingle,
                // 32-bit (32 x 1)
                GX2AttribFormat.Format_32_UInt => Read_32_UInt,
                GX2AttribFormat.Format_32_SInt => Read_32_SInt,
                GX2AttribFormat.Format_32_Single => Read_32_Single,
                // 32-bit (16 x 2)
                GX2AttribFormat.Format_16_16_UNorm => Read_16_16_UNorm,
                GX2AttribFormat.Format_16_16_UInt => Read_16_16_UInt,
                GX2AttribFormat.Format_16_16_SNorm => Read_16_16_SNorm,
                GX2AttribFormat.Format_16_16_SInt => Read_16_16_SInt,
                GX2AttribFormat.Format_16_16_Single => Read_16_16_Single,
                GX2AttribFormat.Format_16_16_UIntToSingle => Read_16_16_UIntToSingle,
                GX2AttribFormat.Format_16_16_SIntToSingle => Read_16_16_SIntToSingle,
                // 32-bit (10/11 x 3)
                GX2AttribFormat.Format_10_11_11_Single => Read_10_11_11_Single,
                // 32-bit (8 x 4)
                GX2AttribFormat.Format_8_8_8_8_UNorm => Read_8_8_8_8_UNorm,
                GX2AttribFormat.Format_8_8_8_8_UInt => Read_8_8_8_8_UInt,
                GX2AttribFormat.Format_8_8_8_8_SNorm => Read_8_8_8_8_SNorm,
                GX2AttribFormat.Format_8_8_8_8_SInt => Read_8_8_8_8_SInt,
                GX2AttribFormat.Format_8_8_8_8_UIntToSingle => Read_8_8_8_8_UIntToSingle,
                GX2AttribFormat.Format_8_8_8_8_SIntToSingle => Read_8_8_8_8_SIntToSingle,
                // 32-bit (10 x 3 + 2)
                GX2AttribFormat.Format_10_10_10_2_UNorm => Read_10_10_10_2_UNorm,
                GX2AttribFormat.Format_10_10_10_2_UInt => Read_10_10_10_2_UInt,
                GX2AttribFormat.Format_10_10_10_2_SNorm => Read_10_10_10_2_SNorm,
                GX2AttribFormat.Format_10_10_10_2_SInt => Read_10_10_10_2_SInt,
                // 64-bit (32 x 2)
                GX2AttribFormat.Format_32_32_UInt => Read_32_32_UInt,
                GX2AttribFormat.Format_32_32_SInt => Read_32_32_SInt,
                GX2AttribFormat.Format_32_32_Single => Read_32_32_Single,
                // 64-bit (16 x 4)
                GX2AttribFormat.Format_16_16_16_16_UNorm => Read_16_16_16_16_UNorm,
                GX2AttribFormat.Format_16_16_16_16_UInt => Read_16_16_16_16_UInt,
                GX2AttribFormat.Format_16_16_16_16_SNorm => Read_16_16_16_16_SNorm,
                GX2AttribFormat.Format_16_16_16_16_SInt => Read_16_16_16_16_SInt,
                GX2AttribFormat.Format_16_16_16_16_Single => Read_16_16_16_16_Single,
                GX2AttribFormat.Format_16_16_16_16_UIntToSingle => Read_16_16_16_16_UIntToSingle,
                GX2AttribFormat.Format_16_16_16_16_SIntToSingle => Read_16_16_16_16_SIntToSingle,
                // 96-bit (32 x 3)
                GX2AttribFormat.Format_32_32_32_UInt => Read_32_32_32_UInt,
                GX2AttribFormat.Format_32_32_32_SInt => Read_32_32_32_SInt,
                GX2AttribFormat.Format_32_32_32_Single => Read_32_32_32_Single,
                // 128-bit (32 x 4)
                GX2AttribFormat.Format_32_32_32_32_UInt => Read_32_32_32_32_UInt,
                GX2AttribFormat.Format_32_32_32_32_SInt => Read_32_32_32_32_SInt,
                GX2AttribFormat.Format_32_32_32_32_Single => Read_32_32_32_32_Single,
                // Invalid
                _ => throw new ArgumentException($"Invalid {nameof(GX2AttribFormat)} {attribFormat}.",
                                    nameof(attribFormat)),
            };
        }

        /// <summary>
        /// Reads a <see cref="Vector4F"/> instance converted from the given <paramref name="attribFormat"/> and
        /// returns it.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="attribFormat">The <see cref="GX2AttribFormat"/> of the data.</param>
        /// <returns>The <see cref="Vector4F"/> instance.</returns>
        public static Vector4F ReadGX2Attrib(this BinaryStream self, GX2AttribFormat attribFormat) => self.ReadGX2AttribCallback(attribFormat).Invoke(self);

        /// <summary>
        /// Reads a <see cref="Vector4F"/> instances converted from the given <paramref name="attribFormat"/> and
        /// returns them.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/>.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="attribFormat">The <see cref="GX2AttribFormat"/> of the data.</param>
        /// <returns>The <see cref="Vector4F"/> instances.</returns>
        public static IList<Vector4F> ReadGX2Attribs(this BinaryStream self, int count, GX2AttribFormat attribFormat)
        {
            Func<BinaryStream, Vector4F> callback = self.ReadGX2AttribCallback(attribFormat);

            Vector4F[] values = new Vector4F[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = callback.Invoke(self);
            }
            return values;
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        // ---- 8-bit (8 x 1) ----

        private static Vector4F Read_8_UNorm(this BinaryStream self) => new(self.ReadByte() / 255f, 0, 0, 0);
        private static Vector4F Read_8_UInt(this BinaryStream self) => new(self.ReadByte(), 0, 0, 0);
        private static Vector4F Read_8_SNorm(this BinaryStream self) => new(self.ReadSByte() / 127f, 0, 0, 0);
        private static Vector4F Read_8_SInt(this BinaryStream self) => new(self.ReadSByte(), 0, 0, 0);
        private static Vector4F Read_8_UIntToSingle(this BinaryStream self) => new(self.ReadByte(), 0, 0, 0);
        private static Vector4F Read_8_SIntToSingle(this BinaryStream self) => new(self.ReadSByte(), 0, 0, 0);

        // ---- 8-bit (4 x 2) ----

        private static Vector4F Read_4_4_UNorm(this BinaryStream self)
        {
            byte value = (byte)self.ReadByte();
            return new(
                (value & 0b00001111) / 127f,
                (value >> 4) / 127f,
                0,
                0);
        }

        // ---- 16-bit (16 x 1) ----

        private static Vector4F Read_16_UNorm(this BinaryStream self) => new(self.ReadUInt16() / 65535f, 0, 0, 0);
        private static Vector4F Read_16_UInt(this BinaryStream self) => new(self.ReadUInt16(), 0, 0, 0);
        private static Vector4F Read_16_SNorm(this BinaryStream self) => new(self.ReadInt16() / 32767f, 0, 0, 0);
        private static Vector4F Read_16_SInt(this BinaryStream self) => new(self.ReadInt16(), 0, 0, 0);
        private static Vector4F Read_16_Single(this BinaryStream self) => new(self.ReadHalf(), 0, 0, 0);
        private static Vector4F Read_16_UIntToSingle(this BinaryStream self) => new(self.ReadUInt16(), 0, 0, 0);
        private static Vector4F Read_16_SIntToSingle(this BinaryStream self) => new(self.ReadInt16(), 0, 0, 0);

        // ---- 16-bit (8 x 2) ----

        private static Vector4F Read_8_8_UNorm(this BinaryStream self) => new(self.ReadByte() / 255f, self.ReadByte() / 255f, 0, 0);
        private static Vector4F Read_8_8_UInt(this BinaryStream self) => new(self.ReadByte(), self.ReadByte(), 0, 0);
        private static Vector4F Read_8_8_SNorm(this BinaryStream self) => new(self.ReadSByte() / 127f, self.ReadSByte() / 127f, 0, 0);
        private static Vector4F Read_8_8_SInt(this BinaryStream self) => new(self.ReadSByte(), self.ReadSByte(), 0, 0);
        private static Vector4F Read_8_8_UIntToSingle(this BinaryStream self) => new(self.ReadByte(), self.ReadByte(), 0, 0);
        private static Vector4F Read_8_8_SIntToSingle(this BinaryStream self) => new(self.ReadSByte(), self.ReadSByte(), 0, 0);

        // ---- 32-bit (32 x 1) ----

        private static Vector4F Read_32_UInt(this BinaryStream self) => new(self.ReadUInt32(), 0, 0, 0);
        private static Vector4F Read_32_SInt(this BinaryStream self) => new(self.ReadInt32(), 0, 0, 0);
        private static Vector4F Read_32_Single(this BinaryStream self) => new(self.ReadSingle(), 0, 0, 0);

        // ---- 32-bit (16 x 2) ----

        private static Vector4F Read_16_16_UNorm(this BinaryStream self) => new(self.ReadUInt16() / 65535f, self.ReadUInt16() / 65535f, 0, 0);
        private static Vector4F Read_16_16_UInt(this BinaryStream self) => new(self.ReadUInt16(), self.ReadUInt16(), 0, 0);
        private static Vector4F Read_16_16_SNorm(this BinaryStream self) => new(self.ReadInt16() / 32767f, self.ReadInt16() / 32767f, 0, 0);
        private static Vector4F Read_16_16_SInt(this BinaryStream self) => new(self.ReadInt16(), self.ReadInt16(), 0, 0);
        private static Vector4F Read_16_16_Single(this BinaryStream self) => new(self.ReadHalf(), self.ReadHalf(), 0, 0);
        private static Vector4F Read_16_16_UIntToSingle(this BinaryStream self) => new(self.ReadUInt16(), self.ReadUInt16(), 0, 0);
        private static Vector4F Read_16_16_SIntToSingle(this BinaryStream self) => new(self.ReadInt16(), self.ReadInt16(), 0, 0);

        // ---- 32-bit (10/11 x 3) ----

        private static Vector4F Read_10_11_11_Single(this BinaryStream self) => throw new NotImplementedException("10-bit and 11-bit Single values have not yet been implemented.");

        // ---- 32-bit (8 x 4) ----

        private static Vector4F Read_8_8_8_8_UNorm(this BinaryStream self) => new(self.ReadByte() / 255f, self.ReadByte() / 255f, self.ReadByte() / 255f, self.ReadByte() / 255f);
        private static Vector4F Read_8_8_8_8_UInt(this BinaryStream self) => new(self.ReadByte(), self.ReadByte(), self.ReadByte(), self.ReadByte());
        private static Vector4F Read_8_8_8_8_SNorm(this BinaryStream self) => new(self.ReadSByte() / 127f, self.ReadSByte() / 127f, self.ReadSByte() / 127f, self.ReadSByte() / 127f);
        private static Vector4F Read_8_8_8_8_SInt(this BinaryStream self) => new(self.ReadSByte(), self.ReadSByte(), self.ReadSByte(), self.ReadSByte());
        private static Vector4F Read_8_8_8_8_UIntToSingle(this BinaryStream self) => new(self.ReadByte(), self.ReadByte(), self.ReadByte(), self.ReadByte());
        private static Vector4F Read_8_8_8_8_SIntToSingle(this BinaryStream self) => new(self.ReadSByte(), self.ReadSByte(), self.ReadSByte(), self.ReadSByte());

        // ---- 32-bit (10 x 3 + 2) ----

        private static Vector4F Read_10_10_10_2_UNorm(this BinaryStream self)
        {
            uint value = self.ReadUInt32();
            return new(
                (value & 0b00000000_00000000_00000011_11111111) / 1023f,
                ((value & 0b00000000_00001111_11111100_00000000) >> 10) / 1023f,
                ((value & 0b00111111_11110000_00000000_00000000) >> 20) / 1023f,
                ((value & 0b11000000_00000000_00000000_00000000) >> 30) / 3f);
        }

        private static Vector4F Read_10_10_10_2_UInt(this BinaryStream self)
        {
            uint value = self.ReadUInt32();
            return new(
                (value & 0b00000000_00000000_00000011_11111111),
                ((value & 0b00000000_00001111_11111100_00000000) >> 10),
                ((value & 0b00111111_11110000_00000000_00000000) >> 20),
                ((value & 0b11000000_00000000_00000000_00000000) >> 30));
        }

        private static Vector4F Read_10_10_10_2_SNorm(this BinaryStream self)
        {
            int value = self.ReadInt32();
            return new(
                (value << 22 >> 22) / 511f,
                (value << 12 >> 22) / 511f,
                (value << 2 >> 22) / 511f,
                value >> 30); // UNorm, though MK8 seems to store SNorm.
        }

        private static Vector4F Read_10_10_10_2_SInt(this BinaryStream self)
        {
            int value = self.ReadInt32();
            return new(
                value << 22 >> 22,
                value << 12 >> 22,
                value << 2 >> 22,
                value >> 30);
        }

        // ---- 64-bit (32 x 2) ----

        private static Vector4F Read_32_32_UInt(this BinaryStream self) => new(self.ReadUInt32(), self.ReadUInt32(), 0, 0);
        private static Vector4F Read_32_32_SInt(this BinaryStream self) => new(self.ReadInt32(), self.ReadInt32(), 0, 0);
        private static Vector4F Read_32_32_Single(this BinaryStream self) => new(self.ReadSingle(), self.ReadSingle(), 0, 0);

        // ---- 64-bit (16 x 4) ----

        private static Vector4F Read_16_16_16_16_UNorm(this BinaryStream self) => new(self.ReadUInt16() / 65535f, self.ReadUInt16() / 65535f, self.ReadUInt16() / 65535f, self.ReadUInt16() / 65535f);
        private static Vector4F Read_16_16_16_16_UInt(this BinaryStream self) => new(self.ReadUInt16(), self.ReadUInt16(), self.ReadUInt16(), self.ReadUInt16());
        private static Vector4F Read_16_16_16_16_SNorm(this BinaryStream self) => new(self.ReadInt16() / 32767f, self.ReadInt16() / 32767f, self.ReadInt16() / 32767f, self.ReadInt16() / 32767f);
        private static Vector4F Read_16_16_16_16_SInt(this BinaryStream self) => new(self.ReadInt16(), self.ReadInt16(), self.ReadInt16(), self.ReadInt16());
        private static Vector4F Read_16_16_16_16_Single(this BinaryStream self) => new(self.ReadHalf(), self.ReadHalf(), self.ReadHalf(), self.ReadHalf());
        private static Vector4F Read_16_16_16_16_UIntToSingle(this BinaryStream self) => new(self.ReadUInt16(), self.ReadUInt16(), self.ReadUInt16(), self.ReadUInt16());
        private static Vector4F Read_16_16_16_16_SIntToSingle(this BinaryStream self) => new(self.ReadInt16(), self.ReadInt16(), self.ReadInt16(), self.ReadInt16());

        // --- 96-bit (32 x 3) ----

        private static Vector4F Read_32_32_32_UInt(this BinaryStream self) => new(self.ReadUInt32(), self.ReadUInt32(), self.ReadUInt32(), 0);
        private static Vector4F Read_32_32_32_SInt(this BinaryStream self) => new(self.ReadInt32(), self.ReadInt32(), self.ReadInt32(), 0);
        private static Vector4F Read_32_32_32_Single(this BinaryStream self) => new(self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), 0);

        // ---- 128-bit (32 x 4) ----

        private static Vector4F Read_32_32_32_32_UInt(this BinaryStream self) => new(self.ReadUInt32(), self.ReadUInt32(), self.ReadUInt32(), self.ReadUInt32());
        private static Vector4F Read_32_32_32_32_SInt(this BinaryStream self) => new(self.ReadInt32(), self.ReadInt32(), self.ReadInt32(), self.ReadInt32());
        private static Vector4F Read_32_32_32_32_Single(this BinaryStream reader) => new(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
}
