using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values
{
    public class BoolValue : BaseValue
    {

        private bool _value;

        public bool Value
        {
            get { return _value; }
        }

        public BoolValue(bool value)
            : base(BaseValue.ValueTypeEnum.Bool)
        {
            _value = value;
        }


    }
}
