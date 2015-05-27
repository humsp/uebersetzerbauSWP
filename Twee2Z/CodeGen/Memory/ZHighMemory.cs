using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Instructions;
using Twee2Z.CodeGen.Instructions.Templates;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Topmost memory containing routines and text. Goes from 0xFFFF to 0x80000.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZHighMemory : ZComponentBase
    {
        private const int HighMemorySize = 0x70000;

        internal const ushort HighMemoryBase = 0xFFFF;
        
        private List<ZRoutine> _routines = new List<ZRoutine>();

        public ZHighMemory()
        {
            ZRoutine main = new ZRoutine("main", new ZInstruction[] {new Quit()});
            _routines.Add(main);
        }

        public ZHighMemory(IEnumerable<ZRoutine> routines)
        {
            _routines.AddRange(routines);
        }

        public List<ZRoutine> Routines { get { return _routines; } }

        public override Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[HighMemorySize];
            List<Byte> routineByteList = new List<byte>();

            foreach (ZRoutine routine in _routines)
            {
                routineByteList.AddRange(routine.ToBytes());
            }

            routineByteList.ToArray().CopyTo(byteArray, 0);

            return byteArray;
        }

        public override int Size { get { return HighMemorySize; } }
    }
}
