using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.Utils;
using Twee2Z.ObjectTree.PassageContents.Macro;

namespace Twee2Z.ObjectTree.PassageContents
{
    public class PassageContent
    {
        private PassageContentFormat _contentFormat;

        public PassageContentFormat ContentFormat
        {
            get { return _contentFormat; }
            set { _contentFormat = value; }
        }

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
            _type = type;
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

        public PassageVariable PassageVariable
        {
            get
            {
                if (_type == ContentType.VariableContent)
                {
                    return (PassageVariable)this;
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
        }
    }
}
