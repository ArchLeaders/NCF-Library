using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntxLibrary.Shared.AddressLibrary
{
    internal class TileInfo
    {
        internal uint Banks { get; set; } = 0;
        internal uint BankWidth { get; set; } = 0;
        internal uint BankHeight { get; set; } = 0;
        internal uint MacroAspectRatio { get; set; } = 0;
        internal uint TileSplitBytes { get; set; } = 0;
        internal uint PipeConfig { get; set; } = 0;
    }
}
