using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Memory
{
    public class ZDynamicMemory
    {
        private ZHeader _header;
        private ZHeaderExtension _headerExtension;

        public ZDynamicMemory()
        {
            _header = new ZHeader();
            _headerExtension = new ZHeaderExtension();
        }

        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[0x4000];
            _header.ToBytes().CopyTo(byteArray, 0x0000);
            _headerExtension.ToBytes().CopyTo(byteArray, 0x0040);

            //main
            byteArray[0x2000] = 0x0F | 0x80;
            byteArray[0x2001] = (Byte)(0x2000 >> 8);
            byteArray[0x2002] = (Byte)0x00;
            return byteArray;
        }
    }
}
