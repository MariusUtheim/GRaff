
 
namespace GRaff.Particles
{
	public class FadeoutFeature : IParticleFeature
	{
		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new FadeoutBehavior());
		}
	}
}
