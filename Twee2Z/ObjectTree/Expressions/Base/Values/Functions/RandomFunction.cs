using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values.Functions
{
    public class RandomFunction : FunctionValue
    {
        private int _min;
        private int _max;

        public RandomFunction()
            : base(FunctionTypeEnum.Random)
        { }

        public override int MaxArgCount
        {
            get { return 2; }
        }
    }
}
