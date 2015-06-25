using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class PassageFormat : PassageContent
    {
        private bool _monospace = false;

        public bool Monospace
        {
            get { return _monospace; }
            set { _monospace = value; }
        }
        private bool _subscript = false;

        public bool Subscript
        {
            get { return _subscript; }
            set { _subscript = value; }
        }
        private bool _comment = false;

        public bool Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
        private bool _italic = false;

        public bool Italic
        {
            get { return _italic; }
            set { _italic = value; }
        }
        private bool _underline = false;

        public bool Underline
        {
            get { return _underline; }
            set { _underline = value; }
        }
        private bool _strikeout = false;

        public bool Strikeout
        {
            get { return _strikeout; }
            set { _strikeout = value; }
        }
        private bool _superscript = false;

        public bool Superscript
        {
            get { return _superscript; }
            set { _superscript = value; }
        }
        private bool _bold = false;

        public bool Bold
        {
            get { return _bold; }
            set { _bold = value; }
        }


        public PassageFormat()
            : base(ContentType.FormatContent)
        {
        }


    }
}
