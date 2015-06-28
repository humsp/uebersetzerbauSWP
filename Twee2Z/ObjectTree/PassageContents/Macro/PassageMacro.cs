using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Twee2Z.ObjectTree.PassageContents.Macro.Branch;

namespace Twee2Z.ObjectTree.PassageContents.Macro
{
    public class PassageMacro : PassageContent
    {

        public enum PassageMarcroType
        {
            SetMacro,
            DisplayMacro,
            BranchMacro
        };

        private PassageMarcroType _macroType;

        /*
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

        public PassageMacro(string macro, ArrayList list)
            : base(ContentType.MacroContent)
        {
            _macro = macro;
            _macroElements = list;
        }*/

        public PassageMacro(PassageMarcroType macroType)
            : base(ContentType.MacroContent)
        {
            _macroType = macroType;
        }

        public PassageMarcroType MacroType
        {
            get { return _macroType; }
        }

        public PassageMacroBranch PassageMacroBranch
        {
            get
            {
                if (_macroType == PassageMarcroType.BranchMacro)
                {
                    return (PassageMacroBranch)this;
                }
                return null;
            }
        }

        public PassageMacroDisplay PassageMacroDisplay
        {
            get
            {
                if (_macroType == PassageMarcroType.DisplayMacro)
                {
                    return (PassageMacroDisplay)this;
                }
                return null;
            }
        }

        public PassageMacroSet PassageMacroSet
        {
            get
            {
                if (_macroType == PassageMarcroType.SetMacro)
                {
                    return (PassageMacroSet)this;
                }
                return null;
            }
        }
    }
}