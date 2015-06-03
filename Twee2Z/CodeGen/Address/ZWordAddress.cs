using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Address
{
    /// <summary>
    /// Points to a word in the bottom 128K of memory. Used in the abbreviations table only.
    /// See also "1.2.2 Addresses" on page 13 for reference.
    /// </summary>
    [DebuggerDisplay("Absolute = {_address}, Byte = {(ushort)(_address / 2)}")]
    class ZWordAddress : ZAddress
    {
        /// <param name="address">Absolute address in memory.</param>
        public ZWordAddress(int address)
            : base(address)
        {
            if (address < 0x0000 || address > 0x1FFFE)
                throw new ArgumentException("A word address must be between 0x0000 and 0x1FFFE (bottom 128K of memory).", "address");

            else if (address % 2 != 0)
                throw new ArgumentException("A word address must be even.", "address");
        }

        public override byte[] ToBytes()
        {
            byte[] byteArray = new byte[2];

            byteArray[0] = (byte)((_address / 2) >> 8);
            byteArray[1] = (byte)((_address / 2));

            return byteArray;
        }
    }
}
