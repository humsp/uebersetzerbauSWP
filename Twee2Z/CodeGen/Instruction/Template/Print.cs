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
    /// Print the quoted (literal) Z-encoded string.
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Output = {_text.Text}")]
    class Print : ZInstruction
    {
        private ZText _text = null;

        public Print(string output)
            : base("print", 0x02, InstructionFormKind.Short, OperandCountKind.ZeroOP)
        {
            _text = new ZText(output);
            _subComponents.Add(_text);
        }

        protected override void Setup(int currentAddress)
        {
            base.Setup(currentAddress);
        }

        public string Output { get { return _text.Text; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.AddRange(_text.ToBytes());

            return byteList.ToArray();
        }
    }
}
