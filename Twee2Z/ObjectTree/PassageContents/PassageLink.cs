using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions;

namespace Twee2Z.ObjectTree.PassageContents
{
    public class PassageLink : PassageContent
    {
        private Passage _targetPassage;
        private string _target;
        private Expression _expression;
        private string _displayText;

        public PassageLink(string link)
            : base(ContentType.LinkContent)
        {
            _target = link;
        }
        public PassageLink(string link, string Text)
            : base(ContentType.LinkContent)
        {
            _target = link;
            _displayText = Text;
        }
        public PassageLink(string link, Expression exp)
            : base(ContentType.LinkContent)
        {
            _target = link;
            _expression = exp;
        }
        public PassageLink(string link, string displayText, Expression expression)
            : base(ContentType.LinkContent)
        {
            _target = link;
            _displayText = displayText;
            _expression = expression;
        }

        public Passage TargetPassage
        {
            get
            {
                return _targetPassage;
            }
            set
            {
                _targetPassage = value;
            }
        }

        public string Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        public Expression Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                _expression = value;
            }
        }

        public string DisplayText
        {
            get
            {
                return _displayText;
            }
            set
            {
                _displayText = value;
            }
        }
    }

}

/*
 * 	public class PassageExpression
	{

		private string _var ;
		private string _variable ;
		private string _stringVar ;
		private string _type ;
		private int _intVar ;
		private float _floatVar ;


		public PassageExpression (string type, string variable, string wert){

			_var = variable;
			_type = type;
			if (_type = "int" ) variable = 
			if (_type = "float")
			if (_type = "string")
		}

		public int LinkInt
		{
			get
			{
				if (_type == ExpType.IntContent)
				{
					return (int)_type;
				}

			}
		}


	}
 * 
 */
