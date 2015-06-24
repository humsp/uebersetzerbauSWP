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
        private string _args ;

            public PassageFunction(string func)
            : base(ContentType.FunctionContent)
        {
            _func = func;
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
