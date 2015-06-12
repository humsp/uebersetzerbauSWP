using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen
{
    abstract class ZLabeledComponent : ZComponent, IZLabeledComponent
    {
        protected ZLabel _label = new ZLabel((string)null);

        public override ZAddress Position
        {
            get
            {
                return _label.TargetAddress;
            }
            set
            {
                _label.TargetAddress = value;
            }
        }

        /// <summary>
        /// Gets the absolute address with an optional label name. The address is set right before the code generation.
        /// </summary>
        public virtual ZLabel Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        public virtual void SetLabel(int absoluteAddr, string name)
        {
            if (_label == null)
                _label = new ZLabel(name, new ZAddress(absoluteAddr));
            else if (_label.TargetAddress == null)
            {
                _label.TargetAddress = new ZAddress(absoluteAddr);
                _label.Name = name;
            }
            else
            {
                _label.TargetAddress.Absolute = absoluteAddr;
                _label.Name = name;
            }
        }

        public override void Setup(int currentAddress)
        {
            foreach (IZComponent component in _subComponents)
            {
                IZLabeledComponent labeledComponent = component as IZLabeledComponent;

                if (labeledComponent == null)
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
                else
                {
                    if (labeledComponent.Label == null)
                    {
                        labeledComponent.SetLabel(currentAddress, null);
                        currentAddress += labeledComponent.Size;
                    }
                    else if (labeledComponent.Label.Name != null)
                    {
                        labeledComponent.SetLabel(currentAddress, labeledComponent.Label.Name);
                        currentAddress += labeledComponent.Size;
                    }
                    else if (labeledComponent.Label.Position != null)
                    {
                        currentAddress = labeledComponent.Label.TargetAddress.Absolute;
                        currentAddress += labeledComponent.Size;
                    }
                    else
                    {
                        labeledComponent.SetLabel(currentAddress, null);
                        currentAddress += labeledComponent.Size;
                    }

                    labeledComponent.Setup(labeledComponent.Label.TargetAddress.Absolute);
                }
            }
        }
    }
}
