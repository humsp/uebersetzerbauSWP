using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instructions
{
    class ZRoutine : IZComponent
    {
        protected string _name; 
        protected byte _localVariableCount;
        protected List<ZInstruction> _instructions = new List<ZInstruction>();

        public ZRoutine(string name)
        {
            _name = name;
            _localVariableCount = 0x00;
        }

        public ZRoutine(string name, IEnumerable<ZInstruction> instructions)
            : this(name)
        {
            _instructions.AddRange(instructions);
        }

        public string Name { get { return _name; } }
        public IEnumerable<ZInstruction> Instructions { get { return _instructions; } }

        public virtual Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.Add(_localVariableCount);

            foreach (ZInstruction instruction in Instructions)
            {
                byteList.AddRange(instruction.ToBytes());
            }

            return byteList.ToArray();
        }
    }
}
