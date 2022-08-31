using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntxLibrary.Shared.AddressLibrary
{
    internal class TileInfo
    {
        internal int Banks { get; set; } = 0;
        internal int BankWidth { get; set; } = 0;
        internal int BankHeight { get; set; } = 0;
        internal int MacroAspectRatio { get; set; } = 0;
        internal int TileSplitBytes { get; set; } = 0;
        internal int PipeConfig { get; set; } = 0;
    }
}
