﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;

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
            VariableContent
        };

        private ContentType _type;

        public PassageContent(ContentType type)
        {

            _monospace = Monospace;
            _subscript = Subscript;
            _comment = Comment;
            _italic = Italic;
            _underline = Underline;
            _strikeout = Strikeout;
            _superscript = Superscript;
            _bold = Bold;
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
