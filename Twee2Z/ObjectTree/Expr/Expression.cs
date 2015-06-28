using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expr
{
    public class Expression
    {
        private string _expression;

        public Expression(string expression)
        {
            _expression = expression;
        }

        public string ExpressionString
        {
            get { return _expression; }
        }
    }
}
