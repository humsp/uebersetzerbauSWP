using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction
{
    /// <summary>
    /// Represents an store and branch instruction (opcode + operands + store + branch) in Z-Code. It can be labeled for jumping in Z-Code execution.
    /// See also "4. How instructions are encoded" on page 26 for reference.
    /// See also "14. Complete table of opcodes" on page 70 for reference.
    /// </summary>
    [DebuggerDisplay("Opcode = {_opcode}, Operands = {_operands}, Store = {_store}, Branch = {_branch}")]
    class ZInstructionStBr : ZInstruction
    {
        protected ZVariable _store;
        protected ZLabel _branch;

        /// <summary>
        /// Creates a new instance of a ZInstructionStBr. Check the complete table of opcodes for valid values.
        /// </summary>
        /// <param name="name">The Inform name for the used opcode. It is used for debugging purposes only, so you COULD leave this empty or null.</param>
        /// <param name="opcodeNumer">The opcode number.</param>
        /// <param name="opcodeType">The opcode type.</param>
        /// <param name="store">The store variable.</param>
        /// <param name="branch">The branch label.</param>
        /// <param name="operands">The operands to use.</param>
        public ZInstructionStBr(string name, byte opcodeNumber, OpcodeTypeKind opcodeType, ZVariable store, ZLabel branch, params ZOperand[] operands)
            : base(name, opcodeNumber, opcodeType, operands)
        {
            _store = store;
            _branch = branch;
            _subComponents.Add(store);
            _subComponents.Add(branch);
        }

        public ZVariable Store
        {
            get
            {
                return _store;
            }
        }

        public ZLabel Branch
        {
            get
            {
                return _branch;
            }
        }

        public override int Size
        {
            get
            {
                return base.Size;
                //return base.Size + _store.Size + _branch.Size;
            }
        }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.AddRange(_store.ToBytes());
            byteList.AddRange(_branch.ToBytes());

            return byteList.ToArray();
        }
    }
}
