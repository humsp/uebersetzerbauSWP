using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Variable
{
    [DebuggerDisplay("GlobalVariableNumber = {GlobalVariableNumber}")]
    class ZGlobalVariable : ZVariable
    {
        public ZGlobalVariable(byte globalVariableNumber)
            : base(VariableKind.Global)
        {
            if (globalVariableNumber > 239)
                throw new ArgumentException("There are 240 global variables to choose from only.", "globalVariableNumber");

            _variableNumber = (byte)(globalVariableNumber + 0x10);
        }

        public byte GlobalVariableNumber
        {
            get
            {
                return (byte)(_variableNumber - 0x10);
            }
        }
    }
}
