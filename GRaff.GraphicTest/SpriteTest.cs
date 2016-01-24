using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	[Test]
	class SpriteTest : GameElement
	{
		private Sprite _sprite;
		private Transform _transform = new Transform { Location = Room.Current.Center, Rotation = Angle.Deg(25), XShear = 0.3, YShear= 0.4 };
		public SpriteTest()
		{
			_sprite = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, 4));
		}

		public override void OnDraw()
		{
			Draw.Sprite(_sprite, Time.LoopCount / 4, _transform);
		}

	}
}
