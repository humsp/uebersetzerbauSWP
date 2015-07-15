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
    /// Signed 16-bit division. Division by zero should halt the interpreter with a suitable error message.
    /// <para>
    /// See also "div" on page 83 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, A = {_operands[0].Value}, B = {_operands[1].Value}, Store = {_store}")]
    class Div : ZInstructionSt
    {
        private Div(ZVariable store, params ZOperand[] operands)
            : base("div", 0x17, OpcodeTypeKind.TwoOP, store, operands)
        {
        }

        /// <summary>
        /// Creates a new instance of a Div instruction.
        /// </summary>
        public Div(short a, short b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Div instruction.
        /// </summary>
        public Div(short a, ZVariable b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Div instruction.
        /// </summary>
        public Div(ZVariable a, short b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Div instruction.
        /// </summary>
        public Div(ZVariable a, ZVariable b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }
    }
}
