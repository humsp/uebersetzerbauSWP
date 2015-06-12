using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction
{
    /// <summary>
    /// Represents an instruction (opcode + operands) in Z-Code. It can be labeled for jumping in Z-Code execution.
    /// See also "4. How instructions are encoded" on page 26 for reference.
    /// See also "14. Complete table of opcodes" on page 70 for reference.
    /// </summary>
    [DebuggerDisplay("Opcode = {_opcode}, Operands = {_operands}")]
    class ZInstruction : ZLabeledComponent
    {
        protected ZOpcode _opcode;
        protected ZOperand[] _operands;

        /// <summary>
        /// Creates a new instance of a ZInstruction. Check the complete table of opcodes for valid values.
        /// </summary>
        /// <param name="name">The Inform name for the used opcode. It is used for debugging purposes only, so you COULD leave this empty or null.</param>
        /// <param name="opcodeNumer">The opcode number.</param>
        /// <param name="opcodeType">The opcode type.</param>
        /// <param name="operands">The operands to use.</param>
        public ZInstruction(string name, byte opcodeNumber, OpcodeTypeKind opcodeType, params ZOperand[] operands)
        {
            var result = OpcodeHelper.GetFormAndCount(opcodeNumber, opcodeType, operands);
            _opcode = new ZOpcode(name, opcodeNumber, result.Item1, result.Item2, operands.Select(o => o.OperandType).ToArray());
            _subComponents.Add(_opcode);

            _operands = operands;
            _subComponents.AddRange(operands);
        }

        public ZOpcode Opcode { get { return _opcode; } }

        public ZOperand[] Operands { get { return _operands; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            // Add the opcode
            byteList.AddRange(_opcode.ToBytes());

            // Add the operands
            foreach (ZOperand item in _operands)
            {
                byteList.AddRange(item.ToBytes());
            }

            return byteList.ToArray();
        }
    }
}
