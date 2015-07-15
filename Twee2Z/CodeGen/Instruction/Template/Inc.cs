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
    /// Increment variable by 1. (This is signed, so -1 increments to 0.)
    /// <para>
    /// See also "inc" on page 86 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Variable = {_variable}")]
    class Inc : ZInstruction
    {
        private ZVariable _variable;

        /// <summary>
        /// Creates a new instance of an Inc instruction.
        /// </summary>
        public Inc(ZVariable variable)
            : base("inc", 0x05, OpcodeTypeKind.OneOP, new ZOperand(variable))
        {
            _variable = variable;
        }

        public ZVariable Variable { get { return _variable; } }
    }
}
