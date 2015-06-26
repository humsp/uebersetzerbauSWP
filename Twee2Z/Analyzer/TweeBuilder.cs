using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree;
using Twee2Z.Utils;

namespace Twee2Z.Analyzer
{
    class TweeBuilder
    {
        private Passage _currentPassage;
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

        public void AddTag(string tag)
        {
            _currentPassage.AddTag(tag);
        }

        public void AddPassage(Passage passage)
        {
            _currentPassage = passage;
            _currentFormat = new PassageContentFormat();
            _tree.AddPassage(passage);
        }

        public PassageContentFormat CurrentFormat
        {
            get { return _currentFormat; }
        }

        public void AddPassageContent(PassageContent passageContent)
        {
            PassageContent lastContent = LastPassageContent();
            if (lastContent != null &&
                lastContent.Type == PassageContent.ContentType.TextContent &&
                passageContent.Type == PassageContent.ContentType.TextContent &&
                _currentFormat.euquals(lastContent.ContentFormat))
            {
                lastContent.PassageText.MergePassageText(passageContent.PassageText);
            }
            else
            {
                passageContent.ContentFormat = _currentFormat;
                _currentFormat = _currentFormat.Copy();
                _currentPassage.AddPassageContent(passageContent);
            }
        }

        private PassageContent LastPassageContent()
        {
            if (_currentPassage.PassageContentList.Count == 0)
            {
                return null;
            }
            return _currentPassage.PassageContentList[_currentPassage.PassageContentList.Count - 1];
        }
    }
}
