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
    [DebuggerDisplay("Name = {_opcode.Name}")]
    class EraseWindow : ZInstruction
    {
        /// <summary>
        /// Erases window with given number (to background colour);
        /// or if -1 it unsplits the screen and clears the lot;
        /// or if -2 it clears the screen without unsplitting it.
        /// In cases -1 and -2, the cursor may move (see S 8 for precise details).
        /// </summary>
        public EraseWindow(short window)
            : base("erase_window", 0x0D, OpcodeTypeKind.Var, new ZOperand(window))
        {
        }
    }
}
