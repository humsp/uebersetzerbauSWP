using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Instruction
{
    [DebuggerDisplay("OpcodeNumber = {_opcodeNumber}, InstructionForm = {_instructionForm}, OperandCount = {_operandCount}", Name = "{_name}")]
    class ZOpcode : ZComponent
    {
        protected string _name;
        protected byte _opcodeNumber;
        protected InstructionFormKind _instructionForm;
        protected OperandCountKind _operandCount;
        protected OperandTypeKind[] _operandTypes;

        public ZOpcode(string name, byte opcodeNumer, InstructionFormKind instructionForm, OperandCountKind operandCount, params OperandTypeKind[] operandTypes)
        {
            _name = name;

            // Validate the arguments by calling this method (if something is fishy, it will throw an exception)
            OpcodeHelper.ToOpcode(opcodeNumer, instructionForm, operandCount, operandTypes);

            _opcodeNumber = opcodeNumer;
            _instructionForm = instructionForm;
            _operandCount = operandCount;
            _operandTypes = operandTypes;
        }

        public string Name { get { return _name; } }

        public byte OpcodeNumber { get { return _opcodeNumber; } }

        public InstructionFormKind InstructionForm { get { return _instructionForm; } }

        public OperandCountKind OperandCount { get { return _operandCount; } }

        public OperandTypeKind[] OperandTypes { get { return _operandTypes; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(OpcodeHelper.ToOpcode(_opcodeNumber, _instructionForm, _operandCount, _operandTypes));
            
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
