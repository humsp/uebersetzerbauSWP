using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen
{
    class ZRoutineTable
    {
        public Dictionary<string, short> _routines = new Dictionary<string, short>();
        private ushort _routineCount;
        private short _lastIndex = short.MinValue;

        public void AddRoutine(string routine)
        {
            if (!_routines.ContainsKey(routine))
            {
                short uniqueId = _lastIndex++;
                _routines.Add(routine, uniqueId);
                _routineCount++;
            }
        }

        public short GetRoutine(string routine)
        {
            short foundValue;

            if (!_routines.TryGetValue(routine, out foundValue))
            {
                AddRoutine(routine);
                _routines.TryGetValue(routine, out foundValue);
            }

            return foundValue;
        }

        public string GetKey(short value)
        {
            string foundKey = _routines.Keys.FirstOrDefault(key => _routines[key] == value);

            if (foundKey == null)
            {
                throw new KeyNotFoundException("There is no key (routine name) to the given value (routine id): " + value);
            }

            return foundKey;
        }
    }
}
