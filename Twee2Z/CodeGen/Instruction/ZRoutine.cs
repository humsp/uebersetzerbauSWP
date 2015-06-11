﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Instruction
{
    [DebuggerDisplay("Name = {_componentLabel.Name}, LocalVariableCount = {_localVariableCount}, InstructionCount = {_subComponents.Count}", Name = "{_componentLabel.Name}")]
    class ZRoutine : ZComponent
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

        protected override void SetLabel(int absoluteAddr, string name)
        {
            if (_componentLabel == null)
                _componentLabel = new ZLabel(new ZPackedAddress(absoluteAddr), name);
            else if (_componentLabel.TargetAddress == null)
            {
                _componentLabel.TargetAddress = new ZPackedAddress(absoluteAddr);
                _componentLabel.Name = name;
            }
            else
            {
                _componentLabel.TargetAddress.Absolute = absoluteAddr;
                _componentLabel.Name = name;
            }
        }

        protected override void Setup(int currentAddress)
        {
            base.Setup(currentAddress + 1); // dont forget the single byte (local variables counter)
        }
    }
}
