using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class Tree
    {
        static public Tree MainTree;
        public List<Passage> passlist = new List<Passage>();
        public List<Variable> VarList = new List<Variable>();

        public Tree()
        {
            MainTree = this;
        }
    }
}
