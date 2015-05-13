using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Memory
{
    public class ZMemory
    {
        private ZDynamicMemory _dynamicMem;
        private ZStaticMemory _staticMem;
        private ZHighMemory _highMem;

        public ZMemory()
        {
            _dynamicMem = new ZDynamicMemory();
            _staticMem = new ZStaticMemory();
            _highMem = new ZHighMemory();
        }

        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[0x80000];
            _dynamicMem.ToBytes().CopyTo(byteArray, 0x0);
            _staticMem.ToBytes().CopyTo(byteArray, 0x4000);
            _highMem.ToBytes().CopyTo(byteArray, 0x10000);
            return byteArray;
        }
    }
}
