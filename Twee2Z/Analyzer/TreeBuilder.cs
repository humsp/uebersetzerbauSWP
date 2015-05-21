﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree;

namespace Twee2Z.Analyzer
{
    public class TreeBuilder
    {
		public List<Passage> liste = Tree.MainTree.passlist;
        private TweeParser.StartContext startNode;
        private ObjectTree.Root root;

        public TreeBuilder(TweeParser.StartContext startNode)
        {
            this.startNode = startNode;
            root = new ObjectTree.Root();
            root.passages = new Dictionary<String, Passage>();


            walkTree(startNode.GetChild<TweeParser.PassageContext>(0));
        }

        private void walkTree(TweeParser.PassageContext passage)
        {
            if (passage == null)
            {
                return;
            }
            //Console.WriteLine("-name: " + passage.GetChild(1).GetText());
            //Console.WriteLine("-Text: " + passage.GetChild(2).GetText());
	
            //walkTree(passage.GetChild<TweeParser.PassageContext>(0));

        }

		public void BaumDurchlauf(){
			
			for (int i = 0; i < liste.Count; i++) {

				root.passages.Add (liste [i].name, liste [i]);
			}

		}
			


    }
}
