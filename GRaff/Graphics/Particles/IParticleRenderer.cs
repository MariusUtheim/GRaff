using System.Collections.Generic;

namespace GRaff.Graphics.Particles
{
	internal interface IParticleRenderer
	{
		void Render(IEnumerable<Particle> particles);
    }
}