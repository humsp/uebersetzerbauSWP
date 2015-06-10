using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Instruction
{
    [DebuggerDisplay("OpcodeNumber = {_hex}, InstructionForm = {_instructionForm}, OperandCount = {_operandCount}", Name = "{_name}")]
    class ZOpcode : ZComponent
    {
        protected string _name;
        protected byte _opcodeNumber;
        protected InstructionFormKind _instructionForm;
        protected OperandCountKind _operandCount;

        public ZOpcode(string name, byte opcodeNumer, InstructionFormKind instructionForm, OperandCountKind operandCount)
        {
            _name = name;

            // Validate the arguments by calling this method (if something is fishy, it will throw an exception)
            OpcodeHelper.ToOpcode(opcodeNumer, instructionForm, operandCount);

            _opcodeNumber = opcodeNumer;
            _instructionForm = instructionForm;
            _operandCount = operandCount;
        }

        public string Name { get { return _name; } }

        public byte OpcodeNumber { get { return _opcodeNumber; } }

        public InstructionFormKind InstructionForm { get { return _instructionForm; } }

        public OperandCountKind OperandCount { get { return _operandCount; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            ushort opcode = OpcodeHelper.ToOpcode(_opcodeNumber, _instructionForm, _operandCount);

            if (opcode > 0xFF)
            {
                byteList.Add((Byte)(opcode >> 8));
                byteList.Add((Byte)opcode);
            }
            else
            {
                byteList.Add((byte)opcode);
            }

            //TODO: Implement generic support of operands

            return byteList.ToArray();
        }

        public override int Size
        {
            get
            {
                return OpcodeHelper.MeasureOpcodeSize(_instructionForm);
            }
        }
    }
}
