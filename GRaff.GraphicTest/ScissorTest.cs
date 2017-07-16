using GRaff.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
    [Test]
    class ScissorTest : GameElement, IGlobalMousePressListener, IGlobalMouseReleaseListener
    {
        private bool _drawMode = true;
        private Point _mouseClickLocation;

        public ScissorTest()
        {
            Scissor.IsEnabled = true;
            Scissor.Region = ((0, 0), Window.Size);
        }

        private Rectangle _dragRectangle => (_mouseClickLocation, Mouse.Location - _mouseClickLocation);

        protected override void OnDestroy()
        {
            Scissor.IsEnabled = false;
        }

        public override void OnDraw()
        {
            Draw.Clear(Color.FromGray(0.5 * (1 + GMath.Sin(Time.LoopCount / 60))));

            if (!_drawMode)
                Draw.Rectangle(_dragRectangle, Colors.Red);
        }

        public void OnGlobalMousePress(MouseButton button)
        {
            if (button == MouseButton.Left)
                Instance<PlingEffect>.Create(Mouse.Location);
            else if (button == MouseButton.Right)
            {
                _mouseClickLocation = Mouse.Location;
                Scissor.IsEnabled = false;
                _drawMode = false;
            }
        }


        public void OnGlobalMouseRelease(MouseButton button)
        {
            if (button == MouseButton.Right && !_drawMode)
            {
                Scissor.IsEnabled = true;
                Scissor.Region = (IntRectangle)_dragRectangle;

                _drawMode = true;
            }
        }
    }


}
