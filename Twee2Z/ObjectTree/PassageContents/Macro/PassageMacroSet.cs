using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expr;

namespace Twee2Z.ObjectTree.PassageContents.Macro
{
    public class PassageMacroSet : PassageMacro
    {
        private Expression _expression;

        public PassageMacroSet(Expression expression)
            : base(PassageMarcroType.SetMacro)
        {
            _expression = expression;
        }


        public Expression Expression
        {
            get { return _expression; }
        }
    }
}
