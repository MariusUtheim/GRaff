using System.Collections.Generic;

namespace GRaff.Particles
{
	internal interface IParticleRenderer
	{
		void Render(IEnumerable<Particle> particles);
    }
}