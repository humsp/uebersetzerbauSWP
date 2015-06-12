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
    /// Print a Unicode character. See S 3.8.5.4 and S 7.5 for details. The given character code must be defined in Unicode.
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Output = {_unicodeChar}")]
    class PrintUnicode : ZInstruction
    {
        private char _unicodeChar;

        public PrintUnicode(char unicodeChar)
            : base("print_unicode", 0x0B, OpcodeTypeKind.Ext, new ZOperand((short)unicodeChar))
        {
            if (Char.IsControl(unicodeChar))
                throw new ArgumentException("Control characters for printing unicode are not allowed.", "unicodeChar");

            _unicodeChar = unicodeChar;
        }

        public char Output { get { return _unicodeChar; } }
    }
}
