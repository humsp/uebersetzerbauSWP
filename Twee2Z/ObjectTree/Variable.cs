using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class Variable
    {
        public int value;
        public string name;
        
        public Variable(String n)
        {
            name = n;
            value = 0;
            Tree.MainTree.VarList.Add(this);
        }
    }
}
