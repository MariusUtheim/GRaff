using System;
using GRaff.Randomness;


namespace GRaff.Particles.Behaviors
{
	public class RandomColorFeature : IParticleFeature
	{
		private IDistribution<Color> _distribution;

		public RandomColorFeature()
		{
			_distribution = new RgbDistribution();
		}

		public RandomColorFeature(IDistribution<Color> distribution)
		{
			if (distribution == null) throw new ArgumentNullException("distribution"); /*C#6.0*/ 
			this._distribution = distribution;
		}

		public IDistribution<Color> Distribution
		{
			get { return _distribution; }
			private set
			{
				if (value == null)
					throw new ArgumentNullException("value");   /*C#6.0*/
				this._distribution = value;
			}
		}

		public void AttachTo(Particle particle)
		{
			particle.Blend = _distribution.Generate();
		}
	}
}
