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

        public void ValidateTree()
        {
            Logger.LogValidation("Validate Tree");
            validateLinks();
        }

        private void validateLinks()
        {
            Logger.LogValidation("Validate links:");
            Dictionary<string, Passage> _allPassages = _tree.Passages;
            if (_tree.StartPassage != null)
            {
                _allPassages.Add("start", _tree.StartPassage);
            }
                        if (_tree.StoryTitle != null)
            {
                _allPassages.Add("StoryTitle", _tree.StoryTitle);
            }
                        if (_tree.StoryAuthor != null)
            {
                _allPassages.Add("StoryAuthor", _tree.StoryAuthor);
            }


            foreach (Passage passage in _allPassages.Values)//_tree.Passages.Values)
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
        }
    }
}
