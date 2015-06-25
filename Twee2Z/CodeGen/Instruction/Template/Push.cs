using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Pushes value onto the game stack.
    /// <para>
    /// See also "push" on page 93 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Value = {_value}")]
    class Push : ZInstruction
    {
        object _value;

        /// <summary>
        /// Creates a new instance of a Push instruction with a byte as value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public Push(byte value)
            : base("push", 0x08, OpcodeTypeKind.Var, new ZOperand(value))
        {
            _value = value;
        }

        /// <summary>
        /// Creates a new instance of a Push instruction with a short as value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public Push(short value)
            : base("push", 0x08, OpcodeTypeKind.Var, new ZOperand(value))
        {
            _value = value;
        }
    }
}
