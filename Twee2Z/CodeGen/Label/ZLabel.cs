using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen.Label
{
    [DebuggerDisplay("Name = {_name}, TargetAdress = {_targetAddress}")]
    class ZLabel : ZComponent
    {
        protected ZAddress _targetAddress = null;
        protected string _name = null;

        public ZLabel()
        {
        }

        public ZLabel(ZAddress address)
        {
            _targetAddress = address;
        }

        public ZLabel(string name)
        {
            _name = name;
        }

        public ZLabel(string name, ZAddress address)
        {
            _targetAddress = address;
            _name = name;
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

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public override byte[] ToBytes()
        {
            throw new Exception("Cannot convert an abstract ZLabel into Z-Code.");
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
