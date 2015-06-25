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
    /// Pulls value off a stack. (If the stack underflows, the interpreter should halt with a suitable error message.)
    /// <para>
    /// See also "pull" on page 93 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Opcode = {_opcode}, Store = {_store}")]
    class Pull : ZInstructionSt
    {
        /// <summary>
        /// Creates a new instance of a Pull instruction.
        /// </summary>
        public Pull(ZVariable variable)
            : base("pull", 0x09, OpcodeTypeKind.Var, variable)
        {
        }
    }
}
