using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Instruction
{
    /// <summary>
    /// Represents a routine in Z-Code. Must be labeled for jumping in Z-Code execution (you can omit to do so but an unlabeled routine cannot be called).
    /// A routine consists of a local variable count and multiple instructions.
    /// See also "5. How routines are encoded" on page 31 for reference.
    /// </summary>
    [DebuggerDisplay("LabelName = {_label.Name}, LocalVariableCount = {_localVariableCount}, InstructionCount = {_subComponents.Count}")]
    class ZRoutine : ZLabeledComponent
    {
        protected byte _localVariableCount = 0;

        /// <summary>
        /// Creates a new instance of a ZRoutine with no instructions.
        /// </summary>
        public ZRoutine()
        {
        }

        /// <summary>
        /// Creates a new instance of a ZRoutine with instructions.
        /// </summary>
        /// <param name="instructions">The instructions to add.</param>
        public ZRoutine(IEnumerable<ZInstruction> instructions)
        {
            _subComponents.AddRange(instructions);
        }

        /// <summary>
        /// Creates a new instance of a ZRoutine with instructions and a local variable count.
        /// </summary>
        /// <param name="instructions">The instructions to add.</param>
        /// <param name="localVariableCount">The local variable count. Must be in range of 0 - 15.</param>
        public ZRoutine(IEnumerable<ZInstruction> instructions, byte localVariableCount)
            : this()
        {
            if (localVariableCount > 15)
                throw new ArgumentOutOfRangeException("localVariableCount", localVariableCount, "A routine has between 0 and 15 local variables.");

            _subComponents.AddRange(instructions);
            _localVariableCount = localVariableCount;
        }

        /// <summary>
        /// Gets or sets the count of the local variables. Must be in range of 0 - 15.
        /// </summary>
        public byte LocalVariableCount
        {
            get
            {
                return _localVariableCount;
            }
            set
            {
                if (value > 15)
                    throw new ArgumentOutOfRangeException("value", value, "A routine has between 0 and 15 local variables.");

                _localVariableCount = value;
            }
        }

        /// <summary>
        /// Gets the instructions in this routine.
        /// </summary>
        public ReadOnlyCollection<ZInstruction> Instructions
        {
            get
            {
                return _subComponents.OfType<ZInstruction>().ToList().AsReadOnly();
            }
        }

        public override Byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            byteList.Add(_localVariableCount);

            foreach (ZInstruction instruction in _subComponents)
            {
                byteList.AddRange(instruction.ToBytes());
            }

            // Routines use packed addresses. Therefore additionial bytes might have to be added for padding.
            byteList.AddRange(new byte[8 - byteList.Count % 8]);

            return byteList.ToArray();
        }

        public override int Size
        {
            get
            {
                // One byte for the local variable count + all instructions
                int size = 1 + _subComponents.Sum(component => component.Size);
                // Plus additional padding if needed
                return size + (8 - size % 8);
            }
        }

        public override void SetLabel(int absoluteAddr, string name)
        {
            if (_label == null)
                _label = new ZLabel(name, new ZPackedAddress(absoluteAddr));
            else if (_label.TargetAddress == null)
            {
                _label.TargetAddress = new ZPackedAddress(absoluteAddr);
                _label.Name = name;
            }
            else
            {
                _label.TargetAddress.Absolute = absoluteAddr;
                _label.Name = name;
            }
        }

        public override void Setup(int currentAddress)
        {
            base.Setup(currentAddress + 1); // dont forget the first byte (local variables counter)
        }
    }
}
