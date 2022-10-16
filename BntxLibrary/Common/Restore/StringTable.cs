using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BntxLibrary.Core;

namespace BntxLibrary.Common.Restore
{
    public class StringTable
    {
        public SortedDictionary<long, string> Strings { get; set; } = new();

        public void Load(BntxFileLoader loader)
        {
            loader.CheckSignature("_STR");
            uint blockOffset = loader.ReadUInt32();
            long BlockSize = loader.ReadInt64();
            uint StringCount = loader.ReadUInt32();
        }

        public void Save(BntxFileWriter writer)
        {

        }
    }
}
