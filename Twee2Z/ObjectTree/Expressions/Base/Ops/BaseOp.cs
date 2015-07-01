using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;
using Twee2Z.ObjectTree.Expressions.Base.Ops;

namespace Twee2Z.ObjectTree.Expressions.Base.Ops
{
    public class BaseOp : BaseExpression
    {
        public enum OpArgTypeEnum
        {
            Unary,
            Binary,
            Both
        }

        public enum OpTypeEnum
        {
            Logical,
            Normal
        }

        /// <summary>
        /// if unary, left is null
        /// </summary>
        private BaseExpression _leftExpr;
        private BaseExpression _rightExpr;

        private OpArgTypeEnum _opArgType;
        private OpTypeEnum _opType;

        public BaseOp(OpTypeEnum opType, OpArgTypeEnum argType)
            : base(BaseExpression.BaseTypeEnum.Op)
        {
            _opType = opType;
            _opArgType = argType;
        }

        public BaseExpression LeftExpr
        {
            get { return _leftExpr; }
            set
            {
                if (OpArgType == OpArgTypeEnum.Unary)
                {
                    throw new Exception("for unary op left expr is not used");
                }
                _leftExpr = value;
            }
        }

        public BaseExpression RightExpr
        {
            get { return _rightExpr; }
            set { _rightExpr = value; }
        }

        public OpArgTypeEnum OpArgType
        {
            get { return _opArgType; }
        }

        public OpTypeEnum OpType
        {
            get { return _opType; }
        }

        public LogicalOp Logical
        {
            get
            {
                if (OpType == OpTypeEnum.Logical)
                {
                    return (LogicalOp)this;
                }
                return null;
            }
        }

        public NormalOp Normal
        {
            get
            {
                if (OpType == OpTypeEnum.Normal)
                {
                    return (NormalOp)this;
                }
                return null;
            }
        }
    }
}
