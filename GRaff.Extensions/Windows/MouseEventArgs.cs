using System;
namespace GRaff
{
    public class MouseEventArgs : PropagatingEventArgs
    {
        public MouseEventArgs(MouseButton button, Point location, double wheelDelta)
        {
            this.Button = button;
            this.Location = location;
            this.WheelDelta = wheelDelta;
        }

        public MouseButton Button { get; }

        public Point Location { get; }

        public double X => Location.X;

        public double Y => Location.Y;

        public double WheelDelta { get; }
    }
}
