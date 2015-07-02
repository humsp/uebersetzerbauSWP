using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Print a ZSCII character. The operand must be a character code defined in ZSCII for output (see S 3).
    /// In particular, it must certainly not be negative or larger than 1023.
    /// <para>
    /// See also "print_char" on page 92 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Output = {_zsciiChar}")]
    class PrintChar : ZInstruction
    {
        private byte _zsciiChar;

        /// <summary>
        /// Creates a new instance of a PrintChar instruction.
        /// </summary>
        public PrintChar(byte zsciiChar)
            : base("print_char", 0x05, OpcodeTypeKind.Var, new ZOperand((short)zsciiChar))
        {
            _zsciiChar = zsciiChar;
        }

        public byte Output { get { return _zsciiChar; } }
    }
}
