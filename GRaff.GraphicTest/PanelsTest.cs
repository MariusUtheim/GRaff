using System;
using GRaff.Synchronization;
using GRaff.Panels;

namespace GRaff.GraphicTest
{
    [Test]
    public class PanelsTest : GameElement
    {
        class Block : DraggableNode, IPanelMousePressListener
        {
            public Block(Rectangle region, Color color, Color hoverColor)
            {
                this.Region = region;
                this.Color = color;
                this.HoverColor = hoverColor;
            }

            public Color Color { get; private set;  }
            public Color HoverColor { get; private set; }

            public override void OnDraw()
            {
                Draw.FillRectangle(Region, IsMouseHovering ? HoverColor : Color);
            }

            protected override void OnDrag(Point location)
            {
                Location = location.Confine(new Rectangle(Point.Zero, Parent.Region.Size - Region.Size));
            }


            public void OnMousePress(MouseEventArgs e)
            {
                Drag(e.Location); 
            }
        }

        PanelElement _rootElement;

        public PanelsTest()
        {
            _rootElement = Instance.Create(new PanelElement(new Node { Region = new Rectangle(100, 100, 200, 120) }));

            var block = _rootElement.Root.AddChildLast(new Block(new Rectangle(20, 20, 100, 100), Colors.Red, Colors.IndianRed));
            block.AddChildLast(new Block(new Rectangle(10, 10, 40, 30), Colors.Blue, Colors.Aqua));

            _rootElement.Root.AddChildLast(new Block(new Rectangle(0, 0, 45, 40), Colors.Green, Colors.Lime));

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
