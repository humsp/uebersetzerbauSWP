using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Instruction
{
    [DebuggerDisplay("LabelName = {_label.Name}, LocalVariableCount = {_localVariableCount}, InstructionCount = {_subComponents.Count}")]
    class ZRoutine : ZLabeledComponent
    {
        protected byte _localVariableCount;

        public ZRoutine()
        {
        }

        public ZRoutine(IEnumerable<ZInstruction> instructions)
        {
            _subComponents.AddRange(instructions);
        }

        public ZRoutine(IEnumerable<ZInstruction> instructions, byte localVariableCount)
            : this()
        {
            _subComponents.AddRange(instructions);
            _localVariableCount = localVariableCount;
        }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.Add(_localVariableCount);

            foreach (ZInstruction instruction in _subComponents)
            {
                byteList.AddRange(instruction.ToBytes());
            }

            // Routines use packed addresses. Therefore additionial bytes might have to be added for padding.
            byteList.AddRange(new byte[8 - byteList.Count % 8]);

            return byteList.ToArray();
        }

        public override int Size
        {
            get
            {
                // One byte for the local variable count + all instructions
                int size = 1 + _subComponents.Sum(component => component.Size);
                // Plus additional padding if needed
                return size + (8 - size % 8);
            }
        }

        public override void SetLabel(int absoluteAddr, string name)
        {
            if (_label == null)
                _label = new ZLabel(name, new ZPackedAddress(absoluteAddr));
            else if (_label.TargetAddress == null)
            {
                _label.TargetAddress = new ZPackedAddress(absoluteAddr);
                _label.Name = name;
            }
            else
            {
                _label.TargetAddress.Absolute = absoluteAddr;
                _label.Name = name;
            }
        }

        public override void Setup(int currentAddress)
        {
            base.Setup(currentAddress + 1); // dont forget the single byte (local variables counter)
        }
    }
}
