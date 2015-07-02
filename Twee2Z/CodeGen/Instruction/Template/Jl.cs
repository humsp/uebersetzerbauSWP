using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Jump if a > b (using a signed 16-bit comparison).
    /// <para>
    /// See also "jg" on page 87 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, A = {_operands[0].Value}, B = {_operands[1].Value}, Branch = {_branch}")]
    class Jg : ZInstructionBr
    {
        private Jg(ZBranchLabel branchLabel, params ZOperand[] operands)
            : base("jg", 0x03, OpcodeTypeKind.TwoOP, branchLabel, operands)
        {
        }

        public Jg(short a, short b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Jg(short a, ZVariable b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Jg(ZVariable a, short b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Jg(ZVariable a, ZVariable b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }
    }
}
