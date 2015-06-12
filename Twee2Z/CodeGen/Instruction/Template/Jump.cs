using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Text;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Executes routine() and throws away result.
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, JumpAddress = {_jumpLabel.TargetAddress}")]
    class Jump : ZInstruction
    {
        private ZJumpLabel _jumpLabel = null;

        public Jump(ZJumpLabel jumpLabel)
            : base("jump", 0x0C, OpcodeTypeKind.OneOP, new ZOperand(jumpLabel))
        {
            _jumpLabel = jumpLabel;
            _subComponents.Add(jumpLabel);
        }

        public ZJumpLabel JumpAddress { get { return _jumpLabel; } }
    }
}
