using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values
{
    public class VariableValue : BaseValue
    {
        public VariableValue(string name)
            : base(ValueTypeEnum.Var)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
