using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Text;
using Twee2Z.CodeGen.Instruction.Opcode;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Print the quoted (literal) Z-encoded string.
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Output = {_text.Text}")]
    class Print : ZInstruction
    {
        private ZText _text = null;

        public Print(string output)
            : base("print", 0x02, OpcodeTypeKind.ZeroOP)
        {
            if (string.IsNullOrEmpty(output))
                throw new ArgumentException("The given output to print is null or empty.", "output");

            _text = new ZText(output);
            // Print is an unique case here
            // It is listed as ZeroOP but the string appended to the opcode
            // Do not add normal operands to SubComponents
            // It results in really nasty bugs (Size will count it twice -> setting labels will fail)
            _subComponents.Add(_text);
        }

        public string Output { get { return _text.Text; } }

        public override int Size
        {
            get
            {
                return base.Size;
            }
        }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.AddRange(_text.ToBytes());

            return byteList.ToArray();
        }
    }
}
