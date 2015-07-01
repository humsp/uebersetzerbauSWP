using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Ops
{
    public class Op : BaseOp
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

        public Op(LocicalOpEnum type)
            : base(type == LocicalOpEnum.Add || type == LocicalOpEnum.Sub ?
            BaseOp.OpTypeEnum.Both : BaseOp.OpTypeEnum.Binary)
        {
            _type = type;
        }
    }
}
