// SARC IO Pulled from EditorCore : https://github.com/exelix11/EditorCore

using Syroot.BinaryData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nintendo.Sarc.Parser
{
    internal class SARC
    {
        internal static uint StringHashToUint(string hash)
        {
            if (hash.Contains('.'))
                hash = hash.Split('.')[0];
            if (hash.Length != 8) throw new Exception("Invalid hash length");
            return Convert.ToUInt32(hash, 16);
        }

        internal static uint NameHash(string name)
        {
            uint result = 0;

            for (int i = 0; i < name.Length; i++)
                result = name[i] + result * 0x00000065;

            return result;
        }

		internal static KeyValuePair<int, byte[]> CompileSarc(SarcFile sarcFile, int _align = -1)
		{
			int align = _align >= 0 ? _align : (int)NestedFile.GuessAlignment(sarcFile.Files);

			using MemoryStream memoryStream = new();
            using BinaryStream writer = new(memoryStream, ByteConverter.GetConverter(sarcFile.Endianness), leaveOpen: false);

			writer.Write("SARC", StringCoding.Raw);
			writer.Write((ushort)0x14); // Chunk length
			writer.Write((ushort)0xFEFF); // BOM
			writer.Write((uint)0x00); //filesize update later
			writer.Write((uint)0x00); //Beginning of data
			writer.Write((ushort)0x100);
			writer.Write((ushort)0x00);
			writer.Write("SFAT", StringCoding.Raw);
			writer.Write((ushort)0xc);
			writer.Write((ushort)sarcFile.Files.Keys.Count);
			writer.Write((uint)0x00000065);

			List<uint> offsetToUpdate = new();

			//Sort files by hash
			string[] Keys = sarcFile.Files.Keys.OrderBy(x => sarcFile.HashOnly ? StringHashToUint(x) : NameHash(x)).ToArray();
			foreach (string k in Keys)
			{
				if (sarcFile.HashOnly)
					writer.Write(StringHashToUint(k));
				else
					writer.Write(NameHash(k));
				offsetToUpdate.Add((uint)writer.BaseStream.Position);
				writer.Write((uint)0);
				writer.Write((uint)0);
				writer.Write((uint)0);
			}
			writer.Write("SFNT", StringCoding.Raw);
			writer.Write((ushort)0x8);
			writer.Write((ushort)0);
			List<uint> StringOffsets = new();
			foreach (string k in Keys)
			{
				StringOffsets.Add((uint)writer.BaseStream.Position);
				writer.Write(k, StringCoding.ZeroTerminated);
				writer.Align(4);
			}
			writer.Align(0x1000); //TODO: check if works in odyssey
			List<uint> FileOffsets = new();
			foreach (string k in Keys)
			{
				writer.Align((int)NestedFile.GuessFileAlignment(sarcFile.Files[k]));
				FileOffsets.Add((uint)writer.BaseStream.Position);
				writer.Write(sarcFile.Files[k]);
			}
			for (int i = 0; i < offsetToUpdate.Count; i++)
			{
				writer.BaseStream.Position = offsetToUpdate[i];
				if (!sarcFile.HashOnly)
					writer.Write(0x01000000 | ((StringOffsets[i] - StringOffsets[0]) / 4));
				else
					writer.Write((uint)0);
				writer.Write((uint)(FileOffsets[i] - FileOffsets[0]));
				writer.Write((uint)(FileOffsets[i] + sarcFile.Files[Keys[i]].Length - FileOffsets[0]));
			}
			writer.BaseStream.Position = 0x08;
			writer.Write((uint)writer.BaseStream.Length);
			writer.Write(FileOffsets[0]);

			return new KeyValuePair<int, byte[]>(align, memoryStream.ToArray());
		}

        internal static SarcFile DecompileSarc(Stream stream)
		{
			Dictionary<string, byte[]> res = new();
            using BinaryStream br = new(stream, ByteConverter.Little, leaveOpen: false);

			// Get endienness
			br.BaseStream.Position = 6;
			if (br.ReadUInt16() == 0xFFFE)
				br.ByteConverter = ByteConverter.Big;

			// Check file header
			br.BaseStream.Position = 0;
			if (br.ReadString(4) != "SARC")
				throw new Exception($"Could not open SARC. Invalid magic '({br.ReadString(4)})'");

			br.ReadUInt16(); // Chunk length
			br.ReadUInt16(); // BOM
			br.ReadUInt32(); // File size
			uint startingOff = br.ReadUInt32();
			br.ReadUInt32(); // Unknown;

			SFAT sfat = new();
			sfat.Parse(br);

			SFNT sfnt = new();
			sfnt.Parse(br, (int)br.BaseStream.Position, sfat, (int)startingOff);

			bool HashOnly = false;
			if (sfat.NodeCount > 0)
				if (sfat.Nodes[0].FileBool != 1) HashOnly = true;

			for (int m = 0; m < sfat.NodeCount; m++)
			{
				br.Seek(sfat.Nodes[m].NodeOffset + startingOff, 0);
				byte[] temp;
				if (m == 0)
				{
					temp = br.ReadBytes((int)sfat.Nodes[m].EON);
				}
				else
				{
					int tempInt = (int)sfat.Nodes[m].EON - (int)sfat.Nodes[m].NodeOffset;
					temp = br.ReadBytes(tempInt);
				}
				if (sfat.Nodes[m].FileBool == 1)
					res.Add(sfnt.FileNames[m], temp);
				else
					// Guess file name
					res.Add(sfat.Nodes[m].Hash.ToString("X8") + NestedFile.GuessExtension(temp), temp);
			}

            stream.Dispose();
			return new SarcFile(res, br.ByteConverter.Endian, HashOnly);
		}
	}
}
