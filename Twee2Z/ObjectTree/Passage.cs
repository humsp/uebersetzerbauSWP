using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class Passage
    {
        public List<Passage> passlist = new List<Passage>();
        public string name;
        public string text;
        //public Macro[] macro;
        public Tags tags;
        public List<Link> links = new List<Link>();
        public Passage()
        {
            passlist.Add(this);
        }
    
    }
}
