using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.CodeGen.Instruction
{
    [DebuggerDisplay("OperandCount = {_operandCount}, Hex = {_hex}", Name = "{_name}")]
    class ZOpcode : ZComponent
    {
        protected string _name;
        protected ushort _hex;
        protected OperandCountKind _operandCount;

        public ZOpcode(string name,  ushort hex, OperandCountKind operandCount)
        {
            _name = name;
            _hex = hex;
            _operandCount = operandCount;
        }

        public string Name { get { return _name; } }
        public ushort Hex { get { return _hex; } }
        public OperandCountKind OperandCount { get { return _operandCount; } }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            ushort opcode = OpcodeHelper.ToOpcode(_hex, _operandCount);

            if (opcode > 0xFF)
            {
                byteList.Add((Byte)(opcode >> 8));
                byteList.Add((Byte)opcode);
            }
            else
            {
                byteList.Add((byte)opcode);
            }

            //TODO: Implement generic support of operands

            return byteList.ToArray();
        }

        public override int Size
        {
            get
            {
                return OpcodeHelper.MeasureOpcodeSize(_hex);
            }
        }
    }
}
