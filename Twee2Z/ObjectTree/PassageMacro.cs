using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    class PassageMacro : PassageContent
    {

        private string _macro;

        
        public string Macro
		{
			get
			{
				return _macro;
			}
			set
			{
				_macro = value;
			}
		}


        
        public PassageMacro(string macro)
               : base(ContentType.MacroContent)
        {
            _macro = macro;
        }

    }
}
