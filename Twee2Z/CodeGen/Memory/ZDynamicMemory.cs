using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Tables;
using Twee2Z.CodeGen.Instructions.Templates;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Bottommost part of the memory which contains dynamic stuff like the global variables. Goes from 0x0000 to 0xDFFF.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZDynamicMemory : ZComponent
    {
        private const int DynamicMemorySize = ZStaticMemory.StaticMemoryBase;

        internal const ushort InitProgramCounterAddr = 0x2000;
        internal const ushort HeaderExtensionTableAddr = 0x0040;
        internal const ushort ObjectTableAddr = 0x0048;
        internal const ushort GlobalVariablesTableAddr = 0x0048;

        private ZHeader _header;
        private ZHeaderExtension _headerExtension;
        private ZObjectTable _objectTable;
        private ZGlobalVariablesTable _globalVariablesTable;

        private Call1n _callMainRoutine;

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
            _callMainRoutine = new Call1n(0x10000);

            _subComponents.Add(_header);
            _subComponents.Add(_headerExtension);
            _subComponents.Add(_objectTable);
            _subComponents.Add(_globalVariablesTable);
            _subComponents.Add(_callMainRoutine);
        }

        public override Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[DynamicMemorySize];

            _header.ToBytes().CopyTo(byteArray, 0x0000);
            _headerExtension.ToBytes().CopyTo(byteArray, 0x0040);
            
            _objectTable.ToBytes().CopyTo(byteArray, ObjectTableAddr);
            _globalVariablesTable.ToBytes().CopyTo(byteArray, GlobalVariablesTableAddr);

            // Jumps to the beginning of the high memory for the main routine.
            _callMainRoutine.ToBytes().CopyTo(byteArray, 0x2000);

            return byteArray;
        }

        public override int Size { get { return DynamicMemorySize; } }
    }
}
