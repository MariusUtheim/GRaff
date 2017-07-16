 

namespace GRaff.Particles
{
	public class FadeoutProperty : IParticleProperty
	{
		public void Update(Particle particle)
		{
			particle.Blend = new Color(particle.Blend.R, particle.Blend.G, particle.Blend.B, (byte)(255.0 - particle.Lifetime * 255.0 / particle.TotalLifetime));
		}
	}
}