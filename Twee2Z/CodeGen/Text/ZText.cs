using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Text
{
    [DebuggerDisplay("Text = {_text}")]
    class ZText : ZComponent
    {
        private string _text;
        
        public ZText(string text)
        {
            _text = text;
        }

        public string Text { get { return _text; } }

        public override byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            foreach (UInt16 chars in TextHelper.Convert(_text))
            {
                unchecked
                {
                    byteList.Add((Byte)(chars >> 8));
                    byteList.Add((Byte)chars);
                }
            }

            return byteList.ToArray();
        }

        public override int Size
        {
            get
            {
                return TextHelper.Measure(_text);
            }
        }
    }
}
