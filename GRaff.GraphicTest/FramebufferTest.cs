using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Synchronization;

namespace GRaff.GraphicTest
{
	[Test(Order = -10)]
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

                Draw.FillCircle((0, 0), 5, Colors.Black);
                Draw.FillCircle((w, 0), 5, Colors.Black);
                Draw.FillCircle((0, h), 5, Colors.Black);
                Draw.FillCircle((w, h), 5, Colors.Black);

				Draw.Line((0, 0), (w, h), Colors.Black);
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
