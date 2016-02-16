using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Particles;
using GRaff.Randomness;

namespace GRaff.GraphicTest
{
	[Test]
	class ParticleTest : GameElement, IGlobalMouseListener
	{
		static ParticleType starType = _createStarType();
		ParticleSystem system = Instance.Create(new ParticleSystem(starType));

		static ParticleType _createStarType()
		{
			var type = new ParticleType(new Sprite(TextureBuffers.Star.Texture), 75);
			//type.BlendMode = BlendMode.Additive;
			type.AddBehavior(new RotationBehavior());
			type.AddBehavior(new RotatingBehavior(Angle.Deg(3)));
			type.AddBehavior(new ScalingBehavior(0.95));
			type.AddBehavior(new ScaleBehavior(new DoubleDistribution(0.36, 0.60)));
			//type.AddBehavior(new ColorBehavior(new RgbDistribution()));
			type.AddBehavior(new LinearMotionBehavior(5, 6));
			//type.AddBehavior(new FadeoutBehavior());
			
			return type;
		}
		

		public override void OnStep()
		{
			Window.Title = $"Particle count: {system.Count.ToString()}\t-\tFPS: {Time.FPS}";
        }

		public void OnGlobalMouse(MouseButton button)
		{
			system.Create(Mouse.Location, 1);
		}




	}
}
