using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen.Label
{
    class ZRoutineLabel : ZLabel
    {
        public ZRoutineLabel(string name)
            : base(name)
        {
        }

        public ZRoutineLabel(string name, ZPackedAddress address)
            : base(name, address)
        {
        }

        public override byte[] ToBytes()
        {
            if (_targetAddress == null)
                throw new Exception("Cannot convert a ZLabel into Z-Code before the TargetAddress is set.");

            return ((ZPackedAddress)_targetAddress).ToBytes();
        }
    }
}
