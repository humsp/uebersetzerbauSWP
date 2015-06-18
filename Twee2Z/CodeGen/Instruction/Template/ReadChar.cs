using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Reads a single character from input stream 0 (the keyboard) and stores the result in the store variable.
    /// <para>
    /// See also "read_char" on page 96 for reference.
    /// </para>
    /// </summary>
    /// <remarks>
    /// The operands "time" and "routine" are not supported yet. If you want to use them, then go ahead and create a ZInstruction on your own.
    /// </remarks>
    [DebuggerDisplay("Name = {_opcode.Name}, Store = {_store}")]
    class ReadChar : ZInstructionSt
    {
        /// <summary>
        /// Creates a new instance of a ReadChar instruction.
        /// </summary>
        public ReadChar(ZVariable variable)
            : base("read_char", 0x16, OpcodeTypeKind.Var, variable, new ZOperand((byte)1), new ZOperand((byte)0), new ZOperand((byte)0))
        {
        }
    }
}
