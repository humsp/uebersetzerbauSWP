using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Variable;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;

namespace Twee2Z.CodeGen.Instruction
{
    /// <summary>
    /// Represents an store instruction (opcode + operands + store) in Z-Code. It can be labeled for jumping in Z-Code execution.
    /// See also "4. How instructions are encoded" on page 26 for reference.
    /// See also "14. Complete table of opcodes" on page 70 for reference.
    /// </summary>
    [DebuggerDisplay("Opcode = {_opcode}, Operands = {_operands}, Store = {_store}")]
    class ZInstructionSt : ZInstruction
    {
        protected ZVariable _store;

        /// <summary>
        /// Creates a new instance of a ZInstructionSt. Check the complete table of opcodes for valid values.
        /// </summary>
        /// <param name="name">The Inform name for the used opcode. It is used for debugging purposes only, so you COULD leave this empty or null.</param>
        /// <param name="opcodeNumer">The opcode number.</param>
        /// <param name="opcodeType">The opcode type.</param>
        /// <param name="store">The store variable.</param>
        /// <param name="operands">The operands to use.</param>
        public ZInstructionSt(string name, byte opcodeNumber, OpcodeTypeKind opcodeType, ZVariable store, params ZOperand[] operands)
            : base(name, opcodeNumber, opcodeType, operands)
        {
            _store = store;
            _subComponents.Add(store);
        }

        public ZVariable Store
        {
            get
            {
                return _store;
            }
        }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.AddRange(base.ToBytes());
            byteList.AddRange(_store.ToBytes());

            return byteList.ToArray();
        }
    }
}
