using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;

namespace Twee2Z.ObjectTree.Expressions.Base.Ops
{
    public class BaseOp : BaseExpression
    {
        public enum OpTypeEnum
        {
            Unary,
            Binary,
            Both
        }

        protected List<BaseExpression> _args = new List<BaseExpression>();
        private OpTypeEnum _opType;

        public BaseOp(OpTypeEnum type)
            : base(BaseExpression.BaseTypeEnum.Op)
        {
            _opType = type;
        }


        public void AddArg(BaseExpression expr)
        {
            if (OpType == OpTypeEnum.Unary && _args.Count == 0  ||
                OpType != OpTypeEnum.Unary && _args.Count < 1)
            {
                _args.Add(expr);
            }
            else
            {
                throw new Exception("invalid arg count");
            }
        }

        public OpTypeEnum OpType
        {
            get { return _opType; }
        }
    }
}
