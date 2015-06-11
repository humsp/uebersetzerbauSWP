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
        private List<MacroContentType> _content;

        public enum MacroContentType
        {
            VarContent,
            ExpContent,
            FunktionContent
        };

        
        public PassageMacro(string macro)
               : base(ContentType.MacroContext)
        {
            _macro = macro;
        }

        public void addCont(string content)
        {

           _content.Add(content);
        }

		public void addExp(string exp)
		{

			_content.Add(exp);
		}
    }
}
