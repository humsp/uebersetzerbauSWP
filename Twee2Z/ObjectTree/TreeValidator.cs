using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;

namespace Twee2Z.ObjectTree
{
    public class TreeValidator
    {
        private Tree _tree;


        public TreeValidator(Tree tree)
        {
            _tree = tree;
        }

        public bool ValidateTree()
        {
            Logger.LogValidation("Validate Tree");
            return validateStartPassage() && validateLinks();
        }

        private bool validateStartPassage()
        {
            if (_tree.StartPassage != null)
            {
                Logger.LogError("There ist no passage called 'Start'");
                return false;
            }
            return true;
        }

        private bool validateLinks()
        {
            Logger.LogValidation("Validate links:");
            foreach(Passage passage in _tree.Passages.Values)
            {
                for (int i = 0; i < passage.PassageContentList.Count; i++ )
                {
                    PassageContent content = passage.PassageContentList[i];

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
                            Logger.LogWarning("Ignore Link to: " + link.Target);
                            if(link.DisplayText != null)
                            {
                                passage.PassageContentList[i] = new PassageText(link.DisplayText);
                            }
                            else
                            {
                                passage.PassageContentList.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
