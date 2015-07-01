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
    /// Jump (unconditionally) to the given label.
    /// It is legal for this to jump into a different routine (which should not change the routine call state), although it is considered bad practice to do so.
    /// <para>
    /// See also "jump" on page 87 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, JumpAddress = {_jumpLabel.TargetAddress}")]
    class Jump : ZInstruction
    {
        private ZJumpLabel _jumpLabel = null;

        /// <summary>
        /// Creates a new instance of a Jump instruction.
        /// </summary>
        /// <param name="jumpLabel">The label to jump to.</param>
        public Jump(ZJumpLabel jumpLabel)
            : base("jump", 0x0C, OpcodeTypeKind.OneOP, new ZOperand(jumpLabel))
        {
            _jumpLabel = jumpLabel;
        }

        public ZJumpLabel JumpAddress { get { return _jumpLabel; } }
    }
}
