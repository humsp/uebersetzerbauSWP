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
    /// Jump if a = 0.
    /// <para>
    /// See also "jz" on page 87 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, A = {_operands[0].Value}, Branch = {_branch}")]
    class Jz : ZInstructionBr
    {
        private Jz(ZBranchLabel branchLabel, params ZOperand[] operands)
            : base("jz", 0x00, OpcodeTypeKind.OneOP, branchLabel, operands)
        {
        }

        public Jz(byte a, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a))
        {
        }

        public Jz(short a, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a))
        {
        }

        public Jz(ZVariable a, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a))
        {
        }
    }
}
