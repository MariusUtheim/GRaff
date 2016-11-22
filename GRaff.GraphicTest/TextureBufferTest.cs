using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.GraphicTest
{
	[Test]
	class TextureBufferTest : GameElement
	{
		Texture texture = TextureBuffers.Giraffe.Texture;
		Sprite sprite = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, 2));

		public TextureBufferTest()
		{
			TestController.Background.Color = Colors.LightGray;
		}

		public override void OnDraw()
		{
			Draw.Texture(TextureBuffers.Giraffe.Texture, 0, 0);
			Draw.Sprite(sprite, Time.LoopCount / 20, 0.25 * texture.Width, 1.5 * texture.Height);
		}
	}
}
