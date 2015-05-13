using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class Link
    {
        
        //public Passage linkToPassage;
        String ziel;
        String expression;
        String displayText;
        public Link(string s)
        {
            ziel = s;
        }
        public Link(string s1, string s2)
        {
            ziel = s1;
            displayText = s2;
        }
        public Link(string s1, string s2, string s3)
        {
            ziel = s1;
            displayText = s2;
            expression = s3;
        }
    }
}
