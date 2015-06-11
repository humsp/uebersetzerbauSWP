using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Text;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// Jump if a is equal to any of the subsequent operands.
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, BranchAddress = {_branchLabel.TargetAddress}")]
    class Je : ZInstruction
    {
        private ZBranchLabel _branchLabel = null;

        private byte? aAsByte = null;
        private ushort? aAsUShort = null;
        private ZVariable aAsVariable = null;

        private byte? bAsByte = null;
        private ushort? bAsUShort = null;
        private ZVariable bAsVariable = null;

        private Je(ZBranchLabel branchLabel, OperandTypeKind[] operandTypes)
            : base("je", 0x01, InstructionFormKind.Variable, OperandCountKind.TwoOP, operandTypes)
        {
            _branchLabel = branchLabel;
            _subComponents.Add(branchLabel);
        }

        public Je(byte a, byte b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.SmallConstant, OperandTypeKind.SmallConstant })
        {
            aAsByte = a;
            bAsByte = b;
        }

        public Je(byte a, ushort b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.SmallConstant, OperandTypeKind.LargeConstant })
        {
            aAsByte = a;
            bAsUShort = b;
        }

        public Je(byte a, ZVariable b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.SmallConstant, OperandTypeKind.Variable })
        {
            aAsByte = a;
            bAsVariable = b;
        }

        public Je(ushort a, byte b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.LargeConstant, OperandTypeKind.SmallConstant })
        {
            aAsUShort = a;
            bAsByte = b;
        }

        public Je(ushort a, ushort b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.LargeConstant, OperandTypeKind.LargeConstant })
        {
            aAsUShort = a;
            bAsUShort = b;
        }

        public Je(ushort a, ZVariable b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.LargeConstant, OperandTypeKind.Variable })
        {
            aAsUShort = a;
            bAsVariable = b;
        }

        public Je(ZVariable a, byte b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.Variable, OperandTypeKind.SmallConstant })
        {
            aAsVariable = a;
            bAsByte = b;
        }

        public Je(ZVariable a, ushort b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.Variable, OperandTypeKind.LargeConstant })
        {
            aAsVariable = a;
            bAsUShort = b;
        }

        public Je(ZVariable a, ZVariable b, ZBranchLabel branchLabel)
            : this(branchLabel, new OperandTypeKind[] { OperandTypeKind.Variable, OperandTypeKind.Variable })
        {
            aAsVariable = a;
            bAsVariable = b;
        }

        public ZBranchLabel BranchAddress { get { return _branchLabel; } }

        public override int Size
        {
            get
            {
                int size = base.Size;

                if (aAsByte != null)
                    size += 1;
                else if (aAsUShort != null)
                    size += 2;
                else if (aAsVariable != null)
                    size += aAsVariable.Size;

                if (bAsByte != null)
                    size += 1;
                else if (bAsUShort != null)
                    size += 2;
                else if (bAsVariable != null)
                    size += bAsVariable.Size;

                return size;
            }
        }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());

            if (aAsByte != null)
            {
                byteList.Add((byte)aAsByte);
            }
            else if (aAsUShort != null)
            {
                byteList.Add((byte)(aAsUShort >> 8));
                byteList.Add((byte)aAsUShort);
            }
            else if (aAsVariable != null)
            {
                byteList.AddRange(aAsVariable.ToBytes());
            }

            if (bAsByte != null)
            {
                byteList.Add((byte)bAsByte);
            }
            else if (bAsUShort != null)
            {
                byteList.Add((byte)(bAsUShort >> 8));
                byteList.Add((byte)bAsUShort);
            }
            else if (bAsVariable != null)
            {
                byteList.AddRange(bAsVariable.ToBytes());
            }

            byteList.AddRange(_branchLabel.ToBytes());

            return byteList.ToArray();
        }
    }
}
