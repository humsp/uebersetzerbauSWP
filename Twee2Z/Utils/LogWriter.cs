using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security;

namespace Twee2Z.Utils
{
    public class LogWriter
    {

        private TextWriter _textWriter;

        public LogWriter()
            : this(System.Console.Out)
        {
        }

        public LogWriter(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        public void Log(string text)
        {
            _textWriter.WriteLine(text);
            _textWriter.Flush();
        }


        public TextWriter TextWriter
        {
            get
            {
                return _textWriter;
            }
            set
            {
                _textWriter = value;
            }
        }
    }
}
