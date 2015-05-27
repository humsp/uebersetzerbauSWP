using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instructions.Templates
{
    class Quit : ZInstruction
    {
        public Quit()
            : base("quit", 0x0A, OperandCountKind.ZeroOP, InstructionFormKind.Short)
        {
        }
    }
}
