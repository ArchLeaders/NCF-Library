#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8605 // Unboxing a possibly null value.

using Nintendo.Aamp.IO;
using System.Text;
using Syroot.Maths;
using SharpYaml.Serialization;
using Nintendo.Aamp.Parser;
using Aamp.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Nintendo.Aamp
{
    public class YamlConverter
    {
        #region Read
        public static AampFile FromYaml(string text)
        {
            AampFile file = new();
            var yaml = new YamlStream();
            yaml.Load(new StringReader(text));
            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
        
            foreach (var entry in mapping.Children)
            {
                var key = ((YamlScalarNode)entry.Key).Value;
                var value = entry.Value.ToString();
                if (key == "aamp_version")
                {
                    file.Version = (uint)ParseValueInt(value);
                    if (file.Version == 2)
                        file = new AampFileV2();
                    else
                        file = new AampFileV1();
                }
                if (key == "type")
                    file.ParameterIOType = value;

                if (entry.Value.Tag == "!list")
                    file.RootNode = ParseParamList(key, (YamlMappingNode)entry.Value);
            }

            return file;
        }

        private static ParamList ParseParamList(YamlScalarNode node, YamlNode valueNode)
        {
            ParamList paramList = new();
            paramList.Hash = ParseHash(node.Value);

            List<ParamList> children = new();
            List<ParamObject> objects = new();

            if (valueNode is YamlMappingNode castValueNode) {
                foreach (YamlMappingNode child in castValueNode.Children.Values)
                {
                    foreach (var subChild in child.Children)
                    {
                        var scalar = ((YamlScalarNode)subChild.Key);

                        if (subChild.Value.Tag == "!list")
                            children.Add(ParseParamList(scalar, subChild.Value));
                        if (subChild.Value.Tag == "!obj")
                            objects.Add(ParseParamObject(scalar, subChild.Value));
                    }
                }
            }

            paramList.ChildParams = children.ToArray();
            paramList.ParamObjects = objects.ToArray();

            return paramList;
        }

        private static ParamObject ParseParamObject(YamlScalarNode node, YamlNode valueNode)
        {
            ParamObject paramObject = new();
            paramObject.Hash = ParseHash(node.Value);

            List<ParamEntry> entries = new();
            if (valueNode is YamlMappingNode castValueNode) {
                foreach (var child in castValueNode.Children)
                    entries.Add(ParseParamEntry(((YamlScalarNode)child.Key).Value, child.Value));

            }

            paramObject.ParamEntries = entries.ToArray();
            return paramObject;
        }

        private static ParamEntry ParseParamEntry(string key, YamlNode valueNode)
        {
            ParamEntry entry = new() { Hash = ParseHash(key) };

            if (valueNode is YamlSequenceNode castValues)
            {
                switch (valueNode.Tag)
                {
                    case "!BufferBinary":
                        entry.Value = ToByteArray(castValues);
                        entry.ParamType = ParamType.BufferBinary;
                        break;
                    case "!BufferFloat":
                        entry.Value = ToFloatArray(castValues);
                        entry.ParamType = ParamType.BufferFloat;
                        break;
                    case "!BufferInt":
                        entry.Value = ToIntArray(castValues);
                        entry.ParamType = ParamType.BufferInt;
                        break;
                    case "!BufferUint":
                        entry.Value = ToUIntArray(castValues);
                        entry.ParamType = ParamType.BufferUint;
                        break;
                    case "!quat":
                        entry.Value = ToFloatArray(castValues);
                        entry.ParamType = ParamType.Quat;
                        break;
                    case "!vec2": {
                            float[] singles = ToFloatArray(castValues, 2);
                            entry.Value = new Vector2F(singles[0], singles[1]);
                            entry.ParamType = ParamType.Vector2F;
                        }
                        break;
                    case "!vec3": {
                            float[] singles = ToFloatArray(castValues, 3);
                            entry.Value = new Vector3F(singles[0], singles[1], singles[2]);
                            entry.ParamType = ParamType.Vector3F;
                        }
                        break;
                    case "!color": {
                            float[] singles = ToFloatArray(castValues, 4);
                            entry.Value = new Vector4F(
                                singles[0], singles[1],
                                singles[2], singles[3]);
                            entry.ParamType = ParamType.Color4F;
                        }
                        break;
                    case "!vec4": {
                            float[] singles = ToFloatArray(castValues, 4);
                            entry.Value = new Vector4F(
                                singles[0], singles[1],
                                singles[2], singles[3]);
                            entry.ParamType = ParamType.Vector4F;
                        }
                        break;
                    case "!curve1":
                        entry.Value = ParseCurves(castValues, 1);
                        entry.ParamType = ParamType.Curve1;
                        break;
                    case "!curve2":
                        entry.Value = ParseCurves(castValues, 2);
                        entry.ParamType = ParamType.Curve2;
                        break;
                    case "!curve3":
                        entry.Value = ParseCurves(castValues, 3);
                        entry.ParamType = ParamType.Curve3;
                        break;
                    case "!curve4":
                        entry.Value = ParseCurves(castValues, 4);
                        entry.ParamType = ParamType.Curve4;
                        break;
                    default:
                        throw new Exception($"Unknown tag type using a sequence! {valueNode.Tag}");
                }
            }
            else if (valueNode is YamlScalarNode castValueNode)
            {
                var value = castValueNode.Value;
                switch (valueNode.Tag)
                {
                    case "!str256":
                        entry.Value = new StringEntry(Encoding.UTF8.GetBytes(value));
                        entry.ParamType = ParamType.String256;
                        break;
                    case "!str64":
                        entry.Value = new StringEntry(Encoding.UTF8.GetBytes(value));
                        entry.ParamType = ParamType.String64;
                        break;
                    case "!str32":
                        entry.Value = new StringEntry(Encoding.UTF8.GetBytes(value));
                        entry.ParamType = ParamType.String32;
                        break;
                    case "!strRef":
                        entry.Value = new StringEntry(Encoding.UTF8.GetBytes(value));
                        entry.ParamType = ParamType.StringRef;
                        break;
                    default:
                        bool booleanValue;
                        uint uintValue;
                        float floatValue;
                        int intValue;
                        bool isBoolean = bool.TryParse(value, out booleanValue);
                        bool isUint = uint.TryParse(value, out uintValue);
                        bool isFloat = float.TryParse(value, out floatValue);
                        bool isInt = int.TryParse(value, out intValue);
                        bool HasDecimal = value.Contains('.');
                        if (isBoolean)
                        {
                            entry.Value = booleanValue;
                            entry.ParamType = ParamType.Boolean;
                        }
                        else if (isUint && !HasDecimal)
                        {
                            entry.Value = uintValue;
                            entry.ParamType = ParamType.Uint;
                        }
                        else if (isInt && !HasDecimal)
                        {
                            entry.Value = intValue;
                            entry.ParamType = ParamType.Int;
                        }
                        else if (isFloat)
                        {
                            entry.Value = floatValue;
                            entry.ParamType = ParamType.Float;
                        }
                        else
                            throw new Exception($"Failed to parse value for param {key} value {value}!");
                        break;
                }
            }

            return entry;
        }

        private static byte[] ToByteArray(YamlSequenceNode nodes)
        {
            List<byte> values = new();
            foreach (var val in nodes)
                values.Add(byte.Parse(val.ToString()));
            return values.ToArray();
        }

        private static uint[] ToUIntArray(YamlSequenceNode nodes)
        {
            List<uint> values = new();
            foreach (var val in nodes)
                values.Add(uint.Parse(val.ToString()));
            return values.ToArray();
        }

        private static int[] ToIntArray(YamlSequenceNode nodes)
        {
            List<int> values = new();
            foreach (var val in nodes)
                values.Add(int.Parse(val.ToString()));
            return values.ToArray();
        }

        private static float[] ToFloatArray(YamlSequenceNode nodes, int expectedLength = -1)
        {
            List<float> values = new();
            foreach (var val in nodes)
                values.Add(float.Parse(val.ToString()));

            if (expectedLength != -1 && values.Count != expectedLength)
                throw new System.Exception($"Invalid value length. " +
                    $"Expected {expectedLength}, got {values.Count}");

            return values.ToArray();
        }

        private static Curve[] ParseCurves(YamlSequenceNode nodes, int numCurves)
        {
            var values = nodes.ToList();

            Curve[] curves = new Curve[numCurves];
            int numValues = values.Count / numCurves; //Should be 32
            for (int i = 0; i < numCurves; i++) {
                List<uint> valueUints = new();
                List<float> valueFloats = new();

                //2 ints
                //30 floats
                for (int j = 0; j < numValues; j++)
                {
                    var val = values[i * numValues + j].ToString();
                    if (j < 2)
                        valueUints.Add(ParseValueUnit(val));
                    else 
                        valueFloats.Add(ParseValueFloat(val));
                }

                curves[i] = new Curve()
                {
                    ValueUints = valueUints.ToArray(),
                    ValueFloats = valueFloats.ToArray(),
                };
            }
            return curves;
        }

        private static uint ParseHash(string name)
        {
            bool isHash = uint.TryParse(name, out uint hash);
            if (!isHash || hash == 0 || Hashes.HasString(name))
                return Crc32.Compute(name);
            else
                return hash;
        }

        private static float ParseValueFloat(string value) {
            return float.Parse(ParseValueString(value));
        }

        private static uint ParseValueUnit(string value) {
            return uint.Parse(ParseValueString(value));
        }

        private static int ParseValueInt(string value) {
            return Int32.Parse(ParseValueString(value));
        }

        private static string ParseValueString(string value) {
            return value.Split(':').LastOrDefault();
        }

        #endregion

        #region Write

        public static string ToYaml(AampFile aampFile)
        {
            StringBuilder sb = new();

            using (TextWriter writer = new StringWriter(sb))
            {
                writer.WriteLine($"aamp_version: {aampFile.Version}");
                writer.WriteLine($"io_version: {aampFile.ParameterIOVersion}");
                writer.WriteLine($"type: {aampFile.ParameterIOType}");
                WriteParamList(writer, aampFile.RootNode, 0);
            }

            return sb.ToString();
        }

        private static void WriteParamList(TextWriter writer, ParamList paramList, int IndentAmount)
        {
            writer.WriteLine($"{YamlHashStr(paramList.HashString)}: !list".Indent(IndentAmount));
           // Console.WriteLine($"HashString {paramList.HashString}");

            if (paramList.ParamObjects.Length <= 0)
                writer.WriteLine("objects: {}".Indent(IndentAmount + 2));
            else
                writer.WriteLine("objects: ".Indent(IndentAmount + 2));

            foreach (var paramObj in paramList.ParamObjects)
            {
                WriteParamObject(writer, paramObj, IndentAmount + 4);
            }

            if (paramList.ChildParams.Length <= 0)
                writer.WriteLine("lists: {}".Indent(IndentAmount + 2));
            else
                writer.WriteLine("lists: ".Indent(IndentAmount + 2));

            foreach (var child in paramList.ChildParams)
            {
                WriteParamList(writer, child, IndentAmount + 4);
            }
        }

        private static void WriteParamObject(TextWriter writer, ParamObject paramObj, int IndentAmount)
        {
            writer.WriteLine($"{YamlHashStr(paramObj.HashString)}: !obj".Indent(IndentAmount));
            foreach (var entry in paramObj.ParamEntries)
            {
                writer.WriteLine($"{WriteParamData(entry)}".Indent(IndentAmount + 2));
            }
        }

        private static string WriteParamData(ParamEntry entry)
        {
            string value = entry.ParamType switch
            {
                ParamType.Boolean => $"{(bool)entry.Value}",
                ParamType.BufferBinary => $"!BufferBinary [{WriteBytes((byte[])entry.Value)}]",
                ParamType.BufferFloat => $"!BufferFloat [{WriteFloats((float[])entry.Value)}]",
                ParamType.BufferInt => $"!BufferInt [{WriteInts((int[])entry.Value)}]",
                ParamType.BufferUint => $"!BufferUint [{WriteUints((uint[])entry.Value)}]",
                ParamType.Quat => $"!quat [{WriteFloats((float[])entry.Value)}]",
                ParamType.Color4F => $"{WriteColor4F((Color4F)entry.Value)}",
                ParamType.Vector2F => $"{WriteVec2F((Vector2F)entry.Value)}",
                ParamType.Vector3F => $"{WriteVec3F((Vector3F)entry.Value)}",
                ParamType.Vector4F => $"{WriteVec4F((Vector4F)entry.Value)}",
                ParamType.Uint => $"{(uint)entry.Value}",
                ParamType.Int => $"{(int)entry.Value}",
                ParamType.Float => WriteFloat((float)entry.Value),
                ParamType.String256 => $"!str256 {(StringEntry)entry.Value}",
                ParamType.String32 => $"!str32 {(StringEntry)entry.Value}",
                ParamType.String64 => $"!str64 {(StringEntry)entry.Value}",
                ParamType.StringRef => $"!strRef {(StringEntry)entry.Value}",
                ParamType.Curve1 => $"{WriteCurve((Curve[])entry.Value, 1)}",
                ParamType.Curve2 => $"{WriteCurve((Curve[])entry.Value, 2)}",
                ParamType.Curve3 => $"{WriteCurve((Curve[])entry.Value, 3)}",
                ParamType.Curve4 => $"{WriteCurve((Curve[])entry.Value, 4)}",
                _ => throw new Exception("Unsupported type! " + entry.ParamType),
            };

            return $"{YamlHashStr(entry.HashString)}: {value}";
        }

        private static string YamlHashStr(string hash)
        {
            if (hash.Contains(':'))
                return '"' + hash + '"';
            return hash;
        }

        private static string WriteCurve(Curve[] curves, int Num)
        {
            string[] values = new string[Num];
            for (int i = 0; i < values.Length; i++)
                values[i] = $"{WriteUints(curves[i].ValueUints)},{WriteFloats(curves[i].ValueFloats)}";

            return $"!curve{Num} [{string.Join(",", values)}]";
        }

        private static string WriteUints(uint[] arr)
        {
            return String.Join(", ", arr.Select(p => p.ToString()).ToArray());
        }

        private static string WriteFloats(float[] arr)
        {
            return String.Join(", ", arr.Select(p => WriteFloat(p)).ToArray());
        }

        private static string WriteInts(int[] arr)
        {
            return String.Join(", ", arr.Select(p => p.ToString()).ToArray());
        }

        private static string WriteBytes(byte[] arr)
        {
            return String.Join(", ", arr.Select(p => p.ToString()).ToArray());
        }

        private static string WriteVec2F(Vector2F vec2) { return $"!vec2 [{WriteFloat(vec2.X)}, {WriteFloat(vec2.Y)}]"; }
        private static string WriteVec3F(Vector3F vec3) { return $"!vec3 [{WriteFloat(vec3.X)}, {WriteFloat(vec3.Y)}, {WriteFloat(vec3.Z)}]"; }
        private static string WriteVec4F(Vector4F vec4) { return $"!vec4 [{WriteFloat(vec4.X)}, {WriteFloat(vec4.Y)}, {WriteFloat(vec4.Z)}, {WriteFloat(vec4.W)}]"; }
        private static string WriteColor4F(Color4F col4) { return $"!color [{WriteFloat(col4.R)}, {WriteFloat(col4.G)}, {WriteFloat(col4.B)}, {WriteFloat(col4.A)}]"; }

        private static string WriteFloat(float f) => $"{f:0.0########}";

        #endregion
    }
}
