using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen
{
    abstract class ZComponent : IZComponent
    {
        protected List<IZComponent> _subComponents = new List<IZComponent>();
        protected ZAddress _position = null;
        
        public virtual List<IZComponent> SubComponents
        {
            get
            {
                return _subComponents;
            }
        }

        public virtual ZAddress Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public virtual int Size
        {
            get
            {
                return SubComponents.Sum(component => component.Size);
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

        public virtual void Setup(int currentAddress)
        {
            foreach (IZComponent component in _subComponents)
            {
                if (component.Position == null)
                {
                    component.Position = new ZAddress(currentAddress);
                    currentAddress += component.Size;
                }
                else
                {
                    currentAddress = component.Position.Absolute;
                    currentAddress += component.Size;
                }

                component.Setup(component.Position.Absolute);
            }
        }
    }
}
