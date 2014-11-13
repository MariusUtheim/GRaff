

using System;

namespace GRaff.Particles
{
	public class RandomColorBehavior : IParticleBehavior
	{
		public IParticleProperty Generate()
		{
			return null;// new RandomColorProperty();
		}

		public void Initialize(Particle particle)
		{
			particle.Color = GRandom.Color();
		}
	}

	class RandomColorProperty : IParticleProperty
	{
		public void Update(Particle particle)
		{
			particle.Color = GRandom.Color();
		}
	}
}
