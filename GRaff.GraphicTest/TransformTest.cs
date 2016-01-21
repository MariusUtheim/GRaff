using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	class TransformTest : Test
	{
		Sprite sprite = new Sprite(TextureBuffers.Giraffe.Texture);

		AffineMatrix Identity => new AffineMatrix().Translate(200, 200);
		AffineMatrix HFlip => AffineMatrix.Scaling(GMath.Sin(Time.LoopCount / 60.0), 1).Translate(500, 200);
		AffineMatrix VFlip => AffineMatrix.Scaling(1, GMath.Cos(Time.LoopCount / 60.0)).Translate(800, 200);
		AffineMatrix Scale => AffineMatrix.Scaling(0.8 + 0.2 * GMath.Sin(Time.LoopCount / 65.0), 0.7 + 0.3 * GMath.Cos(Time.LoopCount / 70.0)).Translate(200, 500);
		AffineMatrix Rotation => AffineMatrix.Rotation(Angle.Deg(Time.LoopCount)).Translate(500, 500);
		AffineMatrix Shear => AffineMatrix.Shearing(0.3 *GMath.Sin(Time.LoopCount / 55.0), 0.4 * GMath.Cos(Time.LoopCount / 60.0)).Translate(800, 500);


		public override void OnDraw()
		{
			Draw.Sprite(sprite, 0, Identity);
			Draw.Sprite(sprite, 0, HFlip);
			Draw.Sprite(sprite, 0, VFlip);
			Draw.Sprite(sprite, 0, Scale);
			Draw.Sprite(sprite, 0, Rotation);
			Draw.Sprite(sprite, 0, Shear);
		}

	}
}
