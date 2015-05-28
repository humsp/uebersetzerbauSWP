using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class PassageContent
    {
        public enum ContentType
        {
            TextContent,
            LinkContent,
            VariableContent
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
