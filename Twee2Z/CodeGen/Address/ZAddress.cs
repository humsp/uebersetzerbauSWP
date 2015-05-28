using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Address
{
    /// <summary>
    /// Abstraction of a memory address in the Z-machine.
    /// </summary>
    abstract class ZAddress : ZComponent
    {
        protected int _address;

        /// <param name="address">Absolute address in memory.</param>
        public ZAddress(int address)
        {
            _address = address;
        }

        /// <summary>
        /// Get the absolute memory address.
        /// </summary>
        public int Address { get { return _address; } }

        public override int Size
        {
            get
            {
                // Every ZAddress fits into two bytes.
                return 2;
            }
        }
    }
}
