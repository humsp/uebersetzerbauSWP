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



        protected List<BaseExpression> _args = new List<BaseExpression>();
        private OpArgTypeEnum _opArgType;
        private OpTypeEnum _opType;

        public BaseOp(OpTypeEnum opType, OpArgTypeEnum argType)
            : base(BaseExpression.BaseTypeEnum.Op)
        {
            _opType = opType;
            _opArgType = argType;
        }


        public void AddArg(BaseExpression expr)
        {
            if (OpArgType == OpArgTypeEnum.Unary && _args.Count == 0  ||
                OpArgType != OpArgTypeEnum.Unary && _args.Count < 1)
            {
                _args.Add(expr);
            }
            else
            {
                throw new Exception("invalid arg count");
            }
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
