using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instructions.Templates
{
    class Print : ZInstruction
    {
        private string _output = null;

        public Print(string output)
            : base("print", 0x02, InstructionFormKind.Short, OperandCountKind.ZeroOP)
        {
            _output = output;
        }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            ushort opcode = OpcodeHelper.ToOpcode(_hex, _operandCount);

            byteList.Add((byte)opcode);

            foreach (UInt16 chars in Text.ZText.Convert(_output))
            {
                byteList.Add((Byte)(chars >> 8));
                byteList.Add((Byte)chars);
            }

            return byteList.ToArray();
        }
    }
}
