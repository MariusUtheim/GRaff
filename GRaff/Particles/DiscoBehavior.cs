using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class DiscoBehavior : IParticleBehavior
	{
		public void AttachTo(Particle particle)
		{
			particle.AttachProperty(new DiscoProperty());
		}
	}

	public class DiscoProperty : IParticleProperty
	{
		public void Update(Particle particle)
		{
			particle.Color = GRandom.Color();
		}
	}
}
