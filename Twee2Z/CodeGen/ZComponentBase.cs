using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen
{
    abstract class ZComponentBase : IZComponent
    {
        protected List<IZComponent> _subComponents = new List<IZComponent>();

        public virtual IEnumerable<IZComponent> SubComponents
        {
            get
            {
                return _subComponents;
            }
        }

        public virtual byte[] ToBytes()
        {
            List<Byte> byteList = new List<byte>();

            foreach (IZComponent component in SubComponents)
            {
                byteList.AddRange(component.ToBytes());
            }

            return byteList.ToArray();
        }

        public virtual int Size
        {
            get
            { 
                return SubComponents.Sum(component => component.Size);
            }
        }
    }
}
