using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Instruction.Opcode;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Exit the game immediately. (Any "Are you sure?" question must be asked by the game, not the interpreter.)
    /// It is not legal to return from the main routine (that is, from where execution first begins) and this must be used instead.
    /// <para>
    /// See also "quit" on page 94 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}")]
    class Pop : ZInstruction
    {
        /// <summary>
        /// Creates a new instance of a Pop instruction.
        /// </summary>
        public Pop()
            : base("pop", 0x09, OpcodeTypeKind.ZeroOP)
        {
        }
    }
}
