using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Erases window with given number (to background colour).
    /// <para>
    /// See also "erase_window" on page 84 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Window = {Window}")]
    class EraseWindow : ZInstruction
    {
        /// <summary>
        /// Creates a new instance of a EraseWindow instruction.
        /// </summary>
        /// <param name="window">The window to erase. The main window is 0 by default.</param>
        public EraseWindow(short window)
            : base("erase_window", 0x0D, OpcodeTypeKind.Var, new ZOperand(window))
        {
        }

        /// <summary>
        /// Gets or sets the window to erase.
        /// </summary>
        public short Window
        {
            get
            {
                return (short)_operands[0].Value;
            }
            set
            {
                _operands[0] = new ZOperand(value);
            }
        }
    }
}
