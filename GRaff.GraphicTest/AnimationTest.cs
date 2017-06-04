using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.GraphicTest
{
	[Test]
	class AnimationTest : GameElement
	{
		Sprite sprite = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, 20), origin: new Vector(0, 0));
		Sprite pausedSprite = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, 20, new ValueTuple<int, double>[] {
			(0, 4),
			(1, 3),
			(2, 2),
			(3, 1),
			(4, 1),
			(5, 1),
			(6, 1),
			(7, 2),
			(8, 2),
			(9, 3),
			(10, 4),
			(11, 5),
			(12, 4),
			(13, 3),
			(14, 2),
			(15, 1),
			(16, 1),
			(17, 2),
			(18, 3),
			(19, 5),
		}), origin: new Vector(0, 0));



		public override void OnDraw()
		{
			Draw.Sprite(sprite, Time.LoopCount / 2, 0, 0);
			Draw.Sprite(sprite, Time.LoopCount / 4, sprite.Width, 0);
			Draw.Sprite(sprite, Time.LoopCount / 8, 2 * sprite.Width, 0);
			Draw.Sprite(pausedSprite, Time.LoopCount / 2, 0, sprite.Height);
		}
	}
}
