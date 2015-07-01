using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;

namespace Twee2Z.ObjectTree.Expressions.Base.Values
{
    public abstract class FunctionValue : BaseValue
    {
        public enum FunctionTypeEnum
        {
            Random,
            Previous,
            Visited,
            Turns,
            Passage,
            Confirm,
            Prompt,
            Alert,
            Parameter,
            Either
        }
     
        private FunctionTypeEnum _functionType;

        protected List<BaseExpression> _args = new List<BaseExpression>();

        public FunctionValue(FunctionTypeEnum type)
            : base(BaseValue.ValueTypeEnum.Func)
        {
            _functionType = type;
        }

        public virtual void AddArg(BaseExpression expr)
        {
            if (_args.Count > MaxArgCount)
            {
                Logger.LogWarning(_functionType  + "-function can only have " + MaxArgCount + " arguments");
            }
            else
            {
                _args.Add(expr);
            }
        }

        public abstract int MaxArgCount {get;}
    }
}
