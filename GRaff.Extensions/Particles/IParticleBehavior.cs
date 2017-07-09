

using System;
using System.Diagnostics.Contracts;

namespace GRaff.Particles
{
	[ContractClass(typeof(ParticleBehaviorContract))]
	public interface IParticleBehavior
	{ 
		/// <summary>
		/// Attaches this GRaff.Particles.IParticleBehavior to the specified GRaff.Particles.Particle. This could include attaching properties to that GRaff.Particles.Particle.
		/// </summary>
		/// <param name="particle">The GRaff.Particle to initialize</param>
		void AttachTo(Particle particle);

	}

	[ContractClassFor(typeof(IParticleBehavior))]
	abstract class ParticleBehaviorContract : IParticleBehavior
	{
		public void AttachTo(Particle particle)
		{
			Contract.Requires<ArgumentNullException>(particle != null);
		}
	}
}
