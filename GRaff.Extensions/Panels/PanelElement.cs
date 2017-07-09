﻿using System;
using System.Linq;

namespace GRaff.Panels
{
    public class PanelElement : GameElement, IGlobalMouseListener, IGlobalMousePressListener, IGlobalMouseReleaseListener, IGlobalMouseWheelListener
    {

        public PanelElement(Node root)
        {
            Contract.Requires<ArgumentNullException>(root != null);
            this.Root = root;
        }

        public PanelElement(Rectangle region)
            : this(new Node(region)) { }


        public Node Root { get; }

        public TNode AddChildFirst<TNode>(TNode child) where TNode : Node => Root.AddChildFirst(child);

        public TNode AddChildLast<TNode>(TNode child) where TNode : Node => Root.AddChildLast(child);

        public void RemoveChild(Node child) => Root.RemoveChild(child);

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
        }

        public override void OnDraw()
        {
            Root._Draw();
        }
    }
}
