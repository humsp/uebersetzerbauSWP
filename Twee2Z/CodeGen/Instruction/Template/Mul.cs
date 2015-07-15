using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Signed 16-bit multiplication.
    /// <para>
    /// See also "mul" on page 89 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, A = {_operands[0].Value}, B = {_operands[1].Value}, Store = {_store}")]
    class Mul : ZInstructionSt
    {
        private Mul(ZVariable store, params ZOperand[] operands)
            : base("mul", 0x16, OpcodeTypeKind.TwoOP, store, operands)
        {
        }

        /// <summary>
        /// Creates a new instance of a Mul instruction.
        /// </summary>
        public Mul(short a, short b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Mul instruction.
        /// </summary>
        public Mul(short a, ZVariable b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Mul instruction.
        /// </summary>
        public Mul(ZVariable a, short b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Mul instruction.
        /// </summary>
        public Mul(ZVariable a, ZVariable b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }
    }
}
