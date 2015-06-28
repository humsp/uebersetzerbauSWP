using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expr;

namespace Twee2Z.ObjectTree.PassageContents.Macro.Branch
{
    public class PassageMacroIf : PassageMacroBranchNode
    {
        public PassageMacroIf(Expression expression)
            : base(PassageMacroBranchNode.MacroBranchType.If, expression)
        {
        }
    }
}
