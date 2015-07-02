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
    /// Probably the official "no operation" instruction, which, appropriately, was never operated (in any of the Infocom datafiles):
    /// it may once have been a breakpoint.
    /// <para>
    /// See also "nop" on page 89 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}")]
    class Nop : ZInstruction
    {
        /// <summary>
        /// Creates a new instance of a Nop instruction.
        /// </summary>
        public Nop()
            : base("nop", 0x04, OpcodeTypeKind.ZeroOP)
        {
        }
    }
}
