using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Variable
{
    enum VariableKind
    {
        /// <summary>
        /// Top of the stack
        /// </summary>
        Stack,
        /// <summary>
        /// Local variable in routine
        /// </summary>
        Local,
        /// <summary>
        /// Global variable
        /// </summary>
        Global
    }
}
