using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Tables;
using Twee2Z.CodeGen.Instructions.Templates;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Bottommost part of the memory which contains dynamic stuff like the global variables. Goes from 0x0000 to 0xDFFF.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZDynamicMemory : ZComponent
    {
        private const int DynamicMemorySize = ZStaticMemory.StaticMemoryBase;

        internal const ushort HeaderTableAddr = 0x0000;
        internal const ushort HeaderExtensionTableAddr = 0x0040;
        internal const ushort ObjectTableAddr = 0x0048;
        internal const ushort GlobalVariablesTableAddr = 0x0048;
        internal const ushort InitProgramCounterAddr = 0x2000;

        private ZHeader _header;
        private ZHeaderExtension _headerExtension;
        private ZObjectTable _objectTable;
        private ZGlobalVariablesTable _globalVariablesTable;

        private Call1n _mainRoutineCall;

        public ZDynamicMemory()
        {
            _header = new ZHeader(ZStaticMemory.StaticMemoryBase,
                ZHighMemory.HighMemoryBase,
                InitProgramCounterAddr,
                ZStaticMemory.DictionaryTableAddr,
                ObjectTableAddr,
                GlobalVariablesTableAddr,
                HeaderExtensionTableAddr) { Label = new ZLabel(new ZByteAddress(HeaderTableAddr)) };
            _headerExtension = new ZHeaderExtension() { Label = new ZLabel(new ZByteAddress(HeaderExtensionTableAddr)) };
            _objectTable = new ZObjectTable() { Label = new ZLabel(new ZByteAddress(ObjectTableAddr)) };
            _globalVariablesTable = new ZGlobalVariablesTable() { Label = new ZLabel(new ZByteAddress(GlobalVariablesTableAddr)) };
            _mainRoutineCall = new Call1n(new ZRoutineLabel("main")) { Label = new ZLabel(new ZByteAddress(InitProgramCounterAddr)) };

            _subComponents.Add(_header);
            _subComponents.Add(_headerExtension);
            _subComponents.Add(_objectTable);
            _subComponents.Add(_globalVariablesTable);
            _subComponents.Add(_mainRoutineCall);
        }

        public override Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[Size];

            _header.ToBytes().CopyTo(byteArray, _header.Label.TargetAddress.Absolute);
            _headerExtension.ToBytes().CopyTo(byteArray, _headerExtension.Label.TargetAddress.Absolute);

            _objectTable.ToBytes().CopyTo(byteArray, _objectTable.Label.TargetAddress.Absolute);
            _globalVariablesTable.ToBytes().CopyTo(byteArray, _globalVariablesTable.Label.TargetAddress.Absolute);

            // Jumps to the beginning of the high memory for the main routine.
            _mainRoutineCall.ToBytes().CopyTo(byteArray, _mainRoutineCall.Label.TargetAddress.Absolute);

            return byteArray;
        }

        public override int Size { get { return DynamicMemorySize; } }

        protected override void SetLabel(int absoluteAddr, string name)
        {
            if (_componentLabel == null)
                _componentLabel = new ZLabel(new ZByteAddress(absoluteAddr), name);
            else if (_componentLabel.TargetAddress == null)
            {
                _componentLabel.TargetAddress = new ZByteAddress(absoluteAddr);
                _componentLabel.Name = name;
            }
            else
            {
                _componentLabel.TargetAddress.Absolute = absoluteAddr;
                _componentLabel.Name = name;
            }
        }
    }
}
