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
    /// Executes routine() and throws away result.
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, RoutineAddress = {_routineLabel.TargetAddress}")]
    class Call1n : ZInstruction
    {
        public Call1n(ZRoutineLabel routineLabel)
            : base("call_1n", 0x0F, OpcodeTypeKind.OneOP, new ZOperand(routineLabel))
        {
        }
    }
}
