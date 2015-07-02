using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instruction.Opcode
{
    /// <summary>
    /// Determines the form and size of an opcode.
    /// See also "4.3 Form and operand count" on page 27 for reference.
    /// </summary>
    enum InstructionFormKind
    {
        /// <summary>
        /// This opcode is one byte in size and encodes zero or one operand(s) only. The opcode number must be in range of 0x0 - 0xF.
        /// </summary>
        Short,
        /// <summary>
        /// This opcode is one byte in size and encodes two operands only. The opcode number must be in range of 0x00 - 0x1F.
        /// </summary>
        Long,
        /// <summary>
        /// This opcode is two bytes in size and encodes two or multiple operands only. The opcode number must be in range of 0x00 - 0x1F.
        /// </summary>
        Variable,
        /// <summary>
        /// This opcode is three bytes in size and encodes multiple operands only. The opcode number must be in range of 0x00 - 0xFF.
        /// </summary>
        Extended
    }
}
