using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expr;

namespace Twee2Z.ObjectTree.PassageContents.Macro
{
    public class PassageMacroPrint : PassageMacro
    {
        private Expression _expression;

        public PassageMacroPrint(Expression expression)
            : base(PassageMarcroType.PrintMacro)
        {
            _expression = expression;
        }


        public Expression Expression
        {
            get { return _expression; }
        }
    }
}
