using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Instruction
{
    [DebuggerDisplay("Name = {_opcode.Name}, OpcodeNumber = {_opcode.OpcodeNumber}, InstructionForm = {_opcode.InstructionForm}, OperandCount = {_opcode.OperandCount}", Name = "{_opcode.Name}")]
    class ZInstruction : ZComponent
    {
        protected ZOpcode _opcode;

        public ZInstruction(string name, byte opcodeNumber, InstructionFormKind instructionForm, OperandCountKind operandCount, params OperandTypeKind[] operandTypes)
        {
            _opcode = new ZOpcode(name, opcodeNumber, instructionForm, operandCount, operandTypes);
            _subComponents.Add(_opcode);
        }

        public string Name { get { return _opcode.Name; } }

        public InstructionFormKind InstructionForm { get { return _opcode.InstructionForm; } }

        public OperandCountKind OperandCount { get { return _opcode.OperandCount; } }

        public ZOpcode Opcode { get { return _opcode; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(_opcode.ToBytes());
            
            //TODO: Implement generic support of operands

            return byteList.ToArray();
        }
    }
}
