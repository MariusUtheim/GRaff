using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Randomness;

namespace GRaff.Particles
{
	public class RotatingBehavior : IParticleBehavior
	{
		private IDistribution<Angle> _direction;

		public RotatingBehavior()
			: this(new AngleDistribution())
		{ }


		public RotatingBehavior(IDistribution<Angle> initialDirection)
		{
			if (initialDirection == null) throw new ArgumentNullException("initialDirection");	 /*C#6.0*/
			_direction = initialDirection;
		}

		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new RotatingProperty(_direction.Generate()));
		}
	}
}
