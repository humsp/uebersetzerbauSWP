using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen.Instructions
{
    [DebuggerDisplay("LocalVariableCount = {_localVariableCount}, InstructionCount = {_subComponents.Count}", Name = "{_name}")]
    class ZRoutine : ZComponent
    {
        protected string _name;
        protected byte _localVariableCount;

        public ZRoutine(string name)
        {
            _name = name;
            _localVariableCount = 0x00;
        }

        public ZRoutine(string name, IEnumerable<ZInstruction> instructions)
            : this(name)
        {
            _subComponents.AddRange(instructions);
        }

        public string Name { get { return _name; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.Add(_localVariableCount);

            foreach (ZInstruction instruction in _subComponents)
            {
                byteList.AddRange(instruction.ToBytes());
            }

            // Routines use packed addresses. Therefore additionial bytes might have to be added for padding.
            byteList.AddRange(new byte[8 - byteList.Count % 8]);

            return byteList.ToArray();
        }

        public override int Size
        {
            get
            {
                // One byte for the local variable count + all instructions
                int size = 1 + _subComponents.Sum(component => component.Size);
                // Plus additional padding if needed
                return size + (8 - size % 8);
            }
        }

        protected override void SetAddress(int absoluteAddr)
        {
            _componentAddress = new ZPackedAddress(absoluteAddr);
        }
    }
}
