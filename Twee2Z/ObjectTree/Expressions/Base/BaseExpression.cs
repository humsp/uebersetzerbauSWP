using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions.Base
{
    public class BaseExpression
    {
        public enum BaseTypeEnum
        {
            Assign,
            Op,
            Value
        }

        private BaseTypeEnum _baseType;

        public BaseExpression(BaseTypeEnum baseType)
        {
            _baseType = baseType;
        }

        public BaseTypeEnum BaseType
        {
            get { return _baseType; }
        }


    }
}
