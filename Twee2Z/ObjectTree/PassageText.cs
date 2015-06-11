using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class PassageText : PassageContent
    {
        


        private string _text;

        public PassageText(string text)
            : base(ContentType.TextContent)
        {
            _monospace = Monospace;
            _subscript = Subscript;
            _comment = Comment;
            _italic = Italic;
            _underline = Underline;
            _strikeout = Strikeout;
            _superscript = Superscript;
            _bold = Bold;
            _text = text;
        }

        public string Text
        {
            get
            {
                return _text;
            }
        }

        public bool Italic
        {
            get
            {
                return _italic;
            }
            set
            {
                _italic = value;
            }
        }

        public bool Underline
        {
            get
            {
                return _underline;
            }
            set
            {
                _underline = value;
            }
        }

        public bool Strikeout
        {
            get
            {
                return _strikeout;
            }
            set
            {
                _strikeout = value;
            }
        }

        public bool Subscript
        {
            get
            {
                return _subscript;
            }
            set
            {
                _subscript = value;
            }
        }

        public bool Monoscpace
        {
            get
            {
                return _monospace;
            }
            set
            {
                _monospace = value;
            }
        }

        public bool Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }
    }
}
