using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class MotionBehavior : IParticleBehavior
	{
		public void Initialize(Particle particle)
		{
			return;
		}

		public IParticleProperty Generate()
		{
			return new LinearMotionProperty(GRandom.Vector());
		}
	}

}
