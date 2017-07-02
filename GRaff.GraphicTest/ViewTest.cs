using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	[Test]
    class ViewTest : GameElement
	{

		public override void OnDraw()
		{
			Draw.Clear(Colors.LightGray);

			using (View.UseView(0, 0, 2, 2))
				Draw.FillRectangle(((-0.2, -0.2), (0.4, 0.4)), Colors.Blue);

			using (View.UseView(256, 256, 512, 512))
				Draw.Circle((256, 256), 256, Colors.Black);

			Draw.Line((0, 0), (Room.Current.Width, Room.Current.Height), Colors.Black);

			var tx = Textures.Giraffe.SubTexture;
			using (View.UseView(0, 0, tx.Width, tx.Height))
				Draw.Texture(tx, (0, 0));
		}
	}
}
