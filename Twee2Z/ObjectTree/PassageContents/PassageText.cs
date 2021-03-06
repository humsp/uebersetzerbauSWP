﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree.PassageContents
{
    public class PassageText : PassageContent
    {

        private string _text = "";

        public PassageText(string text)
            : base(ContentType.TextContent)
        {
            _text = text;
        }

        public string Text
        {
            get
            {
                return _text;
            }
        }

        public void MergePassageText(PassageText passageText)
        {
            _text += passageText.Text;
        }
    }
}
