using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instruction.Opcode
{
    /// <summary>
    /// Determines how the operands are encoded into the opcode.
    /// See also "4.3 Form and operand count" on page 27 for reference.
    /// </summary>
    enum InstructionOperandCountKind
    {
        /// <summary>
        /// Zero operands.
        /// </summary>
        ZeroOP,
        /// <summary>
        /// One operand.
        /// </summary>
        OneOP,
        /// <summary>
        /// Two operands.
        /// </summary>
        TwoOP,
        /// <summary>
        /// Multiple operands (up to four). Up to eight operands are posible but not supported yet (only call_vs2 and call_vn2 make use of this)."
        /// </summary>
        Var
    }
}
