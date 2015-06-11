using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Instruction.Template
{
    [DebuggerDisplay("Name = {_opcode.Name}")]
    class NewLine : ZInstruction
    {
        /// <summary>
        /// Print carriage return.
        /// </summary>
        public NewLine()
            : base("new_line", 0x0B, InstructionFormKind.Short, OperandCountKind.ZeroOP)
        {
        }
    }
}
