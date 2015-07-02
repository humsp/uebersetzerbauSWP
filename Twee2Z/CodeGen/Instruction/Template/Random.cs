using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Operand;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen.Instruction.Template
{
    /// <summary>
    /// If range is positive, returns a uniformly random number between 1 and range. If range is negative,
    /// the random number generator is seeded to that value and the return value is 0. Most interpreters
    /// consider giving 0 as range illegal (because they attempt a division with remainder by the
    /// range).
    /// <para>
    /// See also "random" on page 94 for reference.
    /// </para>
    /// </summary>
    [DebuggerDisplay("Name = {_opcode.Name}, Range = {_range}, Store = {_store}")]
    class Random : ZInstructionSt
    {
        short _range;

        /// <summary>
        /// Creates a new instance of a Random instruction.
        /// </summary>
        /// <param name="range">If range is positive, returns a uniformly random number between 1 and range. If range is negative, the random number generator is seeded to that value and the return value is 0.</param>
        /// <param name="store">The variable where the result will be stored.</param>
        public Random(short range, ZVariable store)
            : base("random", 0x07, OpcodeTypeKind.Var, store, new ZOperand(range))
        {
            _range = range;
        }

        /// <summary>
        /// Gets the range used for randomization.
        /// </summary>
        public short Range { get { return _range; } }
    }
}
