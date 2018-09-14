using System;
using System.Linq;

namespace GRaff.Panels
{
    public class PanelElement : GameElement, IGlobalMouseListener, IGlobalMousePressListener, IGlobalMouseReleaseListener, IGlobalMouseWheelListener
    {
        public PanelElement(Node root)
        {
            Contract.Requires<ArgumentNullException>(root != null && root.Parent == null && root.Container == null);
            this.Root = root;
            root.Container = this;
        }

        public Node Root { get; }

        public Node HoveredNode { get; private set; }


        public void OnGlobalMouse(MouseButton button)
        {
            Root._MouseEvent((IPanelMouseListener n, MouseEventArgs e) => n.OnMouse(e), new MouseEventArgs(button, Mouse.ViewLocation, Mouse.WheelDelta));
        }

        public void OnGlobalMousePress(MouseButton button)
        {
            Root._MouseEvent((IPanelMousePressListener n, MouseEventArgs e) => n.OnMousePress(e), new MouseEventArgs(button, Mouse.ViewLocation, Mouse.WheelDelta));
        }

        public void OnGlobalMouseRelease(MouseButton button)
        {
            Root._MouseEvent((IPanelMouseReleaseListener n, MouseEventArgs e) => n.OnMouseRelease(e), new MouseEventArgs(button, Mouse.ViewLocation, Mouse.WheelDelta));
        }

        public void OnGlobalMouseWheel(double delta)
        {
            Root._MouseEvent((IPanelMouseWheelListener n, MouseEventArgs e) => n.OnMouseWheel(e), new MouseEventArgs(MouseButton.None, Mouse.ViewLocation, delta));
        }

        public override void OnStep()
        {
            Root._Step();
            Node newHover = null;
            Root._MouseEvent((Node n, MouseEventArgs e) =>
            {
                n.OnMouseHover(e);
                if (e.IsHandled)
                {
                    newHover = n;
                    newHover.IsMouseHovering = true;
                }
            }, new MouseEventArgs(MouseButton.None, Mouse.ViewLocation, 0));

            if (newHover != HoveredNode)
            {
                (HoveredNode as IPanelEndHoverListener)?.OnEndHover();
                if (HoveredNode != null)
                    HoveredNode.IsMouseHovering = false;
                HoveredNode = newHover;
                (newHover as IPanelBeginHoverListener)?.OnBeginHover();
            }
        }

        public override void OnDraw()
        {
            Root._Draw();
        }
    }
}
