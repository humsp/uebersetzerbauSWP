using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Instruction.Opcode;

namespace Twee2Z.CodeGen.Instruction.Template
{
    [DebuggerDisplay("Name = {_opcode.Name}")]
    class Quit : ZInstruction
    {
        /// <summary>
        /// Exit the game immediately. (Any "Are you sure?" question must be asked by the game, not the interpreter.)
        /// It is not legal to return from the main routine (that is, from where execution first begins) and this must be used instead.
        /// </summary>
        public Quit()
            : base("quit", 0x0A, OpcodeTypeKind.ZeroOP)
        {
        }
    }
}
