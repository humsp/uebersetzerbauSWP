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
            BranchMacro,
            PrintMacro
        };

        private PassageMarcroType _macroType;

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

        public PassageMacroPrint PassageMacroPrint
        {
            get
            {
                if (_macroType == PassageMarcroType.PrintMacro)
                {
                    return (PassageMacroPrint)this;
                }
                return null;
            }
        }
    }
}