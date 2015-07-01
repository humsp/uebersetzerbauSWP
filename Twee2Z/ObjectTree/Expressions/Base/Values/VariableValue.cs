using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values
{
    public class VariableValue : BaseValue
    {
        private string _name;

        public VariableValue(string name)
            : base(ValueTypeEnum.Var)
        {

        }

    }
}
