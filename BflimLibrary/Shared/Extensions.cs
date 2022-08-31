using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntxLibrary.Shared
{
    public static class Extensions
    {
        public static int ToInt(this decimal _decimal) => Convert.ToInt32(_decimal);
        public static int ToInt(this uint _uint) => Convert.ToInt32(_uint);

        public static uint ToUInt(this decimal _decimal) => Convert.ToUInt32(_decimal);
    }
}
