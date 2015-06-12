using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instruction.Operand
{
    /// <summary>
    /// Determines type and size of an operand.
    /// See also "4.2 Operand types" on page 26 for reference.
    /// </summary>
    enum OperandTypeKind
    {
        /// <summary>
        /// A two byte constant.
        /// </summary>
        LargeConstant = 0x00,
        /// <summary>
        /// An one byte constant.
        /// </summary>
        SmallConstant = 0x01,
        /// <summary>
        /// A two byte variable. Points to the value to work with.
        /// </summary>
        Variable = 0x02,
        /// <summary>
        /// Empty value. It means that there are no futher operands. Do not use this for ZOperand.
        /// </summary>
        OmittedAltogether = 0x03
    }
}
