using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions;

namespace Twee2Z.ObjectTree.PassageContents.Macro.Branch
{
    public class PassageMacroBranchNode : PassageContent
    {
        public enum MacroBranchType
        {
            If,
            ElseIf,
            Else
        };

        private MacroBranchType _branchType;
        private Expression _expression;
        private List<PassageContent> _passageContentList = new List<PassageContent>();

        public PassageMacroBranchNode(MacroBranchType branchType)
            : base(PassageContent.ContentType.BranchContent)
        {
            if (branchType != MacroBranchType.Else)
            {
                throw new Exception("Only a else branch have no expression");
            }
            _branchType = branchType;
        }

        public PassageMacroBranchNode(MacroBranchType branchType, Expression expression)
            : base (PassageContent.ContentType.BranchContent)
        {
            _branchType = branchType;
            _expression = expression;
        }

        public MacroBranchType BranchType
        {
            get { return _branchType; }
        }

        public void AddPassageContent(PassageContent passageContent)
        {
            _passageContentList.Add(passageContent);
        }

        public List<PassageContent> PassageContentList
        {
            get { return _passageContentList; }
        }

        public Expression Expression
        {
            get { return _expression; }
        }
    }
}
