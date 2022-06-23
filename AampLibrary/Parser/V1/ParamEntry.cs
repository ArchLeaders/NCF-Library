using Nintendo.Aamp.IO;
using Syroot.Maths;
using Syroot.BinaryData;
using System.IO;

namespace Nintendo.Aamp.Parser
{
    internal class ParamEntryV1
    {
        internal static ParamEntry Read(FileReader reader)
        {
            ParamEntry entry = new();

            long CurrentPosition = reader.Position;

            uint Size = reader.ReadUInt32();
            entry.ParamType = (ParamType)reader.ReadUInt32();
            entry.Hash = reader.ReadUInt32();

            int DataSize = (int)Size - 12;

            switch (entry.ParamType)
            {
                case ParamType.Boolean: entry.Value = reader.ReadBoolean(); break;
                case ParamType.Float: entry.Value = reader.ReadSingle(); break;
                case ParamType.Int: entry.Value = reader.ReadInt32(); break;
                case ParamType.Vector2F: entry.Value = reader.ReadVector2F(); break;
                case ParamType.Vector3F: entry.Value = reader.ReadVector3F(); break;
                case ParamType.Vector4F: entry.Value = reader.ReadVector4F(); break;
                case ParamType.Quat: entry.Value = reader.ReadSingles(4); break;
                case ParamType.Color4F: entry.Value = reader.ReadColor4F(); break;
                case ParamType.Uint: entry.Value = reader.ReadUInt32(); break;
                case ParamType.BufferUint: entry.Value = reader.ReadUInt32s(DataSize / sizeof(uint)); break;
                case ParamType.BufferInt: entry.Value = reader.ReadInt32s(DataSize / sizeof(int)); break;
                case ParamType.BufferFloat: entry.Value = reader.ReadSingles(DataSize / sizeof(float)); break;
                case ParamType.BufferBinary: entry.Value = reader.ReadBytes(DataSize); break;
                case ParamType.String64:
                    entry.Value = CreateStringEntry(reader, 64);
                    break;
                case ParamType.String32:
                    entry.Value = CreateStringEntry(reader, 32);
                    break;
                case ParamType.String256:
                    entry.Value = CreateStringEntry(reader, 256);
                    break;
                case ParamType.StringRef:
                    entry.Value = CreateStringEntry(reader, -1);
                    break;
                case ParamType.Curve1:
                case ParamType.Curve2:
                case ParamType.Curve3:
                case ParamType.Curve4:
                    int curveAmount = entry.ParamType - ParamType.Curve1 + 1;

                    var curves = new Curve[curveAmount];
                    entry.Value = curves;

                    for (int i = 0; i < curveAmount; i++)
                    {
                        curves[i] = new();
                        curves[i].ValueUints = reader.ReadUInt32s(2);
                        curves[i].ValueFloats = reader.ReadSingles(30);
                    }
                    break;
                 default:
                    entry.Value = reader.ReadBytes(DataSize);
                    break;
            }

            reader.Seek(CurrentPosition + Size, SeekOrigin.Begin);
            return entry;
        }

        private static StringEntry CreateStringEntry(BinaryStream reader, int MaxValue)
        {
            long pos = reader.Position;

            if (MaxValue == -1)
                MaxValue = 255;

            int length = 0;
            for (int i = 0; i < MaxValue; i++)
            {
                if (reader.ReadByte() != 0)
                    length++;
                else
                    break;
            }

            reader.Seek(pos, SeekOrigin.Begin);
            byte[] data = reader.ReadBytes(length);
            return new StringEntry(data, MaxValue);
        }

        internal static void Write(ParamEntry entry, FileWriter writer)
        {
            long startPosition = writer.Position;
            writer.Write(uint.MaxValue); //Write the size after
            writer.Write((uint)entry.ParamType);
            writer.Write(entry.Hash);

            switch (entry.ParamType)
            {
                case ParamType.Boolean: writer.Write((bool)entry.Value == false ? (byte)0 : (byte)1); break;
                case ParamType.Float: writer.Write((float)entry.Value); break;
                case ParamType.Int: writer.Write((int)entry.Value); break;
                case ParamType.Vector2F: writer.WriteVector2F((Vector2F)entry.Value); break;
                case ParamType.Vector3F: writer.WriteVector3F((Vector3F)entry.Value); break;
                case ParamType.Vector4F: writer.WriteVector4F((Vector4F)entry.Value); break;
                case ParamType.Color4F: writer.WriteColor4F((Color4F)entry.Value); break;
                case ParamType.Quat: writer.Write((float[])entry.Value); break;
                case ParamType.Uint: writer.Write((uint)entry.Value); break;
                case ParamType.BufferUint: writer.Write((uint[])entry.Value); break;
                case ParamType.BufferInt: writer.Write((int[])entry.Value); break;
                case ParamType.BufferFloat: writer.Write((float[])entry.Value); break;
                case ParamType.BufferBinary: writer.Write((byte[])entry.Value); break;
                case ParamType.String64:
                case ParamType.String32:
                case ParamType.String256:
                case ParamType.StringRef:
                    writer.Write(((StringEntry)entry.Value).Data);
               //     writer.Align(4);
                    break;
               case ParamType.Curve1:
                case ParamType.Curve2:
                case ParamType.Curve3:
                case ParamType.Curve4:
                    int curveAmount = entry.ParamType - ParamType.Curve1 + 1;

                    var curves = (Curve[])entry.Value;
                    for (int i = 0; i < curveAmount; i++)
                    {
                        writer.Write(curves[i].ValueUints);
                        writer.Write(curves[i].ValueFloats);
                    }
                    break;
                default:
                    writer.Write((byte[])entry.Value);
                    break;
            }

            writer.WriteSize(writer.Position, startPosition);
        }
    }
}
