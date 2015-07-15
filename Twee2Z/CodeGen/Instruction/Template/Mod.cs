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
    /// Remainder after signed 16-bit division. Division by zero should halt the interpreter with a suitable error message.
    /// <para>
    /// See also "mod" on page 88 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, A = {_operands[0].Value}, B = {_operands[1].Value}, Store = {_store}")]
    class Mod : ZInstructionSt
    {
        private Mod(ZVariable store, params ZOperand[] operands)
            : base("mod", 0x18, OpcodeTypeKind.TwoOP, store, operands)
        {
        }

        /// <summary>
        /// Creates a new instance of a Mod instruction.
        /// </summary>
        public Mod(short a, short b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Mod instruction.
        /// </summary>
        public Mod(short a, ZVariable b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Mod instruction.
        /// </summary>
        public Mod(ZVariable a, short b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }

        /// <summary>
        /// Creates a new instance of an Mod instruction.
        /// </summary>
        public Mod(ZVariable a, ZVariable b, ZVariable store)
            : this(store, new ZOperand(a), new ZOperand(b))
        {
        }
    }
}
