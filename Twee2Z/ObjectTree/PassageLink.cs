using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class PassageLink : PassageContent
    {
        private Passage _targetPassage;
        private string _target;
        private string _expression;
        private string _displayText;

        public PassageLink(string link)
            : base(ContentType.LinkContent)
        {
            _target = link;
        }
        public PassageLink(string link, string displayTextorExp, bool isexp)
            : base(ContentType.LinkContent)
        {
            _target = link;
            if (isexp)
            {
                _expression = displayTextorExp;
            }
            else
            {
                _displayText = displayTextorExp;
            }
        }
        public PassageLink(string link, string displayText, string expression)
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

        public string Expression
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
