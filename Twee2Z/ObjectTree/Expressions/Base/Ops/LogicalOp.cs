using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Ops
{
    public class LogicalOp : BaseOp
    {
        public enum LocicalOpEnum
        {
            And,
            Or,
            Xor,
            Not,
            Neq,
            Eq,
            Gt,
            Ge,
            Lt,
            Le
        }

        private LocicalOpEnum _type;

        public LogicalOp(LocicalOpEnum type)
            : base(type == LocicalOpEnum.Not ? BaseOp.OpTypeEnum.Unary : BaseOp.OpTypeEnum.Binary)
        {
            _type = type;
        }
    }
}
