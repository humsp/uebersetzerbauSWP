using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Instructions
{
    [DebuggerDisplay("OperandCount = {OperandCount}", Name = "{Name}")]
    class ZInstruction : ZComponentBase
    {
        protected ZOpcode _opcode;
        protected InstructionFormKind _instructionForm;

        public ZInstruction(string name, ushort hex, OperandCountKind operandCount, InstructionFormKind instructionForm)
        {
            _opcode = new ZOpcode(name, hex, operandCount);
            _subComponents.Add(_opcode);

            _instructionForm = instructionForm;
        }

        public string Name { get { return _opcode.Name; } }
        public OperandCountKind OperandCount { get { return _opcode.OperandCount; } }
        public InstructionFormKind InstructionForm { get { return _instructionForm; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(_opcode.ToBytes());
            
            //TODO: Implement generic support of operands

            return byteList.ToArray();
        }
    }
}
