using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Randomness;

namespace GRaff.Particles.Behaviors
{
	public class RotatingFeature : IParticleFeature
	{
		private IDistribution<Angle> _direction;

		public RotatingFeature()
			: this(new AngleDistribution())
		{ }


		public RotatingFeature(IDistribution<Angle> initialDirection)
		{
			if (initialDirection == null) throw new ArgumentNullException("initialDirection");	 /*C#6.0*/
			_direction = initialDirection;
		}

		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new RotatingBehavior(_direction.Generate()));
		}
	}
}
