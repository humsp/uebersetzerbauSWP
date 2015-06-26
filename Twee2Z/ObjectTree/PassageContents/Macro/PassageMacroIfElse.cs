using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expr;

namespace Twee2Z.ObjectTree.PassageContents.Macro
{
    public class PassageMacroIfElse : PassageMacro
    {
        private Expression _expression;
        private List<PassageContent> _passageContentIfList = new List<PassageContent>();
        private List<PassageContent> _passageContentElseList = new List<PassageContent>();

        public PassageMacroIfElse(Expression expression)
            : base(PassageMarcroType.IfElse)
        {
            _expression = expression;
        }

        public void AddPassageContentIf(PassageContent passageContent)
        {
            _passageContentIfList.Add(passageContent);
        }

        public void AddPassageContentElse(PassageContent passageContent)
        {
            _passageContentElseList.Add(passageContent);
        }

        public List<PassageContent> PassageContentIfList
        {
            get { return _passageContentIfList; }
        }

        public List<PassageContent> PassageContentElseList
        {
            get { return _passageContentElseList; }
        }

        public Expression Expression
        {
            get { return _expression; }
        }
    }
}
