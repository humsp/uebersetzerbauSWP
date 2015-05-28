﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class Tree
    {
        private const string _startPassageId = "start";

        private Dictionary<string, Passage> _passages = new Dictionary<string, Passage>();
        //private Dictionary<string, Variable> _variables = new Dictionary<string, Variable>();

        public void AddPassage(Passage passage)
        {
            if (!_passages.ContainsKey(passage.Name))
            {
                _passages.Add(passage.Name, passage);
            }
            else
            {
                System.Console.WriteLine("WARNING: ignoring passage with same name: " + passage.Name);
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

        // getter

        public Passage StartPassage
        {
            get
            {
                return _passages[_startPassageId];
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
