using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Template
{
    [DebuggerDisplay("Name = {_opcode.Name}")]
    class ReadChar : ZInstructionSt
    {
        /// <summary>
        /// Reads a single character from input stream 0 (the keyboard). The first operand must be 1 (presumably it was provided to support multiple input devices, but only the keyboard was ever used).
        /// time and routine are optional (in Versions 4 and later only) and dealt with as in read above.
        /// </summary>
        public ReadChar()
            : base("read_char", 0x16, OpcodeTypeKind.Var, new ZLocalVariable(0), new ZOperand((byte)1), new ZOperand((byte)0), new ZOperand((byte)0))
        {
        }
    }
}
