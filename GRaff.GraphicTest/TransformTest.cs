using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.GraphicTest
{
	[Test]
	class TransformTest : GameElement
	{
		Sprite sprite = new Sprite(TextureBuffers.Giraffe.Texture);
		Sprite fourSprites = new Sprite(new AnimationStrip(TextureBuffers.Giraffe, new IntVector(2, 2)), origin: Vector.Zero);


		Matrix Identity => new Matrix().Translate(200, 200);
		Matrix HFlip => Matrix.Scaling(GMath.Sin(Time.LoopCount / 60.0), 1).Translate(500, 200);
		Matrix VFlip => Matrix.Scaling(1, GMath.Cos(Time.LoopCount / 60.0)).Translate(800, 200);
		Matrix Scale => Matrix.Scaling(0.8 + 0.2 * GMath.Sin(Time.LoopCount / 65.0), 0.7 + 0.3 * GMath.Cos(Time.LoopCount / 70.0)).Translate(200, 500);
		Matrix Rotation => Matrix.Rotation(Angle.Deg(Time.LoopCount)).Translate(500, 500);
		Matrix Shear => Matrix.Shearing(0.3 * GMath.Sin(Time.LoopCount / 55.0), 0.4 * GMath.Cos(Time.LoopCount / 60.0)).Translate(800, 500);


		public override void OnDraw()
		{
			Draw.Sprite(fourSprites, Time.LoopCount / 30, 0, 0);
			Draw.Sprite(sprite, 0, Identity);
			Draw.Sprite(sprite, 0, HFlip);
			Draw.Sprite(sprite, 0, VFlip);
			Draw.Sprite(sprite, 0, Scale);
			Draw.Sprite(sprite, 0, Rotation);
			Draw.Sprite(sprite, 0, Shear);
		}

	}
}
