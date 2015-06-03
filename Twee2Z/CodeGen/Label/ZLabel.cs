using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen.Label
{
    abstract class ZLabel : ZComponent
    {
        protected ZAddress _targetAddress = null;
        protected string _name;

        public ZLabel(string name)
        {
            _name = name;
        }

        public ZLabel(string name, ZAddress targetAddress)
            : this(name)
        {
            _targetAddress = targetAddress;
        }

        public ZAddress TargetAddress
        {
            get
            {
                return _targetAddress;
            }
            set
            {
                _targetAddress = value;
            }
        }

        public string Name { get { return _name; } }

        public override byte[] ToBytes()
        {
            throw new Exception("Cannot convert a abstract ZLabel into Z-Code.");
        }

        public override int Size
        {
            get
            {
                return ZAddress.Size;
            }
        }
    }
}
