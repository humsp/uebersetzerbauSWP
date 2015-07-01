using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.Expressions
{
    public class Expression
    {
        public enum Type
        {
            NameType,
            BaseExprType
        }

        private Type _type;

        public Type EypressionType
        {
            get { return _type; }
        }
    }
}
