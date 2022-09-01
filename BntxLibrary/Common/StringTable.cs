using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BntxLibrary.Core;

namespace BntxLibrary.Common
{
    public class StringTable
    {
        public SortedDictionary<long, string> Strings = new SortedDictionary<long, string>();
        public void Load(BntxFileLoader loader)
        {
            loader.CheckSignature("_STR");
            uint blockOffset = loader.ReadUInt32();
            long BlockSize = loader.ReadInt64();
            uint StringCount = loader.ReadUInt32();
        }

        public void Save(BntxFileSaver saver)
        {

        }
    }
}
