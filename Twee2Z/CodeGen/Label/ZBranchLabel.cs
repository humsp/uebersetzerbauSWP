using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Instruction;

namespace Twee2Z.CodeGen.Label
{
    class ZBranchLabel : ZLabel
    {
        IZComponent _sourceComponent;
        bool _branchOn = true;
        bool? _routineReturnValue;

        public ZBranchLabel(string name)
            : base(name)
        {
        }

        public ZBranchLabel(string name, ZAddress address)
            : base(address, name)
        {
        }

        public ZBranchLabel(bool routineReturnValue)
            : base((string)null)
        {
            _routineReturnValue = routineReturnValue;
        }

        public IZComponent SourceComponent
        {
            get
            {
                return _sourceComponent;
            }
            set
            {
                if (TargetAddress.Absolute - value.Label.TargetAddress.Absolute - value.Size < 0)
                    throw new ArgumentException("ZBranchLabels cannot jump backwards.", "absoluteAddr");

                if (TargetAddress.Absolute - value.Label.TargetAddress.Absolute - value.Size > 16383)
                    throw new ArgumentException("The jump distance of ZBranchLabels has to be between 0 and 16383.", "absoluteAddr");

                _sourceComponent = value;
            }
        }

        public bool BranchOn
        {
            get
            {
                return _branchOn;
            }
            set
            {
                _branchOn = value;
            }
        }

        public bool? RoutineReturnValue
        {
            get
            {
                return _routineReturnValue;
            }
            set
            {
                _routineReturnValue = value;
            }
        }

        public short Offset
        {
            get
            {
                if (_routineReturnValue == false)
                    return -2;
                else if (_routineReturnValue == true)
                    return -1;

                // Offset is the target address minus the address of this label
                else
                    return (short)(TargetAddress.Absolute - SourceComponent.Label.TargetAddress.Absolute - SourceComponent.Size);
            }
        }

        public override byte[] ToBytes()
        {
            if (_targetAddress == null)
                throw new Exception("Cannot convert a ZLabel into Z-Code before the TargetAddress is set.");

            List<byte> byteList = new List<byte>();

            short value;

            if (_routineReturnValue == false)
                value = 0;
            else if (_routineReturnValue == true)
                value = 1;
            else
                value = (short)(Offset + 2);
            
            if (Offset <= 63)
            {
                byte byteVal = (byte)value;
                
                if (BranchOn == true)
                    byteVal |= 0x80;

                byteVal |= 0x40;

                byteList.Add(byteVal);
            }
            else
            {
                byte byteVal1 = (byte)(value >> 8);
                byte byteVal2 = (byte)value;

                if (BranchOn == true)
                    byteVal1 |= 0x80;
                
                byteList.Add(byteVal1);
                byteList.Add(byteVal2);
            }

            return byteList.ToArray();
        }
    }
}
