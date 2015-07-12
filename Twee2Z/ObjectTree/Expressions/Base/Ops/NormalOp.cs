using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Ops
{
    public class NormalOp : BaseOp
    {
        public enum NormalOpEnum
        {
            Add,
            Sub,
            Mul,
            Div,
            Mod,
        }

        private NormalOpEnum _type;

        public NormalOp(NormalOpEnum type)
            : base(OpTypeEnum.Normal, type == NormalOpEnum.Add || type == NormalOpEnum.Sub ?
            BaseOp.OpArgTypeEnum.Both : BaseOp.OpArgTypeEnum.Binary)
        {
            _type = type;
        }

        public NormalOpEnum Type { get { return _type; } }
    }
}
