using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Variable
{
    [DebuggerDisplay("VariableKind = {_variableKind}, VariableNumber = {_variableNumber}")]
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

        public override int Size
        {
            get
            {
                return 1; // these are exactly 1 byte long
            }
        }

        public override byte[] ToBytes()
        {
            return new byte[] { _variableNumber };
        }
    }
}
