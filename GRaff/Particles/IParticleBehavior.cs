

namespace GRaff.Particles
{
	public interface IParticleBehavior
	{
		/// <summary>
		/// Initializes a new GRaff.Particle with the custom behavior of this GRaff.IParticleBehavior
		/// </summary>
		/// <param name="particle">The GRaff.Particle to initialize</param>
		void Initialize(Particle particle);

		/// <summary>
		/// Generates a new GRaff.IParticleProperty based on this GRaff.IParticleBehavior.
		/// If null is returned, the GRaff.Particle will not add any 
		/// </summary>
		/// <returns>A new GRaff.IParticleProperty based on this GRaff.IParticleBehavior</returns>
		IParticleProperty Generate();
	}
}
