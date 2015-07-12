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
    /// Set the VARiable referenced by the operand to value.
    /// <para>
    /// See also "store" on page 102 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Variable = {_variable}, Value = {_value}")]
    class Store : ZInstruction
    {
        ZVariable _variable;
        object _value;

        /*/// <summary>
        /// Creates a new instance of a Store instruction with a byte as value.
        /// </summary>
        /// <param name="variable">The referenced variable.</param>
        /// <param name="value">The value to set.</param>
        public Store(ZVariable variable, byte value)
            : base("store", 0x0D, OpcodeTypeKind.TwoOP, new ZOperand(variable), new ZOperand(value))
        {
            _variable = variable;
            _value = value;
        }

        /// <summary>
        /// Creates a new instance of a Store instruction with a short as value.
        /// </summary>
        /// <param name="variable">The referenced variable.</param>
        /// <param name="value">The value to set.</param>
        public Store(ZVariable variable, short value)
            : base("store", 0x0D, OpcodeTypeKind.TwoOP, new ZOperand(variable), new ZOperand(value))
        {
            _variable = variable;
            _value = value;
        }*/

        /// <summary>
        /// Creates a new instance of a Store instruction with a byte as value.
        /// </summary>
        /// <param name="variable">The referenced variable.</param>
        /// <param name="value">The value to set.</param>
        public Store(ZVariable variable, byte value)
            : base("store", 0x0D, OpcodeTypeKind.TwoOP, new ZOperand(variable.VariableNumber), new ZOperand(value))
        {
            // While this instruction requires a variable
            // We have to convert it into a byte constant
            // Thus "new ZOperand(variable.VariableNumber)"
            _variable = variable;
            _value = value;
        }

        /// <summary>
        /// Creates a new instance of a Store instruction with a short as value.
        /// </summary>
        /// <param name="variable">The referenced variable.</param>
        /// <param name="value">The value to set.</param>
        public Store(ZVariable variable, short value)
            : base("store", 0x0D, OpcodeTypeKind.TwoOP, new ZOperand(variable.VariableNumber), new ZOperand(value))
        {
            // While this instruction requires a variable
            // We have to convert it into a byte constant
            // Thus "new ZOperand(variable.VariableNumber)"
            _variable = variable;
            _value = value;
        }

        /// <summary>
        /// Creates a new instance of a Store instruction with a variable as value.
        /// </summary>
        /// <param name="variable">The referenced variable.</param>
        /// <param name="value">The value to set.</param>
        public Store(ZVariable variable, ZVariable value)
            : base("store", 0x0D, OpcodeTypeKind.TwoOP, new ZOperand(variable.VariableNumber), new ZOperand(value))
        {
            // While this instruction requires a variable
            // We have to convert it into a byte constant
            // Thus "new ZOperand(variable.VariableNumber)"
            _variable = variable;
            _value = value;
        }
    }
}
