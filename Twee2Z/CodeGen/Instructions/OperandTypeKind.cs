﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Instructions
{
    public enum OperandTypeKind
    {
        LargeConstant,
        SmallConstant,
        Variable,
        OmittedAltogether
    }
}
