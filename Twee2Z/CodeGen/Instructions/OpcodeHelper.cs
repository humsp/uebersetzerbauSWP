using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instructions
{
    public static class OpcodeHelper
    {
        public static ushort ToOpcode(ushort hex, OperandCountKind operandCount)
        {
            switch (operandCount)
            {
                case OperandCountKind.ZeroOP:
                    return (ushort)(hex | 0xB0);
                case OperandCountKind.OneOP:
                    return (ushort)(hex | 0x80);
                case OperandCountKind.TwoOP:
                    throw new NotImplementedException("Opcodes with two operands are not supported yet.");
                case OperandCountKind.Var:
                    return (ushort)(hex | 0xE0);
                default:
                    throw new ArgumentException(String.Format("Unknown OperandCountKind '{0}'", operandCount.ToString()), "operandCount");
            }
        }

        public static int MeasureOpcodeSize(ushort hex)
        {
            if (hex > 0xFF)
                return 2;
            else
                return 1;
        }
    }
}
