using GRaff.Graphics;
using GRaff.Graphics.Particles;
using GRaff.Randomness;


namespace GRaff.GraphicTest
{
    [Test]
	class ParticleTest : GameElement, IGlobalMouseListener
	{
		ParticleSystem starSystem;
        ParticleSystem pentagonSystem;
		static PointAttractionDescriptor attractor = new PointAttractionDescriptor(Mouse.ViewLocation, 1);
        static ParticleType starType = _createStarType();
        static ParticleType pentagonType = _createPentagonType();
        Background background;

		static ParticleType _createStarType()
		{
            var type = new ParticleType(new Sprite(Textures.Star), 75);

            type.AddDescriptors(new IParticleTypeDescriptor[]
            {
                new InitialStateDescriptor().UniformRotation().ConstantScale(0.95),
                new TransformationDescriptor().ConstantRotation(Angle.Deg(3)).UniformScaling(0.36, 0.60),
                LinearMotionDescriptor.Uniform(5, 6)
            });

			return type;
		}

		static ParticleType _createPentagonType()
		{
			var type = new ParticleType(Polygon.Regular(5, 25), 75);
			type.BlendMode = BlendMode.Additive;
            type.AddDescriptors(new IParticleTypeDescriptor[]
            {
                ColorDescriptor.Uniform(),
                new InitialStateDescriptor().UniformRotation().UniformScale(1, 2),
                new TransformationDescriptor().UniformRotation(Angle.Deg(-3), Angle.Deg(3)).ConstantScaling(0.95),
                LinearMotionDescriptor.Uniform(5, 12),
                FadeoutDescriptor.Default,
                attractor
            });

			return type;
		}

		public ParticleTest()
		{
			background = Instance.Create(new Background { Color = Colors.Black });
            starSystem = Instance.Create(new ParticleSystem(starType));
            pentagonSystem = Instance.Create(new ParticleSystem(pentagonType));
        }

		protected override void OnDestroy()
		{
            background.Destroy();
			starSystem.Destroy();
			pentagonSystem.Destroy();
		}

		public override void OnStep()
		{
			//Window.Title = $"Particle count: {starSystem.Count.ToString()}\t-\tFPS: {Time.Fps}";
			attractor.Location = Mouse.ViewLocation;
        }

		public void OnGlobalMouse(MouseButton button)
		{
		//	if (button == MouseButton.Left)
		//		starSystem.Create(Mouse.Location, 1);
		//	else
				pentagonSystem.Create(Mouse.ViewLocation, 1);
		}




	}
}
