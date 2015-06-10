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
        protected ZLabel _componentLabel = null;

        public virtual List<IZComponent> SubComponents
        {
            get
            {
                return _subComponents;
            }
        }

        public virtual ZLabel Label
        {
            get
            {
                return _componentLabel;
            }
            set
            {
                _componentLabel = value;
            }
        }

        public virtual int Size
        {
            get
            {
                return SubComponents.Sum(component => component.Size);
            }
        }

        protected virtual void SetLabel(int absoluteAddr, string name)
        {
            if (_componentLabel == null)
                _componentLabel = new ZLabel(new ZAddress(absoluteAddr), name);
            else if (_componentLabel.TargetAddress == null)
            {
                _componentLabel.TargetAddress = new ZAddress(absoluteAddr);
                _componentLabel.Name = name;
            }
            else
            {
                _componentLabel.TargetAddress.Absolute = absoluteAddr;
                _componentLabel.Name = name;
            }
        }

        protected virtual void Setup()
        {
            int currentAddress = Label.TargetAddress.Absolute;
            foreach (ZComponent component in _subComponents)
            {
                if (component.Label == null)
                {
                    component.SetLabel(currentAddress, null);
                    currentAddress += component.Size;
                }
                else if (component.Label.Name != null)
                {
                    component.SetLabel(currentAddress, component.Label.Name);
                    currentAddress += component.Size;
                }
                else
                {
                    currentAddress = component.Label.TargetAddress.Absolute;
                    currentAddress += component.Size;
                }
                component.Setup();
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
    }
}
