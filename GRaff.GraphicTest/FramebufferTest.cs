using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Synchronization;

namespace GRaff.GraphicTest
{
	[Test]
	class FramebufferTest : GameElement
	{
		Framebuffer _framebuffer;
		Texture _giraffe = Textures.Giraffe;
        int w = 500, h = 500;

		public FramebufferTest()
		{
				_framebuffer = new Framebuffer(w, h);

				using (_framebuffer.Use())
				{
                    Draw.Clear(Colors.Red);

					Draw.Texture(_giraffe, (0, 0));

					Draw.FillTriangle((0, 0), (0, 10), (10, 0), Colors.Black.Transparent(0), Colors.Black, Colors.Black);
					Draw.FillTriangle((w, 0), (w, 10), (w - 10, 0), Colors.Black.Transparent(0), Colors.Black, Colors.Black);
					Draw.FillTriangle((0, h), (0, h - 10), (10, h), Colors.Black.Transparent(0), Colors.Black, Colors.Black);
					Draw.FillTriangle((w, h), (w, h - 10), (w - 10, h), Colors.Black.Transparent(0), Colors.Black, Colors.Black);

					Draw.Line((0, h), (w, 0), Colors.Black);
				}
		}


		public override void OnDraw()
		{
            Draw.Clear(Colors.LightGray);
			Draw.Texture(_framebuffer?.Texture, (0, 0));
			Draw.Rectangle(((0, 0), (w, h)), Colors.Black);
		}
    }
}
