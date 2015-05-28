using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Tables;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Memory between dynamic and high. Contains static stuff like the dictionary table. Goes from 0x4000 to 0xFFFE.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZStaticMemory : ZComponentBase
    {
        private const int StaticMemorySize = ZHighMemory.HighMemoryBase - ZStaticMemory.StaticMemoryBase;

        internal const ushort StaticMemoryBase = 0x4000;
        internal const ushort DictionaryTableAddr = 0x0048;
        internal const ushort AbbreviationTableAddr = 0x0048;

        private ZDictionaryTable _dictionaryTable;
        private ZAbbreviationTable _abbreviationTable;

        public ZStaticMemory()
        {
            _dictionaryTable = new ZDictionaryTable();
            _abbreviationTable = new ZAbbreviationTable();
        }

        public override Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[StaticMemorySize];

            _dictionaryTable.ToBytes().CopyTo(byteArray, DictionaryTableAddr);
            _abbreviationTable.ToBytes().CopyTo(byteArray, AbbreviationTableAddr);

            return byteArray;
        }

        public override int Size { get { return StaticMemorySize; } }
    }
}
