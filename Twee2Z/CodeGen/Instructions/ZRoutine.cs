using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instructions
{
    class ZRoutine
    {
        protected byte _localVariableCount;
        protected List<ZInstruction> _instructions = new List<ZInstruction>();

        public ZRoutine()
        {
            _localVariableCount = 0x00;
        }

        public ZRoutine(IEnumerable<ZInstruction> instructions)
            : base()
        {
            _instructions.AddRange(instructions);
        }

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
