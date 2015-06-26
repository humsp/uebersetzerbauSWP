using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.PassageContents.Macro
{
    public class PassageMacroDisplay : PassageMacro
    {
        private string _target;
        private Passage _targetPassage;


        public PassageMacroDisplay(string target)
            : base(PassageMarcroType.Display)
        {
            _target = target;
        }


        public string Target
        {
            get { return _target; }
        }

        public Passage TargetPassage
        {
            get { return _targetPassage; }
            set { _targetPassage = value; }
        }
    }
}
