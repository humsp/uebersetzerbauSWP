using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions.Base.Ops;
using Twee2Z.ObjectTree.Expressions.Base.Values;

namespace Twee2Z.ObjectTree.Expressions.Base
{
    public class BaseExpression : Expression
    {
        public enum BaseTypeEnum
        {
            Assign,
            Op,
            Value
        }

        private BaseTypeEnum _baseType;

        public BaseExpression(BaseTypeEnum baseType)
            : base(Expression.TypeEnum.BaseExprType)
        {
            _baseType = baseType;
        }

        public BaseTypeEnum BaseType
        {
            get { return _baseType; }
        }


        public BaseOp Assing
        {
            get
            {
                if (BaseType == BaseTypeEnum.Assign)
                {
                    return (BaseOp)(this);
                }
                return null;
            }
        }

        public BaseOp BaseOp
        {
            get
            {
                if (BaseType == BaseTypeEnum.Op)
                {
                    return (BaseOp)(this);
                }
                return null;
            }
        }

        public BaseValue BaseValue
        {
            get
            {
                if (BaseType == BaseTypeEnum.Value)
                {
                    return (BaseValue)(this);
                }
                return null;
            }
        }
    }
}
