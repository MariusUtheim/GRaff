using System;

namespace GRaff.Particles
{
	public class FadeProperty : IParticleProperty
	{
		public void Update(Particle particle)
		{
			particle.Color = new Color((byte)(255.0 - particle.Lifetime * 255.0 / particle.TotalLifetime), particle.Color.R, particle.Color.G, particle.Color.B);
		}
	}
}