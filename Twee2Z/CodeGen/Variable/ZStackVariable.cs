using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Variable
{
    class ZStackVariable : ZVariable
    {
        public ZStackVariable()
            : base(VariableKind.Stack, 0x00)
        {
        }
    }
}
