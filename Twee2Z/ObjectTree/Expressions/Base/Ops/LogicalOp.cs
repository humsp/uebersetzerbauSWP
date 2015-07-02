﻿using System;
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

        public LogicalOp(LocicalOpEnum type)
            : base(OpTypeEnum.Logical, type == LocicalOpEnum.Not ? BaseOp.OpArgTypeEnum.Unary : BaseOp.OpArgTypeEnum.Binary)
        {
            Type = type;
        }

        public LocicalOpEnum Type { get; set; }
    }
}
