using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Variable
{
    abstract class ZVariable : ZComponent
    {
        protected VariableKind _variableKind;
        protected byte _variableNumber;

        public ZVariable(VariableKind variableKind)
        {
            _variableKind = variableKind;
        }

        public ZVariable(VariableKind variableKind, byte variableNumber)
            : this(variableKind)
        {
            _variableNumber = variableNumber;
        }

        public VariableKind VariableKind { get { return _variableKind; } }

        public byte VariableNumber { get { return _variableNumber; } }

        public override byte[] ToBytes()
        {
            return new byte[] { _variableNumber };
        }
    }
}
