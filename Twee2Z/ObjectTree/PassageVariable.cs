using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class PassageVariable : PassageContent
    {
        public enum VariableType
        {
            Bool,
            String,
            Int
        };

        private string _id;
        private string _value;
        private bool _boolValue;
        private int _intValue;
        private VariableType _variableType;

        public PassageVariable(string id, int value)
            : base(ContentType.VariableContent)
        {
            _id = id;
            _value = value.ToString();
            _intValue = value;
            _variableType = VariableType.Int;
        }

        public PassageVariable(string id, bool value)
            : base(ContentType.VariableContent)
        {
            _id = id;
            _value = value.ToString();
            _boolValue = value;
            _variableType = VariableType.Bool;
        }

        public PassageVariable(string id, string value)
            : base(ContentType.VariableContent)
        {
            _id = id;
            _value = value;
            _variableType = VariableType.String;
        }


        public string Id
        {
            get
            {
                return _id;
            }
        }

        public bool BoolValue
        {
            get
            {
                return _boolValue;
            }
        }

        public int IntValue
        {
            get
            {
                return _intValue;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public VariableType TypeOfVariable
        {
            get
            {
                return _variableType;
            }
        }

    }
}
