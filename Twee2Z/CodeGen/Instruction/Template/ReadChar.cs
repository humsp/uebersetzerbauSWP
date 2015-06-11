using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen.Instruction.Template
{
    [DebuggerDisplay("Name = {_opcode.Name}")]
    class ReadChar : ZInstruction
    {
        /// <summary>
        /// Reads a single character from input stream 0 (the keyboard). The first operand must be 1 (presumably it was provided to support multiple input devices, but only the keyboard was ever used).
        /// time and routine are optional (in Versions 4 and later only) and dealt with as in read above.
        /// </summary>
        public ReadChar()
            : base("read_char", 0x16, InstructionFormKind.Variable, OperandCountKind.Var, new OperandTypeKind[] { OperandTypeKind.SmallConstant, OperandTypeKind.SmallConstant, OperandTypeKind.SmallConstant })
        {
        }

        public override int Size
        {
            get
            {
                return base.Size + 4;
            }
        }

        public override byte[] ToBytes()
        {
            List<byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.Add(1);
            byteList.Add(0);
            byteList.Add(0);
            byteList.AddRange(new ZLocalVariable(0).ToBytes());

            return byteList.ToArray();
        }
    }
}
