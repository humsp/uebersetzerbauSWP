using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Instruction;
using Twee2Z.CodeGen.Instruction.Template;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Topmost memory containing routines and text. Goes from 0xFFFF to 0x80000.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZHighMemory : ZComponent
    {
        internal const ushort HighMemoryBase = 0xFFFF;
        
        private List<ZRoutine> _routines = new List<ZRoutine>();

        public ZHighMemory()
        {
            ZRoutine main = new ZRoutine(new ZInstruction[] { new Quit() }) { Label = new ZRoutineLabel("main") };
            _routines.Add(main);
            _subComponents.AddRange(_routines);
        }

        public ZHighMemory(IEnumerable<ZRoutine> routines)
        {
            _routines.AddRange(routines);
            _subComponents.AddRange(_routines);
        }

        public List<ZRoutine> Routines { get { return _routines; } }
        
        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<Byte>();

            foreach (ZRoutine routine in _routines)
            {
                byteList.AddRange(routine.ToBytes());
            }

            return byteList.ToArray();
        }
    }
}
