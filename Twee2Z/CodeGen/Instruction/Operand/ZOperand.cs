using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Instruction.Opcode;

namespace Twee2Z.CodeGen.Instruction.Operand
{
    /// <summary>
    /// Represents an operand in Z-Code. It can be either a small or large constant, a label or variable.
    /// See also "4.2 Operand types" on page 26 for reference.
    /// </summary>
    [DebuggerDisplay("OperandType = {_operandType}, Value = {_value}")]
    class ZOperand : ZComponent
    {
        private object _value;
        private OperandTypeKind _operandType;

        /// <summary>
        /// Creates a new instance of a ZOperand with <see cref="byte"/> as value.
        /// </summary>
        /// <param name="value">The value as byte.</param>
        public ZOperand(byte value)
        {
            _value = value;
            _operandType = OperandTypeKind.SmallConstant;
        }

        /// <summary>
        /// Creates a new instance of a ZOperand with <see cref="short"/> as value.
        /// </summary>
        /// <param name="value">The value as short.</param>
        public ZOperand(short value)
        {
            _value = value;
            _operandType = OperandTypeKind.LargeConstant;
        }

        /// <summary>
        /// Creates a new instance of a ZOperand with <see cref="ZLabel"/> as value.
        /// </summary>
        /// <param name="value">The value as ZLabel.</param>
        public ZOperand(ZLabel value)
        {
            _value = value;
            _subComponents.Add(value);
            _operandType = OperandTypeKind.LargeConstant;
        }

        /// <summary>
        /// Creates a new instance of a ZOperand with <see cref="ZVariable"/> as value.
        /// </summary>
        /// <param name="value">The value as ZVariable.</param>
        public ZOperand(ZVariable value)
        {
            _value = value;
            _subComponents.Add(value);
            _operandType = OperandTypeKind.Variable;
        }

        /// <summary>
        /// Gets the value as object.
        /// </summary>
        public object Value { get { return _value; } }

        /// <summary>
        /// Gets the OperandTypeKind.
        /// </summary>
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
                unchecked
                {
                    byteList.Add((byte)(((short)_value) >> 8));
                    byteList.Add((byte)(short)_value);
                }
            }
            else if (_value is ZLabel || _value is ZVariable)
                byteList.AddRange(((IZComponent)_value).ToBytes());
            else
                throw new Exception("This ZOperand contains an unsupported type.");

            return byteList.ToArray();
        }
    }
}
