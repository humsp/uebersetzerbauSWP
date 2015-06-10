using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Executes routine() and throws away result.
    /// </summary>
    [DebuggerDisplay("RoutineAddress = {_routineLabel.TargetAddress}", Name = "call_1n")]
    class Call1n : ZInstruction
    {
        private ZRoutineLabel _routineLabel = null;

        public Call1n(ZRoutineLabel routineLabel)
            : base("call_1n", 0x0F, InstructionFormKind.Short, OperandCountKind.OneOP)
        {
            _routineLabel = routineLabel;
            _subComponents.Add(routineLabel);
        }

        public ZAddress RoutineAddress { get { return _routineLabel.TargetAddress; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.AddRange(_routineLabel.ToBytes());

            return byteList.ToArray();
        }
    }
}
