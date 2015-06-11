using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instruction
{
    enum OperandTypeKind
    {
        LargeConstant = 0x00,
        SmallConstant = 0x01,
        Variable = 0x02,
        OmittedAltogether = 0x03
    }
}
