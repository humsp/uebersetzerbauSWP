using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Sets the text style to: Roman (if 0), Reverse Video (if 1), Bold (if 2), Italic (4), Fixed Pitch (8).
    /// In some interpreters (though this is not required) a combination of styles is possible (such as reverse video and bold).
    /// In these, changing to Roman should turn off all the other styles currently set.
    /// </summary>
    [DebuggerDisplay("Style = {_styleFlags}", Name = "set_text_style")]
    class SetTextStyle : ZInstruction
    {
        private StyleFlags _styleFlags;

        public SetTextStyle(StyleFlags styleFlags)
            : base("set_text_style", 0x11, OperandCountKind.Var, InstructionFormKind.Variable)
        {
            _styleFlags = styleFlags;
        }

        public StyleFlags Style { get { return _styleFlags; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.Add(127); //magic number?!
            byteList.Add((byte)_styleFlags);

            return byteList.ToArray();
        }

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
