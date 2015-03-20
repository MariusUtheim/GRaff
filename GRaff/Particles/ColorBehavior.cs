using GRaff.Randomness;

namespace GRaff.Particles
{
	public class ColorBehavior : IParticleBehavior
	{
		private IDistribution<Color> _color;

		public ColorBehavior()
			: this(new RgbDistribution())
		{ }

		public ColorBehavior(Color color)
			: this(new ConstantDistribution<Color>(color))
		{ }

		public ColorBehavior(IDistribution<Color> color)
		{
			this._color = color;
		}

		public void AttachTo(Particle particle)
		{
			particle.Blend = _color.Generate();
		}
	}
}
