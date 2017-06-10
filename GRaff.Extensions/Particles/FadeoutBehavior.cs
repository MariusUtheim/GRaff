
 
namespace GRaff.Particles
{
	public class FadeoutBehavior : IParticleBehavior
	{
		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new FadeoutProperty());
		}
	}
}
