using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class FadeBehavior : IParticleBehavior
	{
		public void AttachTo(Particle particle)
		{
			particle.AttachProperty(new FadeProperty());
		}
	}
}
