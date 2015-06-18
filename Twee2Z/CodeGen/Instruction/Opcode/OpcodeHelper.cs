using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction.Opcode
{
    static class OpcodeHelper
    {
        /// <summary>
        /// Takes every information needed to return a valid Z-Code opcode in bytes.
        /// </summary>
        /// <param name="opcodeNumber"></param>
        /// <param name="instructionForm"></param>
        /// <param name="operandCount"></param>
        /// <param name="operandTypes"></param>
        /// <returns>An opcode in bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException"> If <paramref name="opcodeNumber"/> has an invalid value or <paramref name="operands"/> an invalid count of operands.</exception>
        /// <exception cref="InvalidOperationException"> If <paramref name="instructionForm"/> and <paramref name="operandCount"/> is an invalid combination.
        /// <exception cref="ArgumentException"> If <paramref name="operands"/> contains an unknown enum value.</exception>
        public static byte[] ToOpcode(byte opcodeNumber, InstructionFormKind instructionForm, InstructionOperandCountKind operandCount, OperandTypeKind[] operandTypes)
        {
            if (operandCount == InstructionOperandCountKind.ZeroOP && operandTypes.Count() != 0)
                throw new ArgumentOutOfRangeException("operandTypes", operandTypes.Count(), "With InstructionOperandCountKind.ZeroOP no operands must be given.");

            if (operandCount == InstructionOperandCountKind.OneOP && operandTypes.Count() != 1)
                throw new ArgumentOutOfRangeException("operandTypes", operandTypes.Count(), "With InstructionOperandCountKind.OneOP exactly one operand must be given.");

            if (operandCount == InstructionOperandCountKind.TwoOP && operandTypes.Count() != 2)
                throw new ArgumentOutOfRangeException("operandTypes", operandTypes.Count(), "With InstructionOperandCountKind.TwoOP exactly two operands must be given.");

            if (operandCount == InstructionOperandCountKind.Var)
            {
                if (operandTypes.Count() > 4)
                {
                    if (operandTypes.Count() <= 8)
                        throw new ArgumentOutOfRangeException("operandTypes", operandTypes.Count(), "With InstructionOperandCountKind.Var only 0 to 4 operands can be given. Up to 8 operands are not supported yet (only call_vs2 and call_vn2 make use of this).");
                    else
                        throw new ArgumentOutOfRangeException("operandTypes", operandTypes.Count(), "With InstructionOperandCountKind.Var only 0 to 4 operands can be given.");
                }                    
            }

            List<byte> byteList = new List<byte>();
            ushort opcodeHead = opcodeNumber;
            byte? opcodeOperands = null;

            switch (instructionForm)
            {
                case InstructionFormKind.Long:
                    if (opcodeNumber > 0x1F)
                        throw new ArgumentOutOfRangeException("opcodeNumber", opcodeNumber, "The opcode number of an InstructionFormKind.Long must be in range of 0x00 - 0x1F.");
                    break;
                case InstructionFormKind.Short:
                    if (opcodeNumber > 0xF)
                        throw new ArgumentOutOfRangeException("opcodeNumber", opcodeNumber, "The opcode number of an InstructionFormKind.Short must be in range of 0x0 - 0xF.");
                    opcodeHead |= 0x80;
                    break;
                case InstructionFormKind.Extended:
                    opcodeHead |= 0xBE00;
                    break;
                case InstructionFormKind.Variable:
                    if (opcodeNumber > 0x1F)
                        throw new ArgumentOutOfRangeException("opcodeNumber", opcodeNumber, "The opcode number of an InstructionFormKind.Variable must be in range of 0x00 - 0x1F.");
                    opcodeHead |= 0xC0;
                    break;
                default:
                    throw new ArgumentException(String.Format("Unknown InstructionFormKind '{0}'", instructionForm.ToString()), "instructionForm");
            }
            
            if (instructionForm == InstructionFormKind.Short)
            {
                if (operandTypes.Count() > 1)
                    throw new ArgumentOutOfRangeException("operandTypes", operandTypes.Count(), "An InstructionForm.Short cannot have more than one operand.");
                
                switch (operandCount)
                {
                    case InstructionOperandCountKind.ZeroOP:
                        opcodeHead |= 0x30;
                        break;
                    case InstructionOperandCountKind.OneOP:
                        switch (operandTypes.Single())
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
                    case InstructionOperandCountKind.TwoOP:
                    case InstructionOperandCountKind.Var:
                        throw new InvalidOperationException("An InstructionForm.Short can only be InstructionOperandCountKind.ZeroOP or InstructionOperandCountKind.OneOP.");
                    default:
                        throw new ArgumentException(String.Format("Unknown InstructionOperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }
            }
            else if (instructionForm == InstructionFormKind.Long)
            {
                if (operandTypes.Count() != 2)
                    throw new ArgumentOutOfRangeException("operandTypes", operandTypes.Count(), "An InstructionForm.Long must have exectly two operands.");

                switch (operandCount)
                {
                    case InstructionOperandCountKind.TwoOP:
                        if (operandTypes[0] == OperandTypeKind.Variable)
                            opcodeHead |= 0x40;
                        else if (operandTypes[0] != OperandTypeKind.SmallConstant)
                            throw new ArgumentException("An InstructionForm.Long can only have OperandTypeKind.SmallConstant or OperandTypeKind.Variable as operands.", "operandTypes");

                        if (operandTypes[1] == OperandTypeKind.Variable)
                            opcodeHead |= 0x20;
                        else if (operandTypes[1] != OperandTypeKind.SmallConstant)
                            throw new ArgumentException("An InstructionForm.Long can only have OperandTypeKind.SmallConstant or OperandTypeKind.Variable as operands.", "operandTypes");
                        break;
                    case InstructionOperandCountKind.ZeroOP:
                    case InstructionOperandCountKind.OneOP:
                    case InstructionOperandCountKind.Var:
                        throw new InvalidOperationException("An InstructionForm.Long can only be InstructionOperandCountKind.TwoOP.");
                    default:
                        throw new ArgumentException(String.Format("Unknown InstructionOperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }
            }
            else if (instructionForm == InstructionFormKind.Variable)
            {
                switch (operandCount)
                {
                    case InstructionOperandCountKind.Var:
                        opcodeHead |= 0x20;
                        break;
                    case InstructionOperandCountKind.TwoOP:
                        break; // nothing to do here
                    case InstructionOperandCountKind.ZeroOP:
                    case InstructionOperandCountKind.OneOP:
                        throw new InvalidOperationException("An InstructionForm.Variable can only be InstructionOperandCountKind.TwoOP or InstructionOperandCountKind.Var.");
                    default:
                        throw new ArgumentException(String.Format("Unknown InstructionOperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }

                if (operandTypes.Count() > 0)
                {
                    opcodeOperands = 0x00;

                    for (int i = 0; i < 4; i++)
                    {
                        if (operandTypes.Count() <= i)
                        {
                            opcodeOperands |= (byte)(0x03 << ((3 - i) * 2));
                            continue;
                        }

                        switch (operandTypes[i])
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
                                throw new ArgumentException(String.Format("Unknown OperandTypeKind '{0}'", operandTypes[i].ToString()), "operandTypes");
                        }
                    }
                }                
            }
            else if (instructionForm == InstructionFormKind.Extended)
            {
                switch (operandCount)
                {
                    case InstructionOperandCountKind.Var:
                        break; // nothing to do here
                    case InstructionOperandCountKind.ZeroOP:
                    case InstructionOperandCountKind.OneOP:
                    case InstructionOperandCountKind.TwoOP:
                        throw new InvalidOperationException("An InstructionForm.Extended can only be InstructionOperandCountKind.Var.");
                    default:
                        throw new ArgumentException(String.Format("Unknown InstructionOperandCountKind '{0}'", operandCount.ToString()), "operandCount");
                }

                if (operandTypes.Count() > 0)
                {
                    opcodeOperands = 0x00;

                    for (int i = 0; i < 4; i++)
                    {
                        if (operandTypes.Count() <= i)
                        {
                            opcodeOperands |= (byte)(0x03 << ((3 - i) * 2));
                            continue;
                        }

                        switch (operandTypes[i])
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
                                throw new ArgumentException(String.Format("Unknown OperandTypeKind '{0}'", operandTypes[i].ToString()), "operandTypes");
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

        /// <summary>
        /// Measures the potential opcode and returns the size in bytes.
        /// </summary>
        /// <param name="instructionFormKind"></param>
        /// <param name="operandCount"></param>
        /// <param name="operandTypes"></param>
        /// <returns>Opcode size in bytes</returns>
        public static int MeasureOpcodeSize(InstructionFormKind instructionFormKind, InstructionOperandCountKind operandCount, params OperandTypeKind[] operandTypes)
        {
            if (instructionFormKind == InstructionFormKind.Extended)
            {
                if (operandTypes.Count() > 0)
                    return 3;
                else
                    return 2;
            }
            else if (instructionFormKind == InstructionFormKind.Variable)
            {
                if (operandTypes.Count() > 0)
                    return 2;
                else
                    return 1;
            }
            else
                return 1;
        }

        /// <summary>
        /// Takes several opcode information and returns the best match of InstructionFormKind and InstructionOperandCountKind.
        /// </summary>
        /// <param name="opcodeNumber">The opcode number.</param>
        /// <param name="opcodeType">The opcode type as written in the table of opcodes.</param>
        /// <param name="operands">The operands to use.</param>
        /// <returns>A tuple of InstructionFormKind and InstructionOperandCountKind.</returns>
        /// <exception cref="ArgumentOutOfRangeException"> If <paramref name="opcodeNumber"/> has an invalid value or <paramref name="operands"/> an invalid count of operands.</exception>
        /// <exception cref="ArgumentException"> If <paramref name="operands"/> contains an unknown enum value.</exception>
        public static Tuple<InstructionFormKind, InstructionOperandCountKind> GetFormAndCount(byte opcodeNumber, OpcodeTypeKind opcodeType, ZOperand[] operands)
        {
            InstructionFormKind instructionForm;
            InstructionOperandCountKind operandCount;

            if (opcodeType == OpcodeTypeKind.ZeroOP)
            {
                if (opcodeNumber > 0xF)
                    throw new ArgumentOutOfRangeException("opcodeNumber", opcodeNumber, "The opcode number of an OpcodeTypeKind.ZeroOP must be in range of 0x0 - 0xF.");

                if (operands.Count() > 0)
                    throw new ArgumentOutOfRangeException("operands", operands.Count(), "With OpcodeTypeKind.ZeroOP no operands must be given.");

                instructionForm = InstructionFormKind.Short;
                operandCount = InstructionOperandCountKind.ZeroOP;
            }
            else if (opcodeType == OpcodeTypeKind.OneOP)
            {
                if (opcodeNumber > 0xF)
                    throw new ArgumentOutOfRangeException("opcodeNumber", opcodeNumber, "The opcode number of an OpcodeTypeKind.OneOP must be in range of 0x0 - 0xF.");

                if (operands.Count() != 1)
                    throw new ArgumentOutOfRangeException("operands", operands.Count(), "With OpcodeTypeKind.OneOP excactly operands must be given.");

                instructionForm = InstructionFormKind.Short;
                operandCount = InstructionOperandCountKind.OneOP;
            }
            else if (opcodeType == OpcodeTypeKind.TwoOP)
            {
                if (opcodeNumber > 0x1F)
                    throw new ArgumentOutOfRangeException("opcodeNumber", opcodeNumber, "The opcode number of an OpcodeTypeKind.TwoOP must be in range of 0x00 - 0x1F.");

                if (operands.Count() != 2)
                    throw new ArgumentOutOfRangeException("operands", operands.Count(), "With OpcodeTypeKind.TwoOP exactly two operands must be given.");

                // Could be in long form for two small constants or variables
                // But let's keep this logic dumb
                // We could save 1 byte per OpcodeTypeKind.TwoOP at best
                instructionForm = InstructionFormKind.Variable;
                operandCount = InstructionOperandCountKind.TwoOP;
            }
            else if (opcodeType == OpcodeTypeKind.Var)
            {
                if (opcodeNumber > 0x1F)
                    throw new ArgumentOutOfRangeException("opcodeNumber", opcodeNumber, "The opcode number of an OpcodeTypeKind.Var must be in range of 0x00-0x1F.");

                if (operands.Count() > 4)
                {
                    if (operands.Count() <= 8)
                        throw new ArgumentOutOfRangeException("operands", operands.Count(), "With OpcodeTypeKind.Var only 0 to 4 operands can be given. Up to 8 operands are not supported yet (only call_vs2 and call_vn2 make use of this).");
                    else
                        throw new ArgumentOutOfRangeException("operands", operands.Count(), "With OpcodeTypeKind.Var only 0 to 4 operands can be given.");
                }

                instructionForm = InstructionFormKind.Variable;
                operandCount = InstructionOperandCountKind.Var;
            }
            else if (opcodeType == OpcodeTypeKind.Ext)
            {
                if (operands.Count() > 4)
                    throw new ArgumentOutOfRangeException("operands", operands.Count(), "With OpcodeTypeKind.Ext only 0 to 4 operands can be given.");

                instructionForm = InstructionFormKind.Extended;
                operandCount = InstructionOperandCountKind.Var;
            }
            else
                throw new ArgumentException(String.Format("Unknown OpcodeTypeKind '{0}'", opcodeType.ToString()), "opcodeType");

            return new Tuple<InstructionFormKind, InstructionOperandCountKind>(instructionForm, operandCount);
        }
    }
}
