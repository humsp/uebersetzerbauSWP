using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;
using Twee2Z.ObjectTree.PassageContents;

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
            return validateStartPassage() && 
                validateStoryTitle() &&
                validateStoryAuthor() &&
                validateLinks();
        }

        private bool validateStartPassage()
        {
            if (_tree.StartPassage == null)
            {
                Logger.LogError("There ist no passage called 'Start'");
                return false;
            }
            return true;
        }

        private bool validateStoryTitle()
        {
            if (_tree.StoryTitle == null)
            {
                Logger.LogWarning("No story title passage found");
                Passage storyTitle = new Passage("StoryTitle");
                storyTitle.AddPassageContent(new PassageText("Unknown title"));
                _tree.AddPassage(storyTitle);
            }
            return true;
        }

        private bool validateStoryAuthor()
        {
            if (_tree.StoryAuthor == null)
            {
                Logger.LogWarning("No story author passage found");
                Passage storyTitle = new Passage("StoryAuthor");
                storyTitle.AddPassageContent(new PassageText("Unknown author"));
                _tree.AddPassage(storyTitle);
            }
            return true;
        }

        private bool validateLinks()
        {
            Logger.LogValidation("Validate links:");
            Dictionary<string, Passage> _allPassages = _tree.Passages;

            foreach (Passage passage in _allPassages.Values)
            {
                for (int i = 0; i < passage.PassageContentList.Count; i++)
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

                            string name = link.DisplayText != null ? link.DisplayText : link.Target;

                            PassageText passageText = new PassageText(name);
                            passageText.ContentFormat = passage.PassageContentList[i].ContentFormat;

                            passage.PassageContentList[i] = passageText;
                        }
                    }
                }
            }
            return true;
        }
    }
}
