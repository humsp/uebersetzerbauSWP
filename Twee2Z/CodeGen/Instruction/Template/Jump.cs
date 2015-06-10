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
    [DebuggerDisplay("JumpAddress = {_jumpLabel.TargetAddress}", Name = "jump")]
    class Jump : ZInstruction
    {
        private ZJumpLabel _jumpLabel = null;

        public Jump(ZJumpLabel jumpLabel)
            : base("jump", 0x0C, InstructionFormKind.Short, OperandCountKind.OneOP, new OperandTypeKind[] { OperandTypeKind.LargeConstant })
        {
            _jumpLabel = jumpLabel;
            _subComponents.Add(jumpLabel);
        }

        public ZJumpLabel JumpAddress { get { return _jumpLabel; } }
        
        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.AddRange(_jumpLabel.ToBytes());

            return byteList.ToArray();
        }
    }
}
