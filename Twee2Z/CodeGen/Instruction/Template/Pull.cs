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
    [DebuggerDisplay("Name = {_opcode.Name}, Variable = {_variable}")]
    class Pull : ZInstruction
    {
        ZVariable _variable;

        /// <summary>
        /// Creates a new instance of a Pull instruction.
        /// </summary>
        public Pull(ZVariable variable)
            : base("pull", 0x09, OpcodeTypeKind.Var, new ZOperand(variable.VariableNumber))
        {
            // While this instruction requires a variable
            // We have to convert it into a byte constant
            // Thus "new ZOperand(variable.VariableNumber)"
            _variable = variable;
        }
    }
}
