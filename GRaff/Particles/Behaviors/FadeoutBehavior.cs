 

namespace GRaff.Particles
{
	public class FadeoutBehavior : IParticleBehavior
	{
		public void Update(Particle particle)
		{
			particle.Blend = new Color((byte)(255.0 - particle.Lifetime * 255.0 / particle.TotalLifetime), particle.Blend.R, particle.Blend.G, particle.Blend.B);
		}
	}
}