using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instruction
{
    static class OpcodeHelper
    {
        public static ushort ToOpcode(byte opcodeNumber, InstructionFormKind instructionForm, OperandCountKind operandCount)
        {
            ushort opcode = opcodeNumber;

            switch (instructionForm)
            {
                case InstructionFormKind.Long:
                    if (opcodeNumber > 0x1F)
                        throw new ArgumentException("The opcode number of an InstructionFormKind.Long must be between 0x00 and 0x1F.", "opcodeNumber");
                    break; // nothing to do here
                case InstructionFormKind.Short:
                    if (opcodeNumber > 0xF)
                        throw new ArgumentException("The opcode number of an InstructionFormKind.Short must be between 0x0 and 0xF.", "opcodeNumber");
                    opcode |= 0x80;
                    break;
                case InstructionFormKind.Extended:
                    opcode |= 0xBE00;
                    break;
                case InstructionFormKind.Variable:
                    if (opcodeNumber > 0x1F)
                        throw new ArgumentException("The opcode number of an InstructionFormKind.Variable must be between 0x00 and 0x1F.", "opcodeNumber");
                    opcode |= 0xC0;
                    break;
                default:
                    throw new ArgumentException(String.Format("Unknown InstructionFormKind '{0}'", instructionForm.ToString()), "instructionForm");
            }
            
            if (instructionForm == InstructionFormKind.Short)
            {
                switch (operandCount)
                {
                    case OperandCountKind.ZeroOP:
                        opcode |= 0x30;
                        break;
                    case OperandCountKind.OneOP:
                        break; // nothing to do here
                    case OperandCountKind.TwoOP:
                    case OperandCountKind.Var:
                        throw new InvalidOperationException("An InstructionForm.Short can only be OperandCountKind.ZeroOP or OperandCountKind.OneOP.");
                    default:
                        throw new ArgumentException(String.Format("Unknown OperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }
            }
            else if (instructionForm == InstructionFormKind.Long)
            {
                switch (operandCount)
                {
                    case OperandCountKind.TwoOP:
                        break; // nothing to do here
                    case OperandCountKind.ZeroOP:
                    case OperandCountKind.OneOP:
                    case OperandCountKind.Var:
                        throw new InvalidOperationException("An InstructionForm.Long can only be OperandCountKind.TwoOP.");
                    default:
                        throw new ArgumentException(String.Format("Unknown OperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }
            }
            else if (instructionForm == InstructionFormKind.Variable)
            {
                switch (operandCount)
                {
                    case OperandCountKind.Var:
                        opcode |= 0x20;
                        break;
                    case OperandCountKind.TwoOP:
                        break; // nothing to do here
                    case OperandCountKind.ZeroOP:
                    case OperandCountKind.OneOP:
                        throw new InvalidOperationException("An InstructionForm.Variable can only be OperandCountKind.TwoOP or OperandCountKind.Var.");
                    default:
                        throw new ArgumentException(String.Format("Unknown OperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }
            }
            else if (instructionForm == InstructionFormKind.Extended)
            {
                switch (operandCount)
                {
                    case OperandCountKind.Var:
                        break; // nothing to do here
                    case OperandCountKind.ZeroOP:
                    case OperandCountKind.OneOP:
                    case OperandCountKind.TwoOP:
                        throw new InvalidOperationException("An InstructionForm.Extended can only be OperandCountKind.Var.");
                    default:
                        throw new ArgumentException(String.Format("Unknown OperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }
            }

            return opcode;

            /*switch (operandCount)
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
            }*/
        }

        public static int MeasureOpcodeSize(InstructionFormKind instructionFormKind)
        {
            if (instructionFormKind == InstructionFormKind.Extended)
                return 2;
            else
                return 1;
        }
    }
}
