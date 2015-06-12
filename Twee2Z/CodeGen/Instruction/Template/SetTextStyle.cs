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
    /// Sets the text style to: Roman (if 0), Reverse Video (if 1), Bold (if 2), Italic (4), Fixed Pitch (8).
    /// In some interpreters (though this is not required) a combination of styles is possible (such as reverse video and bold).
    /// In these, changing to Roman should turn off all the other styles currently set.
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Style = {_styleFlags}")]
    class SetTextStyle : ZInstruction
    {
        private StyleFlags _styleFlags;

        public SetTextStyle(StyleFlags styleFlags)
            : base("set_text_style", 0x11, OpcodeTypeKind.Var, new ZOperand((byte)styleFlags))
        {
            _styleFlags = styleFlags;
        }

        public StyleFlags Style { get { return _styleFlags; } }

        [Flags]
        public enum StyleFlags
        {
            /// <summary>
            /// Default (turns off all the other styles)
            /// </summary>
            Roman = 0,
            ReverseVideo = 1,
            Bold = 2,
            Italic = 4,
            FixedPitch = 8,
        }
    }
}
