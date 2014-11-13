using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class SpiralBehavior : IParticleBehavior
	{
		private Angle t = Angle.Zero;

		public IParticleProperty Generate()
		{
			t += Angle.Deg(GMath.Tau / 3.0);
			return new LinearMotionProperty(new Vector(5, t));
		}

		public void Initialize(Particle particle)
		{
			return;
		}
	}
}
