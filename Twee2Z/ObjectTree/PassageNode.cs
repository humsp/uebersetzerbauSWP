using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class PassageNode
    {
		public List<PassageNode> kids = new List<PassageNode>();
		public object inhalt;

		public PassageNode(object inhalt){
			this.inhalt = inhalt;
		}
    }
}
