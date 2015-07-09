using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values.Functions
{
    public class ConfirmFunction : FunctionValue
    {
        public ConfirmFunction()
            : base(FunctionTypeEnum.Confirm)
        { }

        public override int MaxArgCount
        {
            get { return 1; }
        }
    }
}
