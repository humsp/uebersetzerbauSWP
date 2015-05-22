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
        private string _id;

        public Variable(string id)
        {
            _id = id;
        }


        public string Id
        {
            get
            {
                return _id;
            }
        }

        /*
        public Variable(String n)
        {
            name = n;
            value = 0;
            Tree.MainTree.VarList.Add(this);
        }*/
    }
}
