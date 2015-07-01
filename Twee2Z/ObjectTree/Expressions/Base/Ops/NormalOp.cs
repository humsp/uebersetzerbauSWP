using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Ops
{
    public class NormalOp : BaseOp
    {
        public enum LocicalOpEnum
        {
            Add,
            Sub,
            Mul,
            Div,
            Mod,
        }

        private LocicalOpEnum _type;

        public NormalOp(LocicalOpEnum type)
            : base(OpTypeEnum.Normal, type == LocicalOpEnum.Add || type == LocicalOpEnum.Sub ?
            BaseOp.OpArgTypeEnum.Both : BaseOp.OpArgTypeEnum.Binary)
        {
            _type = type;
        }
    }
}
