using Nintendo.Aamp.IO;
using Syroot.BinaryData;
using System;
using System.IO;

namespace Nintendo.Aamp.Parser
{
    internal class ParamEntryV2
    {
        internal static ParamEntry Read(FileReader reader)
        {
            ParamEntry entry = new();

            long CurrentPosition = reader.Position;

            entry.Hash = reader.ReadUInt32();
            int field4 = reader.ReadInt32();
            int DataOffset = (field4 & 0xffffff);
            var type = (field4 >> 24);
            entry.ParamType = (ParamType)type;

            if (DataOffset != 0)
            {
                using (reader.TemporarySeek(DataOffset * 4 + CurrentPosition, SeekOrigin.Begin))
                {
                    switch (entry.ParamType)
                    {
                        case ParamType.Boolean: entry.Value = reader.ReadIntBoolean(); break;
                        case ParamType.Float: entry.Value = reader.ReadSingle(); break;
                        case ParamType.Quat: entry.Value = reader.ReadSingles(4); break;
                        case ParamType.Int: entry.Value = reader.ReadInt32(); break;
                        case ParamType.Vector2F: entry.Value = reader.ReadVector2F(); break;
                        case ParamType.Vector3F: entry.Value = reader.ReadVector3F(); break;
                        case ParamType.Vector4F: entry.Value = reader.ReadVector4F(); break;
                        case ParamType.Color4F: entry.Value = reader.ReadColor4F(); break;
                        case ParamType.Uint: entry.Value = reader.ReadUInt32(); break;
                        case ParamType.BufferUint:
                            reader.Seek(-4, SeekOrigin.Current);
                            uint countUInt = reader.ReadUInt32();
                            Console.WriteLine($"countUInt {countUInt}");
                            entry.Value = reader.ReadUInt32s((int)countUInt);
                            break;
                        case ParamType.BufferInt:
                            reader.Seek(-4, SeekOrigin.Current);
                            uint countInt = reader.ReadUInt32();
                            Console.WriteLine($"countInt {countInt}");
                            entry.Value = reader.ReadInt32s((int)countInt);
                            break;
                        case ParamType.BufferFloat:
                            reader.Seek(-4, SeekOrigin.Current);
                            uint countF = reader.ReadUInt32();
                            Console.WriteLine($"countF {countF}");
                            entry.Value = reader.ReadSingles((int)countF);
                            break;
                        case ParamType.BufferBinary:
                            reader.Seek(-4, SeekOrigin.Current);
                            uint countBin = reader.ReadUInt32();
                            entry.Value = reader.ReadBytes((int)countBin);
                            break;
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
                                curves[i] = new Curve {
                                    ValueUints = reader.ReadUInt32s(2),
                                    ValueFloats = reader.ReadSingles(30)
                                };
                            }
                            break;
                        default:
                            throw new Exception("Unsupported param type! " + entry.ParamType);
                    }
                }
            }
            else
            {
                switch (entry.ParamType)
                {
                    case ParamType.Boolean:
                        entry.Value = false;
                        break;
                    case ParamType.String256:
                    case ParamType.String32:
                    case ParamType.String64:
                    case ParamType.StringRef:
                        entry.Value = "";
                        break;
                    case ParamType.Float:
                    case ParamType.Uint:
                    case ParamType.Int:
                        entry.Value = 0;
                        break;
                }
            }

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
    }
}
