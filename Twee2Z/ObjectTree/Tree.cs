using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;

namespace Twee2Z.ObjectTree
{
    public class Tree
    {
        private Dictionary<string, Passage> _passages = new Dictionary<string, Passage>();
        private Passage _storyTitle;
        private Passage _storyAuthor;
        private Passage _start;

        public void AddPassage(Passage passage)
        {
            if (!_passages.ContainsKey(passage.Name))
            {
                switch (passage.Name)
                {
                    case "Start":
                        _start = passage;
                        break;
                    case "StoryTitle":
                        _storyTitle = passage;
                        break;
                    case "StoryAuthor":
                        _storyAuthor = passage;
                        break;
                }
                _passages.Add(passage.Name, passage);
            }
            else
            {
                Logger.LogWarning("ignoring passage with same name: " + passage.Name);
            }
        }

        public Passage GetPassage(string name)
        {
            if (!_passages.ContainsKey(name))
            {
                return null;
            }
            return _passages[name];
        }

        public Passage StartPassage
        {
            get
            {
                return _start;
            }
        }
        public Passage StoryAuthor
        {
            get
            {
                return _storyAuthor;
            }
        }
        public Passage StoryTitle
        {
            get
            {
                return _storyTitle;
            }
        }

        public Dictionary<string, Passage> Passages
        {
            get
            {
                return _passages;
            }
        }
    }
}
