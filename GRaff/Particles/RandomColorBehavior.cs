using System;

namespace GRaff.Particles
{
	public class RandomColorBehavior : IParticleBehavior
	{
		public void AttachTo(Particle particle)
		{
			particle.Color = GRandom.Color();
		}
	}
}
