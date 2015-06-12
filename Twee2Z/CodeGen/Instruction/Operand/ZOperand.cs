using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Instruction.Opcode;

namespace Twee2Z.CodeGen.Instruction.Operand
{
    class ZOperand : ZComponent
    {
        private object _value;
        private OperandTypeKind _operandType;

        public ZOperand(byte value)
        {
            _value = value;
            _operandType = OperandTypeKind.SmallConstant;
        }

        public ZOperand(short value)
        {
            _value = value;
            _operandType = OperandTypeKind.LargeConstant;
        }

        public ZOperand(ZLabel value)
        {
            _value = value;
            _subComponents.Add(value);
            _operandType = OperandTypeKind.LargeConstant;
        }

        public ZOperand(ZVariable value)
        {
            _value = value;
            _subComponents.Add(value);
            _operandType = OperandTypeKind.Variable;
        }

        public object Value { get { return _value; } }

        public OperandTypeKind OperandType { get { return _operandType; } }

        public override int Size
        {
            get
            {
                if (_value is byte)
                    return 1;
                else if (_value is short)
                    return 2;
                else if (_value is ZLabel || _value is ZVariable)
                    return ((IZComponent)_value).Size;
                else
                    throw new Exception("This ZOperand contains an unsupported type.");
            }
        }

        public override byte[] ToBytes()
        {
            List<byte> byteList = new List<byte>();

            if (_value is byte)
                byteList.Add((byte)_value);
            else if (_value is short)
            {
                byteList.Add((byte)(((short)_value) >> 8));
                byteList.Add((byte)(short)_value);
            }
            else if (_value is ZLabel || _value is ZVariable)
                byteList.AddRange(((IZComponent)_value).ToBytes());
            else
                throw new Exception("This ZOperand contains an unsupported type.");

            return byteList.ToArray();
        }
    }
}
