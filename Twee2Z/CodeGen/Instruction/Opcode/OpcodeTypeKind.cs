using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instruction.Opcode
{
    /// <summary>
    /// Determines the opcode type. This is an abstraction of the compolete table of opcodes in the specification document.
    /// With this opcode type, an opcode number and the given operands, the OpcodeHelper can create a valid opcode in Z-Code.
    /// That is why you should not be bothered with InstructionFormKind and InstructionOperandCountKind when defining and using instructions.
    /// See also "14. Complete table of opcodes" on page 70 for reference.
    /// </summary>
    enum OpcodeTypeKind
    {
        /// <summary>
        /// Zero-operand opcodes 0OP
        /// </summary>
        ZeroOP,
        /// <summary>
        /// One-operand opcodes 1OP
        /// </summary>
        OneOP,
        /// <summary>
        /// Two-operand opcodes 2OP
        /// </summary>
        TwoOP,
        /// <summary>
        /// Variable-operand opcodes VAR
        /// </summary>
        Var,
        /// <summary>
        /// Extended opcodes EXT
        /// </summary>
        Ext
    }
}
