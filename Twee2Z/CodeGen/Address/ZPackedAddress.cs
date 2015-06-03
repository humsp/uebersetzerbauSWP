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
    /// Points to a routine or string in high memory.
    /// See also "1.2.2 Addresses" on page 13 for reference.
    /// </summary>
    [DebuggerDisplay("Absolute = {_address}, Byte = {(ushort)(_address / 8)}")]
    class ZPackedAddress : ZAddress
    {
        /// <param name="address">Absolute address in memory.</param>
        public ZPackedAddress(int address)
            : base(address)
        {
            if (address < 0x0000 || address > ZMemory.MaxMemorySize)
                throw new ArgumentException(String.Format("A packed address must be between 0x0000 and {0} (last byte of high memory).", ZMemory.MaxMemorySize), "address");

            else if (address % 8 != 0)
                throw new ArgumentException("A packed address must be divisible by 8.", "address");
        }

        public override byte[] ToBytes()
        {
            byte[] byteArray = new byte[2];

            byteArray[0] = (byte)((_address / 8) >> 8);
            byteArray[1] = (byte)((_address / 8));

            return byteArray;
        }
    }
}
