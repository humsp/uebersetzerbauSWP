using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class TreeValidator
    {
        private Tree _tree;


        public TreeValidator(Tree tree)
        {
            _tree = tree;
        }

        public void ValidateTree()
        {
            System.Console.WriteLine("Validate Tree:");
            validateLinks();
        }

        private void validateLinks()
        {
            System.Console.WriteLine("Validate links:");
            foreach(Passage passage in _tree.Passages.Values)
            {
                for (int i = 0; i < passage.PassageContentList.Count; i++ )
                {
                    PassageContent content = passage.PassageContentList.ElementAt(i);

                    if (content.Type == PassageContent.ContentType.LinkContent)
                    {
                        PassageLink link = content.PassageLink;
                        Passage targetPassage = _tree.GetPassage(link.Target);
                        if (targetPassage != null)
                        {
                            link.TargetPassage = targetPassage;
                        }
                        else
                        {
                            System.Console.WriteLine("Ignore Link to: " + link.Target);
                            passage.PassageContentList.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }
    }
}
