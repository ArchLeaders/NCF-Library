namespace Nintendo.Sarc.Parser
{
    internal static class NestedFile
    {
		private static Dictionary<string, string> FileExtensions { get; } = new()
        {
			{ "AAHS", ".sharc" },
			{ "AAMP", ".aamp" },
			{ "BAHS", ".sharcb" },
			{ "BNSH", ".bnsh" },
			{ "BNTX", ".bntx" },
			{ "BY", ".byaml" },
			{ "CFNT", ".bcfnt" },
			{ "CGFX", ".bcres" },
			{ "CLAN", ".bclan" },
			{ "CLYT", ".bclyt" },
			{ "CSTM", ".bcstm" },
			{ "CTPK", ".ctpk" },
			{ "CWAV", ".bcwav" },
			{ "FFNT", ".bffnt" },
			{ "FLAN", ".bflan" },
			{ "FLIM", ".bclim" },
			{ "FLYT", ".bflyt" },
			{ "FRES", ".bfres" },
			{ "FSEQ", ".bfseq" },
			{ "FSHA", ".bfsha" },
			{ "FSTM", ".bfstm" },
			{ "FWAV", ".bfwav" },
			{ "Gfx2", ".gtx" },
			{ "MsgPrjBn", ".msbp" },
			{ "MsgStdBn", ".msbt" },
			{ "SARC", ".sarc" },
			{ "STM", ".bfsha" },
			{ "VFXB", ".pctl" },
			{ "Yaz", ".szs" },
			{ "YB", ".byaml" },
		};

		private static Dictionary<string, uint> FileAlignments { get; } = new()
		{
			{ "SARC", 0x2000 },
			{ "Yaz", 0x80 },
			{ "YB", 0x80 },
			{ "BY", 0x80 },
			{ "FRES", 0x2000 },
			{ "Gfx2", 0x2000 },
			{ "AAHS", 0x2000 },
			{ "BAHS", 0x2000 },
			{ "BNTX", 0x1000 },
			{ "BNSH", 0x1000 },
			{ "FSHA", 0x1000 },
			{ "FFNT", 0x2000 },
			{ "CFNT", 0x80 },
			{ "-STM", 0x20 },
			{ "-WAV", 0x20 },
			{ "FSTP", 0x20 },
			{ "CTPK", 0x10 },
			{ "CGFX", 0x80 },
			{ "AAMP", 8 },
			{ "MsgStdBn", 0x80 },
			{ "MsgPrjBn", 0x80 },
		};

		internal static string GuessExtension(byte[] file)
        {
			// Set default extension
			string guessedExtension = ".bin";

			// Iterate header default extensions
            foreach (var ext in FileExtensions)
				if (file.Matches(ext.Key))
					guessedExtension = ext.Value;

			return guessedExtension;
        }

		internal static uint GuessAlignment(Dictionary<string, byte[]> files)
		{
			uint res = 4;
			foreach (var file in files.Values)
			{
				uint fileRes = GuessFileAlignment(file);
				res = fileRes > res ? fileRes : res;
			}
			return res;
		}

		internal static uint GuessFileAlignment(byte[] file)
		{
			// Set default alignment
			uint guessedAlignment = 0x04;

			// Iterate header alignments
			foreach (var ext in FileAlignments)
            {
				uint offset = 0;
				if (ext.Key.StartsWith("-"))
					offset = 1;

				if (file.Matches(offset, ext.Key))
					guessedAlignment = ext.Value;
            }

			return guessedAlignment;
		}


		internal static bool Matches(this byte[] arr, string magic) => arr.Matches(0, magic.ToCharArray());
		internal static bool Matches(this byte[] arr, uint startIndex, string magic) => arr.Matches(startIndex, magic.ToCharArray());
		internal static bool Matches(this byte[] arr, uint startIndex, params char[] magic)
		{
			if (arr.Length < magic.Length + startIndex) return false;
			for (uint i = 0; i < magic.Length; i++)
			{
				if (arr[i + startIndex] != magic[i]) return false;
			}
			return true;
		}
	}
}
