using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions.Base.Values;

namespace Twee2Z.ObjectTree.Expressions.Base
{
    public class Assign : BaseExpression
    {
        public enum AssignTypeEnum
        {
            AssignEq,
            AssignAdd,
            AssignSub,
            AssignMul,
            AssignDiv,
            AssignMod
        }

        private AssignTypeEnum _assignType;
        private VariableValue _variable;
        private BaseExpression _expr;

        public Assign(AssignTypeEnum type)
            : base(BaseExpression.BaseTypeEnum.Assign)
        {
            _assignType = type;
        }

        public VariableValue Variable
        {
            get { return _variable; }
            set { _variable = value; }
        }

        public BaseExpression Expr
        {
            get { return _expr; }
            set { _expr = value; }
        }

        public AssignTypeEnum AssignType { get { return _assignType; } }
    }
}
