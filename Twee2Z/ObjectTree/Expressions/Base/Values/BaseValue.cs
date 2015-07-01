using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base.Values
{
    public abstract class BaseValue : BaseExpression
    {

        public enum ValueTypeEnum
        {
            Var,
            Int,
            //          Float,
            String,
            Func
        }

        private ValueTypeEnum _valueType;

        public BaseValue(ValueTypeEnum valueType)
            : base(BaseTypeEnum.Value)
        {
            _valueType = valueType;
        }
    }
}
