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
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Print (signed) number in decimal.
    /// <para>
    /// See also "print_num" on page 92 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Value = {_operands[0].Value}")]
    class PrintNum : ZInstruction
    {
        /// <summary>
        /// Creates a new instance of a PrintNum instruction.
        /// </summary>
        public PrintNum(short number)
            : base("print_num", 0x06, OpcodeTypeKind.Var, new ZOperand((short)number))
        {
        }

        /// <summary>
        /// Creates a new instance of a PrintNum instruction.
        /// </summary>
        public PrintNum(ZVariable variable)
            : base("print_num", 0x06, OpcodeTypeKind.Var, new ZOperand(variable))
        {
        }
    }
}
