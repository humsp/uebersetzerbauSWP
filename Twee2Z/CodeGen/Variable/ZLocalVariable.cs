using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Variable
{
    class ZLocalVariable : ZVariable
    {
        public ZLocalVariable(byte localVariableNumber)
            : base(VariableKind.Local)
        {
            if (localVariableNumber > 14)
                throw new ArgumentException("There are 15 local variables to choose from only.", "localVariableNumber");

            _variableNumber = (byte)(localVariableNumber + 0x01);
        }

        public byte LocalVariableNuber
        {
            get
            {
                return (byte)(_variableNumber - 0x01);
            }
        }
    }
}
