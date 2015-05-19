using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class Passage
    {
        public string name;
        public string text;
        //public Macro[] macro;
        public Tags tags;
		public List<PassageNode> kids = new List<PassageNode>();
        public List<Link> links = new List<Link>();
		public Passage()
        {
            Tree.MainTree.passlist.Add(this);
        }
    
    }
}
