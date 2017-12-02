﻿using System;
using System.Linq;

namespace GRaff.Panels
{
    public class PanelElement : GameElement, IGlobalMouseListener, IGlobalMousePressListener, IGlobalMouseReleaseListener, IGlobalMouseWheelListener
    {
        private Node _hoverNode;

        public PanelElement(Node root)
        {
            Contract.Requires<ArgumentNullException>(root != null);
            this.Root = root;
        }

        public PanelElement(Rectangle region)
            : this(new Node { Region = region }) { }


        public Node Root { get; }

        public void OnGlobalMouse(MouseButton button)
        {
            Root._MouseEvent((IPanelMouseListener n, MouseEventArgs e) => n.OnMouse(e), new MouseEventArgs(button, Mouse.Location, Mouse.WheelDelta));
        }

        public void OnGlobalMousePress(MouseButton button)
        {
            Root._MouseEvent((IPanelMousePressListener n, MouseEventArgs e) => n.OnMousePress(e), new MouseEventArgs(button, Mouse.Location, Mouse.WheelDelta));
        }

        public void OnGlobalMouseRelease(MouseButton button)
        {
            Root._MouseEvent((IPanelMouseReleaseListener n, MouseEventArgs e) => n.OnMouseRelease(e), new MouseEventArgs(button, Mouse.Location, Mouse.WheelDelta));
        }

        public void OnGlobalMouseWheel(double delta)
        {
            Root._MouseEvent((IPanelMouseWheelListener n, MouseEventArgs e) => n.OnMouseWheel(e), new MouseEventArgs(MouseButton.None, Mouse.Location, delta));
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
            }, new MouseEventArgs(MouseButton.None, Mouse.Location, 0));

            if (newHover != _hoverNode)
            {
                (_hoverNode as IPanelEndHoverListener)?.OnEndHover();
                if (_hoverNode != null)
                    _hoverNode.IsMouseHovering = false;
                _hoverNode = newHover;
                (newHover as IPanelBeginHoverListener)?.OnBeginHover();
            }
        }

        public override void OnDraw()
        {
            Root._Draw();
        }
    }
}
