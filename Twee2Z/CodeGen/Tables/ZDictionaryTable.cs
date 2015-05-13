using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Tables
{
    public class ZDictionaryTable
    {
        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[4];

            //byte
            byteArray[0x00] = 0x00;
            //entry length
            byteArray[0x01] = 0x06;

            //number of entries
            byteArray[0x02] = 0x00;
            byteArray[0x03] = 0x00;

            return byteArray;
        }
    }
}
