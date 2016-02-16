using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.GraphicTest
{
	[Test(Order = 0)]
	class TextureBufferTest : GameElement
	{
		Texture texture = TextureBuffers.Giraffe.Texture;
		Sprite sprite = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, 2));

		public TextureBufferTest()
		{
		}

		public override void OnDraw()
		{
			//Draw.Texture(TextureBuffers.Giraffe.Texture, 0, 0);
			Draw.Sprite(sprite, Time.LoopCount / 2, texture.Width, texture.Height);
		}
	}
}
