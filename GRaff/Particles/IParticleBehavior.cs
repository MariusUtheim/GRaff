

namespace GRaff.Particles
{
	public interface IParticleBehavior
	{ 
		/// <summary>
		/// Attaches this GRaff.Particles.IParticleBehavior to the specified GRaff.Particles.Particle. This could include attaching properties to that GRaff.Particles.Particle.
		/// </summary>
		/// <param name="particle">The GRaff.Particle to initialize</param>
		void AttachTo(Particle particle);

	}
}
