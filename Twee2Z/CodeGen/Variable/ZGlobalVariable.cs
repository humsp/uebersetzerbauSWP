using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Variable
{
    class ZGlobalVariable : ZVariable
    {
        public ZGlobalVariable(byte globalVariableNumber)
            : base(VariableKind.Global)
        {
            if (globalVariableNumber > 239)
                throw new ArgumentException("There are 240 global variables to choose from only.", "globalVariableNumber");

            _variableNumber = (byte)(globalVariableNumber + 0x0F);
        }

        public byte GlobalVariableNuber
        {
            get
            {
                return (byte)(_variableNumber - 0x0F);
            }
        }
    }
}
