using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen.Label
{
    class ZJumpLabel : ZLabel
    {
        private ZAddress _sourceAddress = null;

        public ZJumpLabel(string name)
            : base(name)
        {
        }

        public ZJumpLabel(string name, ZAddress address)
            : base(address, name)
        {
        }

        public ZAddress SourceAddress
        {
            get
            {
                return _sourceAddress;
            }
            set
            {
                _sourceAddress = value;
            }
        }

        public short Offset
        {
            get
            {
                // Offset is the target address minus the address of this label
                return (short)(TargetAddress.Absolute - SourceAddress.Absolute);
            }
        }

        public override byte[] ToBytes()
        {
            if (_targetAddress == null)
                throw new Exception("Cannot convert a ZLabel into Z-Code before the TargetAddress is set.");

            byte[] byteArray = new byte[2];
            byteArray[0] = (byte)(Offset >> 8);
            byteArray[1] = (byte)Offset;
            return byteArray;
        }
    }
}
