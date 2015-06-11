using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Address
{
    /// <summary>
    /// Abstraction of a memory address in the Z-machine.
    /// </summary>
    [DebuggerDisplay("Absolute = {_address}")]
    class ZAddress
    {
        protected int _address;
        protected Action<int> _validateAddrAction = null;

        /// <param name="address">Absolute address in memory.</param>
        public ZAddress(int address)
        {
            _address = address;
        }

        /// <summary>
        /// Get or set the absolute memory address.
        /// </summary>
        public virtual int Absolute
        {
            get
            {
                return _address;
            }
            set
            {
                if (_validateAddrAction != null)
                    _validateAddrAction.Invoke(_address);

                _address = value;
            }
        }

        public static int Size
        {
            get
            {
                // Every ZAddress fits into two bytes.
                return 2;
            }
        }

        public virtual byte[] ToBytes()
        {
            throw new InvalidOperationException("An absolute address cannot be converted into Z-Code.");
        }
    }
}
