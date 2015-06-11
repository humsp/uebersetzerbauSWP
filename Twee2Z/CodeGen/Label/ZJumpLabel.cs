using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Instruction;

namespace Twee2Z.CodeGen.Label
{
    class ZJumpLabel : ZLabel
    {
        IZComponent _sourceComponent;

        public ZJumpLabel(string name)
            : base(name)
        {
        }

        public ZJumpLabel(string name, ZAddress address)
            : base(address, name)
        {
        }

        public IZComponent SourceComponent
        {
            get
            {
                return _sourceComponent;
            }
            set
            {
                _sourceComponent = value;
            }
        }

        protected override void SetLabel(int absoluteAddr, string name)
        {
            base.SetLabel(absoluteAddr, name);
        }

        public short Offset
        {
            get
            {
                // Offset is the target address minus the address of this label
                return (short)(TargetAddress.Absolute - SourceComponent.Label.TargetAddress.Absolute - SourceComponent.Size);
            }
        }

        public override byte[] ToBytes()
        {
            if (_targetAddress == null)
                throw new Exception("Cannot convert a ZLabel into Z-Code before the TargetAddress is set.");

            byte[] byteArray = new byte[2];

            short value = (short)(Offset + 2);
            byteArray[0] = (byte)(value >> 8);
            byteArray[1] = (byte)value;
            
            return byteArray;
        }
    }
}
