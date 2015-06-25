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
    /// The value of the variable referred to by the operand is stored in the result.
    /// <para>
    /// See also "load" on page 87 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Variable = {_variable}, Store = {_store}")]
    class Load : ZInstructionSt
    {
        ZVariable _variable;

        /// <summary>
        /// Creates a new instance of a Load instruction.
        /// </summary>
        public Load(ZVariable variable, ZVariable store)
            : base("load", 0x0E, OpcodeTypeKind.OneOP, store, new ZOperand(variable.VariableNumber))
        {
            // While this instruction requires a variable
            // We have to convert it into a byte constant
            // Thus "new ZOperand(variable.VariableNumber)"
            _variable = variable;
        }
    }
}
