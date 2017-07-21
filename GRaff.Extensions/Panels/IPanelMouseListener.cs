using System;
namespace GRaff.Panels
{
    public interface IPanelMouseListener
    {
        void OnMouse(MouseEventArgs e);
    }

    public interface IPanelMousePressListener
    {
        void OnMousePress(MouseEventArgs e);
    }

    public interface IPanelMouseReleaseListener
    {
        void OnMouseRelease(MouseEventArgs e);
    }

    public interface IPanelMouseWheelListener
    {
        void OnMouseWheel(MouseEventArgs e);
    }

    public interface IPanelBeginHoverListener
    {
        void OnBeginHover();
    }

    public interface IPanelEndHoverListener
    {
        void OnEndHover();
    }

    public interface IPanelHoverListener : IPanelBeginHoverListener, IPanelEndHoverListener { }
}
