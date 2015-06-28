using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree;
using Twee2Z.Utils;
using Twee2Z.ObjectTree.PassageContents;
using Twee2Z.ObjectTree.PassageContents.Macro;
using Twee2Z.ObjectTree.PassageContents.Macro.Branch;

namespace Twee2Z.Analyzer
{
    class TweeBuilder
    {
        private Passage _currentPassage;
        private PassageContent _lastPassageContent;
        private List<PassageContent> _passageContentMacroStack = new List<PassageContent>();
        private PassageContentFormat _currentFormat;

        private Tree _tree;

        public TweeBuilder()
        {
            _tree = new Tree();
        }

        public Tree Tree
        {
            get { return _tree; }
        }

        public PassageContentFormat CurrentFormat
        {
            get
            {
                if (_currentFormat == null)
                {
                    _currentFormat = new PassageContentFormat();
                }
                return _currentFormat;
            }
        }

        private int MacroStackCount
        {
            get { return _passageContentMacroStack.Count; }
        }

        public void AddTag(string tag)
        {
            _currentPassage.AddTag(tag);
        }

        public void AddPassage(Passage passage)
        {
            if (_passageContentMacroStack.Count != 0)
            {
                Logger.LogWarning("A macro isn´t closed before passage " + passage.Name + " begins");
            }
            _passageContentMacroStack.Clear();
            _lastPassageContent = null;
            _currentPassage = passage;
            _currentFormat = new PassageContentFormat();
            _tree.AddPassage(passage);
        }

        public void AddPassageContent(PassageContent passageContent)
        {
            // set format
            passageContent.ContentFormat = _currentFormat;
            _currentFormat = _currentFormat.Copy();

            PassageContent lastContent = _lastPassageContent;

            // merge text content together
            if (lastContent != null &&
                lastContent.Type == PassageContent.ContentType.TextContent &&
                passageContent.Type == PassageContent.ContentType.TextContent &&
                CurrentFormat.euquals(lastContent.ContentFormat))
            {
                lastContent.PassageText.MergePassageText(passageContent.PassageText);
            }
            else if (MacroStackCount == 0)
            {
                _currentPassage.AddPassageContent(passageContent);

                // put on stack if macro branch
                if (passageContent.Type == PassageContent.ContentType.MacroContent &&
                    passageContent.PassageMacro.MacroType == PassageMacro.PassageMarcroType.BranchMacro)
                {
                    _passageContentMacroStack.Add(passageContent);
                }
            }
            else if (lastContent.Type == PassageContent.ContentType.MacroContent &&
                    lastContent.PassageMacro.MacroType == PassageMacro.PassageMarcroType.BranchMacro)
            {
                PassageMacro macro = _passageContentMacroStack.Last().PassageMacro;
                PassageMacroBranch branch = macro.PassageMacroBranch;

                branch.AddBranch(passageContent.PassageBranch);

                _passageContentMacroStack.Add(passageContent);
            }
            // stack: macro branch if - input: else ifelse
            else if (_passageContentMacroStack[_passageContentMacroStack.Count - 2].Type == PassageContent.ContentType.MacroContent &&
                    _passageContentMacroStack[_passageContentMacroStack.Count - 2].PassageMacro.MacroType == PassageMacro.PassageMarcroType.BranchMacro &&
                    _passageContentMacroStack.Last().Type == PassageContent.ContentType.BranchContent &&
                    _passageContentMacroStack.Last().PassageBranch.BranchType != PassageMacroBranchNode.MacroBranchType.Else &&
                    passageContent.Type == PassageContent.ContentType.BranchContent &&
                    passageContent.PassageBranch.BranchType != PassageMacroBranchNode.MacroBranchType.If)
            {
                FinishBranchStatement();
                PassageMacroBranch branch = _passageContentMacroStack.Last().PassageMacro.PassageMacroBranch;

                branch.AddBranch(passageContent.PassageBranch);

                _passageContentMacroStack.Add(passageContent);

            }
            else if (_passageContentMacroStack.Last().Type == PassageContent.ContentType.BranchContent)
            {
                PassageMacroBranchNode branchNode = _passageContentMacroStack.Last().PassageBranch;
                branchNode.AddPassageContent(passageContent);
            }
            else
            {
                throw new Exception("unknown case");
            }

            _lastPassageContent = passageContent;
        }

        private void FinishBranchStatement()
        {
            _passageContentMacroStack.RemoveAt(_passageContentMacroStack.Count - 1);

            if (_passageContentMacroStack.Count == 0)
            {
                _lastPassageContent = _currentPassage.PassageContentList.Last();
                _currentFormat = _lastPassageContent.ContentFormat.Copy();
            }
            else
            {
                _lastPassageContent = _passageContentMacroStack.Last();
                _currentFormat = _lastPassageContent.ContentFormat.Copy();
            }
        }

        /// <summary>
        /// Call this method, if a endIf statement occurs
        /// </summary>
        public void FinishBranch()
        {
            // stack: branch (if/else) // pop both
            FinishBranchStatement();
            FinishBranchStatement();
        }
    }
}
