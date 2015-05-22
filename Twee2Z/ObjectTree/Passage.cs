using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _passageContentList.Add(passageContent);
        }

        public void AddTag(string tag)
        {
            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
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
    }
}
