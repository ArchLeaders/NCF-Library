using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nintendo.Bfres
{
    interface IBinarySection
    {
        void Import(string filePath, BfresFile resFile);
        void Export(string filePath, BfresFile resFile);
    }
}
