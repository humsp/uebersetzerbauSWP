using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Twee2Z.ObjectTree
{
    public class PassageMacro : PassageContent
    {

        private string _macro;
        private ArrayList _macroElements = new ArrayList();


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

        public ArrayList MacroElements
        {
            get
            {
                return _macroElements;
            }
        }
        /*public int TextCount
        {
            get
            {
                return TextCount;
            }
        }*/


        public PassageMacro(string macro, ArrayList list)
            : base(ContentType.MacroContent)
        {
            _macro = macro;
            _macroElements = list;
        }

    }
}