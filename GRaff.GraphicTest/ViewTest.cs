using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
    [Test]
    class ViewTest : GameElement, IKeyListener
	{

        public override void OnStep()
        {
            Window.Title = View.Current.Location.ToString();// Mouse.Location.ToString();
        }

        protected override void OnDestroy()
        {
            View.FullWindow().Bind();
        }

		public override void OnDraw()
		{
            
            using (View.Rectangle(new Rectangle(0, 0, 1, 1)).Use())
                Draw.FillRectangle(new Rectangle(0, 0, 1, 1), Colors.Gray);

            using (View.Rectangle(new Rectangle(-1, -1, 2, 2)).Use()) 
				Draw.FillRectangle(((-0.2, -0.2), (0.4, 0.4)), Colors.Blue);

            using (View.Centered((256, 256), (512, 512)).Use())
				Draw.Circle((256, 256), 256, Colors.Black);

			Draw.Line((0, 0), (Window.Width, Window.Height), Colors.Black);
            Draw.FillCircle((0, 0), 10, Colors.Black);
            Draw.FillCircle(Window.Size, 10, Colors.White);

            using (View.DrawTo(new Point(150, 50)).Use())
                Draw.FillCircle(Point.Zero, 5, Colors.Red);
            Draw.Point((150, 50), Colors.Black);

			var tx = Textures.Giraffe;
            using (View.Rectangle(0, 0, tx.Width, tx.Height).Use())
                Draw.Texture(tx, (0, 0), Colors.White.Transparent(0.2));

            Draw.FillCircle(Mouse.Location, 5, Colors.Lime);
		}

        public void OnKey(Key key)
        {
            switch (key)
            {
                case Key.A: View.Current.X -= 0.01; break;
                case Key.D: View.Current.X += 0.01; break;
                case Key.W: View.Current.Y -= 0.01; break;
                case Key.S: View.Current.Y += 0.01; break;
                case Key.R: View.Current.Location = Point.Zero; break;
            }
        }
    }
}
