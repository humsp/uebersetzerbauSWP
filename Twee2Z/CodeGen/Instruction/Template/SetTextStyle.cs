using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Text;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Sets the text style to Reverse Video, Bold, Italic or Fixed Pitch. Or a combination of that (not all interpreters support this though).
    /// Setting to <see cref="StyleFlags.None"/> will turn off all styles currently set.
    /// <para>
    /// See also "set_text_style" on page 100 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Style = {Style}")]
    class SetTextStyle : ZInstruction
    {
        /// <summary>
        /// Creates a new instance of a SetTextStyle instruction.
        /// </summary>
        public SetTextStyle(StyleFlags styleFlags)
            : base("set_text_style", 0x11, OpcodeTypeKind.Var, new ZOperand((byte)styleFlags))
        {
        }

        /// <summary>
        /// Gets or sets the current style.
        /// </summary>
        public StyleFlags Style
        {
            get
            {
                return (StyleFlags)_operands[0].Value;
            }
            set
            {
                _operands[0] = new ZOperand((byte)value);
            }
        }

        [Flags]
        public enum StyleFlags
        {
            /// <summary>
            /// Default (turns off all the other styles). In the reference called "Roman".
            /// </summary>
            None = 0,
            ReverseVideo = 1,
            Bold = 2,
            Italic = 4,
            FixedPitch = 8,
        }
    }
}
