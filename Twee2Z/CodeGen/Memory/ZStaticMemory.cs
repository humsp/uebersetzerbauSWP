using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Table;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Memory between dynamic and high. Contains static stuff like the dictionary table. Goes from 0x4000 to 0xFFFE.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZStaticMemory : ZComponent
    {
        internal const ushort StaticMemoryBase = 0x4000;
        internal const ushort DictionaryTableAddr = 0x0048;
        internal const ushort AbbreviationTableAddr = 0x0048;

        private ZDictionaryTable _dictionaryTable;
        private ZAbbreviationTable _abbreviationTable;

        public ZStaticMemory()
        {
            _dictionaryTable = new ZDictionaryTable() { Label = new ZLabel(new ZByteAddress(DictionaryTableAddr)) };
            _abbreviationTable = new ZAbbreviationTable() { Label = new ZLabel(new ZByteAddress(AbbreviationTableAddr)) };

            _subComponents.Add(_dictionaryTable);
            _subComponents.Add(_abbreviationTable);
        }

        public override Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[Size];

            _dictionaryTable.ToBytes().CopyTo(byteArray, _dictionaryTable.Label.TargetAddress.Absolute);
            _abbreviationTable.ToBytes().CopyTo(byteArray, _abbreviationTable.Label.TargetAddress.Absolute);

            return byteArray;
        }

        public override int Size
        {
            get
            {
                return ZMemory.HighMemoryAddr - Label.TargetAddress.Absolute;
            }
        }

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
