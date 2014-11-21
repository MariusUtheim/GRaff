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

		public void AttachTo(Particle particle)
		{
			t += Angle.Deg(3.0);
			particle.Velocity = new Vector(5, t);
		}
	}
}
