using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Instructions.Templates
{
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
