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
        private bool _italic;
        private bool _underline;
        private bool _strikeout;
        private bool _subscript;
        private bool _monospace;
        private bool _comment;

        /*
: (ITALIC_BEGIN	(ITALIC_TEXT_SWITCH | ITALIC_TEXT | passageContent)* ITALIC_END)
| (UNDERLINE_BEGIN (UNDERLINE_TEXT_SWITCH | UNDERLINE_TEXT | passageContent)* UNDERLINE_END)
| (STRIKEOUT_BEGIN (STRIKEOUT_TEXT_SWITCH | STRIKEOUT_TEXT | passageContent)*	STRIKEOUT_END)	
| (SUPERSCRIPT_BEGIN (SUPERSCRIPT_TEXT_SWITCH | SUPERSCRIPT_TEXT | passageContent)* SUPERSCRIPT_END)
| (SUBSCRIPT_BEGIN (SUBSCRIPT_TEXT_SWITCH | SUBSCRIPT_TEXT | passageContent)* SUBSCRIPT_END)
| (MONOSPACE_BEGIN (MONOSPACE_TEXT_SWITCH | MONOSPACE_TEXT | passageContent)* MONOSPACE_END)
| (COMMENT_BEGIN (COMMENT_TEXT_SWITCH | COMMENT_TEXT | passageContent)* COMMENT_END)
;
 */


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
