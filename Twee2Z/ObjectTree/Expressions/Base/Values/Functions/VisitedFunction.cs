using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values.Functions
{
    public class VisitedFunction : FunctionValue
    {
        public VisitedFunction()
            : base(FunctionTypeEnum.Visited)
        { }

        public override int MaxArgCount
        {
            get { return 0; }
        }
    }
}
