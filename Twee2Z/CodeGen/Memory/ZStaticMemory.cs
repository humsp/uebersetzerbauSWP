using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Tables;

namespace Twee2Z.CodeGen.Memory
{
    public class ZStaticMemory
    {
        private ZDictionaryTable _dictionaryTable;

        public ZStaticMemory()
        {
            _dictionaryTable = new ZDictionaryTable();
        }

        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[0xBFFF];

            //_dictionaryTable.ToBytes().CopyTo(byteArray, 0x0000);

            return byteArray;
        }
    }
}
