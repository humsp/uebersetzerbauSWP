using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values
{
    public class StringValue : BaseValue
    {
        private string _value;

        public StringValue(string value)
            : base(BaseValue.ValueTypeEnum.String)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }
}
