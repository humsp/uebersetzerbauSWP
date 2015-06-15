using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class PassageContent
    {
        public static bool Monospace = false;
        public static bool Subscript = false;
        public static bool Comment = false;
        public static bool Italic = false;
        public static bool Underline = false;
        public static bool Strikeout = false;
        public static bool Superscript = false;
        public static bool Bold = false;

        public bool _monospace = false;
        public bool _subscript = false;
        public bool _comment = false;
        public bool _italic = false;
        public bool _underline = false;
        public bool _strikeout = false;
        public bool _superscript = false;
        public bool _bold = false;

        public enum ContentType
        {
            TextContent,
            LinkContent,
            VariableContent,
            FunctionContent,
            MacroContent
        };

        private ContentType _type;

        public PassageContent(ContentType type)
        {
            Type = type;
        }


        public PassageLink PassageLink
        {
            get
            {
                if (_type == ContentType.LinkContent)
                {
                    return (PassageLink)this;
                }
                return null;
            }
        }

        public PassageText PassageText
        {
            get
            {
                if (_type == ContentType.TextContent)
                {
                    return (PassageText)this;
                }
                return null;
            }
        }

        public PassageLink PassageVariable
        {
            get
            {
                if (_type == ContentType.VariableContent)
                {
                    return (PassageLink)this;
                }
                return null;
            }
        }

        public PassageFunction PassageFunction
        {
            get
            {
                if (_type == ContentType.FunctionContent)
                {
                    return (PassageFunction)this;
                }
                return null;
            }
        }

        public PassageMacro PassageMacro
        {
            get
            {
                if (_type == ContentType.MacroContent)
                {
                    return (PassageMacro)this;
                }
                return null;
            }
        }

        public ContentType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
    }
}


/*
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
*/