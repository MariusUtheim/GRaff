using System;

namespace GRaff.Panels
{
    public abstract class DraggableNode : Node
    {
        static DraggableNode()
        {
            GlobalEvent.EndStep += _globalEndStep;
        }

        protected DraggableNode() { }

        private static void _globalEndStep()
        {
            if (DragNode == null)
                return;

            if (Mouse.IsDown(MouseButton.Left))
            {
                DragNode.OnDrag(DragNode.ToParent(Mouse.ViewLocation) + DragOffset);
            }
            else
            {
                DragNode.OnDrop();
                DragNode = null;
            }

        }


        public static DraggableNode DragNode { get; private set; }

        public static Point DragOrigin { get; private set; }

        public static Vector DragOffset { get; private set; }

        protected virtual void OnBeginDrag() { }

        protected abstract void OnDrag(Point location);

        protected virtual void OnDrop() { }

        public void Drag(Point origin)
        {
            DragNode = this;
            DragOffset = this.Location - origin;
            DragOrigin = origin;
            OnBeginDrag();
        }

        public void Drag() => Drag(ToParent(Mouse.ViewLocation));
    }
}
