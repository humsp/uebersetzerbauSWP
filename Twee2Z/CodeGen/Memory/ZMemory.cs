using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Instructions;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// The entire memory within a story file. It is split into dynamic, static and high memory.
    /// See also "1.1 Regions of memory" on page 12 for reference.
    /// </summary>
    class ZMemory : ZComponent
    {
        internal const ushort DynamicMemoryAddr = 0x0000;
        internal const ushort StaticMemoryAddr = 0x4000;
        internal const int HighMemoryAddr = 0x10000;
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

            _componentAddress = new ZAddress(0x0000);
        }

        public void SetRoutines(IEnumerable<ZRoutine> routines)
        {
            _highMem.SubComponents.Clear();
            _highMem.SubComponents.AddRange(routines);
            _highMem.Routines.Clear();
            _highMem.Routines.AddRange(routines);
        }

        protected override void Setup()
        {
            base.Setup();
            SetupLabels(_subComponents);
        }

        private void SetupLabels(IEnumerable<IZComponent> subComponents)
        {
            foreach (IZComponent component in subComponents)
            {
                ZRoutineLabel routineLabel = component as ZRoutineLabel;

                if (routineLabel != null && routineLabel.TargetAddress == null)
                {
                    ZRoutine foundRoutine = _highMem.Routines.FirstOrDefault(r => r.Name == routineLabel.Name);

                    if (foundRoutine != null)
                        routineLabel.TargetAddress = new ZPackedAddress(foundRoutine.Address.Absolute);
                    else
                        throw new Exception(String.Format("The ZRoutineLabel targets a ZRoutine that does not exist. A routine named {0} has not been found.", routineLabel.Name));
                }

                SetupLabels(component.SubComponents);
            }
        }

        public override Byte[] ToBytes()
        {
            Setup();

            byte[] dynamicAndStaticByteArray = new byte[ZHighMemory.HighMemoryBase + 1];
            _dynamicMem.ToBytes().CopyTo(dynamicAndStaticByteArray, _dynamicMem.Address.Absolute);
            _staticMem.ToBytes().CopyTo(dynamicAndStaticByteArray, _staticMem.Address.Absolute);

            List<byte> allMemByteList = new List<byte>();
            allMemByteList.AddRange(dynamicAndStaticByteArray);
            allMemByteList.AddRange(_highMem.ToBytes());
            return allMemByteList.ToArray();
        }
    }
}
