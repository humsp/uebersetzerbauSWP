using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Tables;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Bottommost part of the memory which contains dynamic stuff like the global variables. Goes from 0x0000 to 0xDFFF.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZDynamicMemory : IZComponent
    {
        internal const ushort InitProgramCounterAddr = 0x2000;
        internal const ushort HeaderExtensionTableAddr = 0x0040;
        internal const ushort ObjectTableAddr = 0x0048;
        internal const ushort GlobalVariablesTableAddr = 0x0048;

        private ZHeader _header;
        private ZHeaderExtension _headerExtension;
        private ZObjectTable _objectTable;
        private ZGlobalVariablesTable _globalVariablesTable;

        public ZDynamicMemory()
        {
            _header = new ZHeader(ZStaticMemory.StaticMemoryBase,
                ZHighMemory.HighMemoryBase,
                InitProgramCounterAddr,
                ZStaticMemory.DictionaryTableAddr,
                ObjectTableAddr,
                GlobalVariablesTableAddr,
                HeaderExtensionTableAddr);
            _headerExtension = new ZHeaderExtension();
            _objectTable = new ZObjectTable();
            _globalVariablesTable = new ZGlobalVariablesTable();
        }

        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[ZStaticMemory.StaticMemoryBase];

            _header.ToBytes().CopyTo(byteArray, 0x0000);
            _headerExtension.ToBytes().CopyTo(byteArray, 0x0040);


            _objectTable.ToBytes().CopyTo(byteArray, ObjectTableAddr);
            _globalVariablesTable.ToBytes().CopyTo(byteArray, GlobalVariablesTableAddr);

            // Jumps to the beginning of the high memory for the first routine.
            byteArray[0x2000] = 0x0F | 0x80;
            byteArray[0x2001] = (Byte)(0x2000 >> 8);
            byteArray[0x2002] = (Byte)0x00;

            return byteArray;
        }
    }
}
