using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Instructions;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// The entire memory within a story file. It is split into dynamic, static and high memory.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZMemory : ZComponentBase
    {
        internal const int MaxMemorySize = 0x80000;

        private ZDynamicMemory _dynamicMem;
        private ZStaticMemory _staticMem;
        private ZHighMemory _highMem;

        public ZMemory()
        {
            _dynamicMem = new ZDynamicMemory();
            _staticMem = new ZStaticMemory();
            _highMem = new ZHighMemory();

            _subComponents.Add(_dynamicMem);
            _subComponents.Add(_staticMem);
            _subComponents.Add(_highMem);
        }

        public void SetRoutines(IEnumerable<ZRoutine> routines)
        {
            _highMem.Routines.Clear();
            _highMem.Routines.AddRange(routines);
        }

        public override Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[MaxMemorySize];
            _dynamicMem.ToBytes().CopyTo(byteArray, 0x0);
            _staticMem.ToBytes().CopyTo(byteArray, 0x4000);
            _highMem.ToBytes().CopyTo(byteArray, 0x10000);
            return byteArray;
        }

        public override int Size
        {
            get
            {
                return MaxMemorySize;
            }
        }
    }
}
