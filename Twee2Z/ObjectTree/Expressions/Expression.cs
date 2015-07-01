using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions.Base;

namespace Twee2Z.ObjectTree.Expressions
{
    public class Expression
    {
        public enum TypeEnum
        {
            NameType,
            BaseExprType
        }

        private TypeEnum _type;

        public Expression(TypeEnum type)
        {
            _type = type;
        }

        public TypeEnum ExpressionType
        {
            get { return _type; }
        }

        public BaseExpression BaseExpression
        {
            get
            {
                if (ExpressionType == TypeEnum.BaseExprType)
                {
                    return (BaseExpression)(this);
                }
                return null;
            }
        }
    }
}
