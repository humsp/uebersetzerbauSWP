using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree 
{
    class PassageFunction : PassageContent
    {
        private string _func;
        private List<string> _args ;

            public PassageFunction(string func)
            : base(ContentType.FunctionContent)
        {
            _func = func;
        }

        public void addArg (string argument){

                    
            if (!_args.Contains(argument))
            {
                _args.Add(argument);
            }
            else
            {
                Logger.LogWarning("The Argument "+ tag + " exists already.");
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
