using GRaff.Graphics;
using GRaff.Particles;
using GRaff.Randomness;


namespace GRaff.GraphicTest
{
	[Test]
	class ParticleTest : GameElement, IGlobalMouseListener
	{
		ParticleSystem starSystem;
		ParticleSystem pentagonSystem;
		PointAttractionBehavior attractor = new PointAttractionBehavior(Mouse.Location, 1);

		ParticleType _createStarType()
		{
			var type = new ParticleType(new Sprite(TextureBuffers.Star.Texture), 75);
			type.AddBehaviors(new IParticleBehavior[] {
				new RotationBehavior(),
				new RotatingBehavior(Angle.Deg(3)),
				new ScalingBehavior(0.95),
				new ScaleBehavior(new DoubleDistribution(0.36, 0.60)),
				new LinearMotionBehavior(5, 6),
			});
			return type;
		}

		ParticleType _createPentagonType()
		{
			var type = new ParticleType(Polygon.Regular(5, 25), 75);
			type.BlendMode = BlendMode.Additive;
            type.AddBehavior(new RotationBehavior());
			type.AddBehavior(new ColorBehavior());
			type.AddBehavior(new ScaleBehavior(new DoubleDistribution(1, 2)));
			type.AddBehavior(new ScalingBehavior(0.95));
			type.AddBehavior(new LinearMotionBehavior(5, 12));
			type.AddBehavior(new FadeoutBehavior());
			type.AddBehavior(attractor);
			return type;
		}

		public ParticleTest()
		{
			TestController.Background.Color = Colors.Black;
			starSystem = Instance.Create(new ParticleSystem(_createStarType()));
			pentagonSystem = Instance.Create(new ParticleSystem(_createPentagonType()));

        }

		public override void OnDestroy()
		{
			starSystem.Destroy();
			pentagonSystem.Destroy();
		}

		public override void OnStep()
		{
			Window.Title = $"Particle count: {starSystem.Count.ToString()}\t-\tFPS: {Time.FPS}";
			attractor.Location = Mouse.Location;
        }

		public void OnGlobalMouse(MouseButton button)
		{
			if (button == MouseButton.Left)
				starSystem.Create(Mouse.Location, 1);
			else
				pentagonSystem.Create(Mouse.Location, 1);
		}




	}
}
