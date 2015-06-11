using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instruction
{
    static class OpcodeHelper
    {
        public static byte[] ToOpcode(byte opcodeNumber, InstructionFormKind instructionForm, OperandCountKind operandCount, params OperandTypeKind[] operandTypeKinds)
        {
            if (operandCount == OperandCountKind.ZeroOP && operandTypeKinds.Count() != 0)
                throw new ArgumentException("With OperandCountKind.ZeroOP no operands must be given.", "operandTypeKinds");

            if (operandCount == OperandCountKind.OneOP && operandTypeKinds.Count() != 1)
                throw new ArgumentException("With OperandCountKind.OneOP exactly one operand must be given.", "operandTypeKinds");

            if (operandCount == OperandCountKind.TwoOP && operandTypeKinds.Count() != 2)
                throw new ArgumentException("With OperandCountKind.TwoOP exactly two operands must be given.", "operandTypeKinds");

            if (operandCount == OperandCountKind.Var)
            {
                if (operandTypeKinds.Count() > 4)
                {
                    if (operandTypeKinds.Count() <= 8)
                        throw new ArgumentException("With OperandCountKind.Var only 0 to 4 operands can be given. Up to 8 operands are not supported yet (only call_vs2 and call_vn2 make use of this).", "operandTypeKinds");
                    else
                        throw new ArgumentException("With OperandCountKind.Var only 0 to 4 operands can be given.", "operandTypeKinds");
                }                    
            }

            List<byte> byteList = new List<byte>();
            ushort opcodeHead = opcodeNumber;
            byte? opcodeOperands = null;

            switch (instructionForm)
            {
                case InstructionFormKind.Long:
                    if (opcodeNumber > 0x1F)
                        throw new ArgumentException("The opcode number of an InstructionFormKind.Long must be between 0x00 and 0x1F.", "opcodeNumber");
                    break;
                case InstructionFormKind.Short:
                    if (opcodeNumber > 0xF)
                        throw new ArgumentException("The opcode number of an InstructionFormKind.Short must be between 0x0 and 0xF.", "opcodeNumber");
                    opcodeHead |= 0x80;
                    break;
                case InstructionFormKind.Extended:
                    opcodeHead |= 0xBE00;
                    break;
                case InstructionFormKind.Variable:
                    if (opcodeNumber > 0x1F)
                        throw new ArgumentException("The opcode number of an InstructionFormKind.Variable must be between 0x00 and 0x1F.", "opcodeNumber");
                    opcodeHead |= 0xC0;
                    break;
                default:
                    throw new ArgumentException(String.Format("Unknown InstructionFormKind '{0}'", instructionForm.ToString()), "instructionForm");
            }
            
            if (instructionForm == InstructionFormKind.Short)
            {
                if (operandTypeKinds.Count() > 1)
                    throw new ArgumentException("An InstructionForm.Short cannot have more than one operand.", "operandTypeKinds");
                
                switch (operandCount)
                {
                    case OperandCountKind.ZeroOP:
                        opcodeHead |= 0x30;
                        break;
                    case OperandCountKind.OneOP:
                        switch (operandTypeKinds.First())
	                    {
                            case OperandTypeKind.LargeConstant:
                                opcodeHead |= 0x00;
                                break;
                            case OperandTypeKind.SmallConstant:
                                opcodeHead |= 0x10;
                                break;
                            case OperandTypeKind.Variable:
                                opcodeHead |= 0x20;
                                break;
                            case OperandTypeKind.OmittedAltogether:
                                opcodeHead |= 0x30;
                                break;
                            default:
                                break;
	                    }
                        break;
                    case OperandCountKind.TwoOP:
                    case OperandCountKind.Var:
                        throw new InvalidOperationException("An InstructionForm.Short can only be OperandCountKind.ZeroOP or OperandCountKind.OneOP.");
                    default:
                        throw new ArgumentException(String.Format("Unknown OperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }
            }
            else if (instructionForm == InstructionFormKind.Long)
            {
                if (operandTypeKinds.Count() != 2)
                    throw new ArgumentException("An InstructionForm.Long must have exectly two operands.", "operandTypeKinds");

                switch (operandCount)
                {
                    case OperandCountKind.TwoOP:
                        if (operandTypeKinds[0] == OperandTypeKind.Variable)
                            opcodeHead |= 0x40;
                        else if (operandTypeKinds[0] != OperandTypeKind.SmallConstant)
                            throw new ArgumentException("An InstructionForm.Long can only have OperandTypeKind.SmallConstant or OperandTypeKind.Variable as operands.", "operandTypeKinds");

                        if (operandTypeKinds[1] == OperandTypeKind.Variable)
                            opcodeHead |= 0x20;
                        else if (operandTypeKinds[1] != OperandTypeKind.SmallConstant)
                            throw new ArgumentException("An InstructionForm.Long can only have OperandTypeKind.SmallConstant or OperandTypeKind.Variable as operands.", "operandTypeKinds");
                        break;
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
                        opcodeHead |= 0x20;
                        break;
                    case OperandCountKind.TwoOP:
                        break; // nothing to do here
                    case OperandCountKind.ZeroOP:
                    case OperandCountKind.OneOP:
                        throw new InvalidOperationException("An InstructionForm.Variable can only be OperandCountKind.TwoOP or OperandCountKind.Var.");
                    default:
                        throw new ArgumentException(String.Format("Unknown OperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }

                if (operandTypeKinds.Count() > 0)
                {
                    opcodeOperands = 0x00;

                    for (int i = 0; i < 4; i++)
                    {
                        if (operandTypeKinds.Count() <= i)
                        {
                            opcodeOperands |= (byte)(0x03 << ((3 - i) * 2));
                            continue;
                        }

                        switch (operandTypeKinds[i])
                        {
                            case OperandTypeKind.LargeConstant:
                                opcodeOperands |= (byte)(0x00 << ((3 - i) * 2));
                                break;
                            case OperandTypeKind.SmallConstant:
                                opcodeOperands |= (byte)(0x01 << ((3 - i) * 2));
                                break;
                            case OperandTypeKind.Variable:
                                opcodeOperands |= (byte)(0x02 << ((3 - i) * 2));
                                break;
                            case OperandTypeKind.OmittedAltogether:
                                opcodeOperands |= (byte)(0x03 << ((3 - i) * 2));
                                break;
                            default:
                                throw new ArgumentException(String.Format("Unknown OperandTypeKind '{0}'", operandTypeKinds[i].ToString()), "operandTypeKinds");
                        }
                    }
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

                if (operandTypeKinds.Count() > 0)
                {
                    opcodeOperands = 0x00;

                    for (int i = 0; i < 4; i++)
                    {
                        if (operandTypeKinds.Count() <= i)
                        {
                            opcodeOperands |= (byte)(0x03 << ((3 - i) * 2));
                            continue;
                        }

                        switch (operandTypeKinds[i])
                        {
                            case OperandTypeKind.LargeConstant:
                                opcodeOperands |= (byte)(0x00 << ((3 - i) * 2));
                                break;
                            case OperandTypeKind.SmallConstant:
                                opcodeOperands |= (byte)(0x01 << ((3 - i) * 2));
                                break;
                            case OperandTypeKind.Variable:
                                opcodeOperands |= (byte)(0x02 << ((3 - i) * 2));
                                break;
                            case OperandTypeKind.OmittedAltogether:
                                opcodeOperands |= (byte)(0x03 << ((3 - i) * 2));
                                break;
                            default:
                                throw new ArgumentException(String.Format("Unknown OperandTypeKind '{0}'", operandTypeKinds[i].ToString()), "operandTypeKinds");
                        }
                    }
                }
            }

            if (instructionForm == InstructionFormKind.Extended)
            {
                byteList.Add((byte)(opcodeHead >> 8));
            }
            byteList.Add((byte)opcodeHead);

            if (opcodeOperands != null)
                byteList.Add((byte)opcodeOperands);

            return byteList.ToArray();
        }

        public static int MeasureOpcodeSize(InstructionFormKind instructionFormKind, OperandCountKind operandCount, params OperandTypeKind[] operandTypes)
        {
            if (instructionFormKind == InstructionFormKind.Extended || instructionFormKind == InstructionFormKind.Variable)
            {
                if (operandTypes.Count() > 0)
                    return 2;
                else
                    return 1;
            }
            else
                return 1;
        }
    }
}
