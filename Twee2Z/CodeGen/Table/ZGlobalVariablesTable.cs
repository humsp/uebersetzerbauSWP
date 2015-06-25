using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Table
{
    class ZGlobalVariablesTable : ZComponent
    {
        // The global variables table consists of 240 2-byte words
        private const int GlobalVariablesTableSize = 240 * 2;

        public override int Size { get { return GlobalVariablesTableSize; } }

        public override byte[] ToBytes()
        {
            return new byte[GlobalVariablesTableSize];
        }
    }
}
