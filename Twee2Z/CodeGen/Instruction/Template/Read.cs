using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen.Instruction.Template
{
    [DebuggerDisplay("Name = {_opcode.Name}")]
    class Read : ZInstruction
    {
        /// <summary>
        /// 
        /// </summary>
        public Read()
            : base("read", 0x04, InstructionFormKind.Variable, OperandCountKind.Var, new OperandTypeKind[] { OperandTypeKind.SmallConstant, OperandTypeKind.SmallConstant, OperandTypeKind.SmallConstant, OperandTypeKind.SmallConstant })
        {
        }

        public override int Size
        {
            get
            {
                return base.Size + 4;
            }
        }

        public override byte[] ToBytes()
        {
            List<byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.Add(1);
            byteList.Add(0);
            byteList.Add(0);
            byteList.AddRange(new ZStackVariable().ToBytes());

            return byteList.ToArray();
        }
    }
}
