using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen.Instructions.Templates
{
    /// <summary>
    /// Executes routine() and throws away result.
    /// </summary>
    [DebuggerDisplay("Output = {_text.Text}", Name = "call_1n")]
    class Call1n : ZInstruction
    {
        private ZPackedAddress _routineAddress = null;

        public Call1n(int routineAddress)
            : base("call_1n", 0x0F, OperandCountKind.OneOP, InstructionFormKind.Short)
        {
            _routineAddress = new ZPackedAddress(routineAddress);
            _subComponents.Add(_routineAddress);
        }

        public int RoutineAddress { get { return _routineAddress.Address; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.AddRange(_routineAddress.ToBytes());

            return byteList.ToArray();
        }
    }
}
