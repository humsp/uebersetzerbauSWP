using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions;

namespace Twee2Z.ObjectTree.PassageContents.Macro.Branch
{
    public class PassageMacroBranch : PassageMacro
    {
        private List<PassageMacroBranchNode> _branches = new List<PassageMacroBranchNode>();

        public PassageMacroBranch()
            : base(PassageMacro.PassageMarcroType.BranchMacro)
        {
        }

        public void AddBranch(PassageMacroBranchNode branch)
        {
            if (_branches.Count == 0 && branch.BranchType != PassageMacroBranchNode.MacroBranchType.If)
            {
                throw new Exception("A Branch needs a if first");
            }
            else if (_branches.Count != 0  &&_branches[_branches.Count - 1].BranchType == PassageMacroBranchNode.MacroBranchType.Else)
            {
                throw new Exception("After an else no branches can be addedk");
            }
            _branches.Add(branch);

        }

        public List<PassageMacroBranchNode> BranchNodeList
        {
            get { return _branches; }
        }
    }
}
