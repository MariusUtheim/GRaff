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
		Texture _giraffe = TextureBuffers.Giraffe.Texture;
        int w = 500, h = 500;

		public FramebufferTest()
		{
				_framebuffer = new Framebuffer(w, h);

				using (_framebuffer.Use())
				{
                    Draw.Clear(Colors.Red);

					Draw.Texture(_giraffe, (0, 0));

					Draw.FillTriangle(Colors.Black.Transparent(0), Colors.Black, Colors.Black, (0, 0), (0, 10), (10, 0));
					Draw.FillTriangle(Colors.Black.Transparent(0), Colors.Black, Colors.Black, (w, 0), (w, 10), (w - 10, 0));
					Draw.FillTriangle(Colors.Black.Transparent(0), Colors.Black, Colors.Black, (0, h), (0, h - 10), (10, h));
					Draw.FillTriangle(Colors.Black.Transparent(0), Colors.Black, Colors.Black, (w, h), (w, h - 10), (w - 10, h));

					Draw.Line(Colors.Black, (0, h), (w, 0));
				}
		}


		public override void OnDraw()
		{
            Draw.Clear(Colors.LightGray);
			Draw.Texture(_framebuffer?.Buffer.Texture, (0, 0));
			Draw.Rectangle(Colors.Black, ((0, 0), (w, h)));
		}
    }
}
