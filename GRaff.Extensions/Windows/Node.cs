using System;
namespace GRaff.Windows
{
    public abstract class ChildElement
    {
        public Rectangle Region { get; set;  }

        internal ContainerElement _Parent;
        public ContainerElement Parent
        {
            get => _Parent;
            set
            {
                if (value == _Parent)
                    return;
                _Parent._Children.Remove(this);
                value?._Children.Add(this);
                _Parent = value;
            }
        }

        
    }
}
