using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values
{
    public class IntValue : BaseValue
    {
        private int _value;

        public IntValue(int value)
            : base(BaseValue.ValueTypeEnum.Int)
        {
            _value = value;
        }

        public int Value
        {
            get { return _value; }
        }
    }
}
