using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen
{
    class ZSymbolTable
    {
        private Dictionary<string, ZGlobalVariable> _variables = new Dictionary<string, ZGlobalVariable>();
        private byte _variableCount;

        public void AddSymbol(string symbol)
        {
            if (!_variables.ContainsKey(symbol))
            {
                _variables.Add(symbol, new ZGlobalVariable(_variableCount));
                _variableCount++;
            }
        }

        public ZGlobalVariable GetSymbol(string symbol)
        {
            ZGlobalVariable foundValue = null;
            _variables.TryGetValue(symbol, out foundValue);

            if (foundValue == null)
            {
                AddSymbol(symbol);
                _variables.TryGetValue(symbol, out foundValue);
            }

            return foundValue;
        }
    }
}
