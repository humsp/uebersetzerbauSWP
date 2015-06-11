using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;

namespace Twee2Z.ObjectTree
{
    public class Passage
    {
        private string _name;
        private HashSet<string> _tags = new HashSet<string>();
		private List<PassageContent> _passageContentList = new List<PassageContent>();

		public Passage(String name)
        {
            _name = name;
        }
    
        public void AddPassageContent(PassageContent passageContent)
        {
            int count = _passageContentList.Count;
            if (passageContent.Type == PassageContent.ContentType.TextContent &&
                count != 0 &&
                _passageContentList.Last().Type == PassageContent.ContentType.TextContent)
            {
                String text = _passageContentList.Last().PassageText.Text + 
                    passageContent.PassageText.Text;
                _passageContentList[count - 1] = new PassageText(text);
            }
            else
            {
                _passageContentList.Add(passageContent);
            }
        }

        public void AddTag(string tag)
        {
            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
            }
            else
            {
                Logger.LogWarning("ignoring tag: " + tag + " for passage:" + Name);
             }
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public List<PassageContent> PassageContentList
        {
            get
            {
                return _passageContentList;
            }
        }

        public HashSet<string> Tags
        {
            get
            {
                return _tags;
            }
        }
    }
}
