using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Opcode
{
    /// <summary>
    /// Represents the opcode, the first part of an instruction, in Z-Code. It contains a name and number (matching the entries in the opcode table) and further information to generate a valid opcode.
    /// See also "4. How instructions are encoded" on page 26 for reference.
    /// See also "14. Complete table of opcodes" on page 70 for reference.
    /// </summary>
    [DebuggerDisplay("Name = {_name}, Number = {_opcodeNumber}, InstructionForm = {_instructionForm}, OperandCount = {_operandCount}")]
    class ZOpcode : ZComponent
    {
        protected string _name;
        protected byte _opcodeNumber;
        protected InstructionFormKind _instructionForm;
        protected InstructionOperandCountKind _operandCount;
        protected OperandTypeKind[] _operandTypes;

        /// <summary>
        /// Creates a new instance of a ZOpcode. Check the complete table of opcodes for valid values.
        /// </summary>
        /// <param name="name">The Inform name for this opcode. It is used for debugging purposes only, so you COULD leave this empty or null.</param>
        /// <param name="opcodeNumer">The opcode number.</param>
        /// <param name="instructionForm"></param>
        /// <param name="operandCount"></param>
        /// <param name="operandTypes"></param>
        public ZOpcode(string name, byte opcodeNumer, InstructionFormKind instructionForm, InstructionOperandCountKind operandCount, IEnumerable<OperandTypeKind> operandTypes)
        {
            _name = name;
            _opcodeNumber = opcodeNumer;
            _instructionForm = instructionForm;
            _operandCount = operandCount;
            _operandTypes = operandTypes.ToArray();
        }

        /// <summary>
        /// The Inform name for this opcode.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// The opcode number.
        /// </summary>
        public byte Number { get { return _opcodeNumber; } }

        public InstructionFormKind InstructionForm { get { return _instructionForm; } }

        public InstructionOperandCountKind OperandCount { get { return _operandCount; } }

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
                // We have to measure this opcode with a helper class
                return OpcodeHelper.MeasureOpcodeSize(_instructionForm, _operandCount, _operandTypes);
            }
        }
    }
}
