using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Calls a routine with given label and throws its result away.
    /// <para>
    /// See also "call_1n" on page 80 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, RoutineLabel = {RoutineLabel}")]
    class Call1n : ZInstruction
    {
        /// <summary>
        /// Creates a new instance of a Call1n instruction.
        /// </summary>
        /// <param name="routineLabel">The label of the routine to call.</param>
        public Call1n(ZRoutineLabel routineLabel)
            : base("call_1n", 0x0F, OpcodeTypeKind.OneOP, new ZOperand(routineLabel))
        {
        }

        /// <summary>
        /// Gets or sets the routine label to call.
        /// </summary>
        public ZRoutineLabel RoutineLabel
        {
            get
            {
                return (ZRoutineLabel)_operands[0].Value;
            }
            set
            {
                _operands[0] = new ZOperand(value);
            }
        }
    }
}
