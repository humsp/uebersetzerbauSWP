using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen
{
    abstract class ZComponent : IZComponent
    {
        protected List<IZComponent> _subComponents = new List<IZComponent>();
        protected ZAddress _componentAddress = null;

        public virtual List<IZComponent> SubComponents
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

        public virtual ZAddress Address
        {
            get
            {
                return _componentAddress;
            }
            set
            {
                _componentAddress = value;
            }
        }

        protected virtual void Setup()
        {
            int currentAddress = Address.Absolute;
            foreach (ZComponent component in _subComponents)
	        {
                if (component.Address == null)
                {
                    component.SetAddress(currentAddress);
                    currentAddress += component.Size;
                }
                else
                {
                    currentAddress = component.Address.Absolute;
                    currentAddress += component.Size;
                }
                component.Setup();
	        }
        }

        protected virtual void SetAddress(int absoluteAddr)
        {
            _componentAddress = new ZAddress(absoluteAddr);
        }
    }
}
