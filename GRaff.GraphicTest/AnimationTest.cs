using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	[Test]
	class AnimationTest : GameElement
	{
		Sprite sprite = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, 20), origin: new Vector(0, 0));
		Sprite pausedSprite = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, 20, new[] {
			new Tuple<int, double>(0, 4),
			new Tuple<int, double>(1, 3),
			new Tuple<int, double>(2, 2),
			new Tuple<int, double>(3, 1),
			new Tuple<int, double>(4, 1),
			new Tuple<int, double>(5, 1),
			new Tuple<int, double>(6, 1),
			new Tuple<int, double>(7, 2),
			new Tuple<int, double>(8, 2),
			new Tuple<int, double>(9, 3),
			new Tuple<int, double>(10, 4),
			new Tuple<int, double>(11, 5),
			new Tuple<int, double>(12, 4),
			new Tuple<int, double>(13, 3),
			new Tuple<int, double>(14, 2),
			new Tuple<int, double>(15, 1),
			new Tuple<int, double>(16, 1),
			new Tuple<int, double>(17, 2),
			new Tuple<int, double>(18, 3),
			new Tuple<int, double>(19, 5),
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
