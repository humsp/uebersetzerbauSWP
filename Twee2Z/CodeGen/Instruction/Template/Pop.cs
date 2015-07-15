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
    /// Throws away the top item on the stack.
    /// <para>
    /// See also "pop" on page 91 for reference.
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
