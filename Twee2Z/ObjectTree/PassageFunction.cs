using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree 
{
	class PassageFunction : PassageContent
	{

		private string _functionName;
		private string _paramList ;

		/*
		public PassageFunction()
			: base(ContentType.FunctionContent)
		{
			_functionName = null;
			_paramList = null;
		 	
		}

*/

		public PassageFunction(string functionName)
			: base(ContentType.FunctionContent)
		{
			_functionName = functionName;



		}

		public PassageFunction(string functionName, string paramList)
			: base(ContentType.FunctionContent)
		{
			_functionName = functionName;
			_paramList = paramList;

		}

		public string Function
		{
			get
			{
				return _functionName;
			}
			set
			{
				_functionName = value;
			}
		}

		public string paramList
		{
			get
			{
				return _paramList;
			}
			set
			{
				_paramList = value;
			}
		}

	}
}
