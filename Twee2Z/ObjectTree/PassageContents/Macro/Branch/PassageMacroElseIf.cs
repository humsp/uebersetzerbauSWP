using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions;

namespace Twee2Z.ObjectTree.PassageContents.Macro.Branch
{
    public class PassageMacroElseIf : PassageMacroBranchNode
    {
        public PassageMacroElseIf(Expression expression)
            : base(PassageMacroBranchNode.MacroBranchType.ElseIf, expression)
        {
        }
    }
}
