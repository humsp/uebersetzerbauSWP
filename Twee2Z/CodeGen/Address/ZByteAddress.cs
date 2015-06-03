using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Memory;

namespace Twee2Z.CodeGen.Address
{
    /// <summary>
    /// Points to a byte in dynamic and static memory.
    /// See also "1.2.1 Addresses" on page 13 for reference.
    /// </summary>
    [DebuggerDisplay("Absolute = {_address}, Byte = {(ushort)_address}")]
    class ZByteAddress : ZAddress
    {
        /// <param name="address">Absolute address in memory.</param>
        public ZByteAddress(int address)
            : base(address)
        {
            if (address < 0x0000 || address > ZHighMemory.HighMemoryBase)
                throw new ArgumentException(String.Format("A byte address must be between 0x0000 and {0} (last byte of static memory).", ZHighMemory.HighMemoryBase), "address");
        }

        public override byte[] ToBytes()
        {
            byte[] byteArray = new byte[2];

            byteArray[0] = (byte)(_address >> 8);
            byteArray[1] = (byte)(_address);

            return byteArray;
        }
    }
}
