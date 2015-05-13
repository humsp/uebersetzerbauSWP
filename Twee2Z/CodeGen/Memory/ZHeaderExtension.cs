using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Memory
{
    public class ZHeaderExtension
    {
        private UInt16 _wordCount;
        private UInt16 _mouseX;
        private UInt16 _mouseY;
        private UInt16 _unicodeTableAddr;

        public ZHeaderExtension()
        {
            _wordCount = 0x0003;
            _mouseX = 0x0000;
            _mouseY = 0x0000;
            _unicodeTableAddr = 0x0000;
        }

        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[8];

            byteArray[0x00] = (Byte)(_wordCount >> 8);
            byteArray[0x01] = (Byte)_wordCount;

            byteArray[0x02] = (Byte)(_mouseX >> 8);
            byteArray[0x03] = (Byte)_mouseX;

            byteArray[0x04] = (Byte)(_mouseY >> 8);
            byteArray[0x05] = (Byte)_mouseY;

            byteArray[0x06] = (Byte)(_unicodeTableAddr >> 8);
            byteArray[0x07] = (Byte)_unicodeTableAddr;

            return byteArray;
        }

        public UInt32 Size { get { return 8; } }
    }
}
