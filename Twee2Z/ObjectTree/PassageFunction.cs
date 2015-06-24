using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class PassageFunction : PassageContent
    {
        private string _functionName;
        private List<string> _args;
        /*
         * _functionName = context.GetChild(0).GetText();
            String _paramList
         * */

        public PassageFunction(string func)
            : base(ContentType.FunctionContent)
        {
            _func = func;
        }

        public void addArg(string argument)
        {
            if (!_args.Contains(argument))
            {
                _args.Add(argument);
            }
        }


        public string Function
        {
            get
            {
                return _func;
            }
            set
            {
                _func = value;
            }
        }

        public List<string> Target
        {
            get
            {
                return _args;
            }
            set
            {
                _args = value;
            }
        }
    }
}
