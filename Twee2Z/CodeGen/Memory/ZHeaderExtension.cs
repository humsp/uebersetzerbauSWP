using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Represents the header extension table writen right after the header.
    /// See also "11.1.7.3 The format of the header" on page 64 for reference.
    /// </summary>
    class ZHeaderExtension : ZComponent
    {
        private const int HeaderExtensionSize = 8;

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

        public override Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[HeaderExtensionSize];

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

        public override int Size { get { return HeaderExtensionSize; } }

        protected override void SetAddress(int absoluteAddr)
        {
            _componentAddress = new ZByteAddress(absoluteAddr);
        }
    }
}
