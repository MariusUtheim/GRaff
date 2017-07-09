using System;
using GRaff.Synchronization;
using GRaff.Panels;

namespace GRaff.GraphicTest
{
    [Test(Order = -1)]
    public class PanelsTest : GameElement
    {
        class Block : DraggableNode, IPanelMousePressListener
        {
            public Block(Rectangle region, Color color)
            {
                this.Region = region;
                this.Color = color;
            }

            public Color Color { get; private set;  }

            public override void OnDraw()
            {
                Draw.FillRectangle(Region, Color);
            }

            protected override void OnDrag(Point location)
            {
                Location = location.Confine(new Rectangle(Point.Zero, Parent.Region.Size - Region.Size));
            }

            public void OnMousePress(MouseEventArgs e)
            {
              // var originalColor = Color;
              //  Color = Colors.White;
              //  Tween.Animate(30, Tween.Linear, () => Color, originalColor);
                Drag(e.Location);
               
            }
        }

        PanelElement _rootElement;

        public PanelsTest()
        {
            _rootElement = Instance.Create(new PanelElement(new Rectangle(100, 100, 200, 120)));

            var block = _rootElement.Root.AddChildLast(new Block(new Rectangle(20, 20, 100, 100), Colors.Red));
            block.AddChildLast(new Block(new Rectangle(10, 10, 40, 30), Colors.Blue));

            _rootElement.Root.AddChildLast(new Block(new Rectangle(0, 0, 45, 40), Colors.Lime));

            Depth = 1000;
        }

        public override void OnStep()
        {
           // _root.Root.X = 100 + 50 * GMath.Cos(GMath.Tau * Time.LoopCount / 180);
        }

        public override void OnDraw()
        {
            Draw.Clear(Colors.Gray);
            Draw.Rectangle(_rootElement.Root.Region, Colors.Black);
        }

        protected override void OnDestroy()
        {
            _rootElement.Destroy();
        }
    }
}
