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
    /// Jump if the two operands equal.
    /// <para>
    /// See also "je" on page 86 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Branch = {_branch}")]
    class Je : ZInstructionBr
    {
        private Je(ZBranchLabel branchLabel, params ZOperand[] operands)
            : base("je", 0x01, OpcodeTypeKind.TwoOP, branchLabel, operands)
        {
        }

        public Je(byte a, byte b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Je(byte a, short b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Je(byte a, ZVariable b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Je(short a, byte b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Je(short a, short b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Je(short a, ZVariable b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Je(ZVariable a, byte b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Je(ZVariable a, short b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }

        public Je(ZVariable a, ZVariable b, ZBranchLabel branchLabel)
            : this(branchLabel, new ZOperand(a), new ZOperand(b))
        {
        }
    }
}
