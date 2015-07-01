using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expr;

namespace Twee2Z.ObjectTree.PassageContents.Macro
{
    public class PassageMacroDisplay : PassageMacro
    {
        private Expression _expression;

        public PassageMacroDisplay(Expression expression)
            : base(PassageMarcroType.DisplayMacro)
        {
            _expression = expression;
        }


        public Expression Expression
        {
            get { return _expression; }
        }
    }
}
