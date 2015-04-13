using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Randomness;
using System.Diagnostics.Contracts;

namespace GRaff.Particles
{
	public class RotatingBehavior : IParticleBehavior
	{
		private IDistribution<Angle> _rotation;

		public RotatingBehavior(Angle rotation)
			: this(new ConstantDistribution<Angle>(rotation))
		{ }

		public RotatingBehavior(Angle minRotation, Angle maxRotation)
			: this(new AngleDistribution(minRotation, maxRotation))
		{ }

		public RotatingBehavior(IDistribution<Angle> rotation)
		{
			Contract.Requires(rotation != null);
			_rotation = rotation;
		}

		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new RotatingProperty(_rotation.Generate()));
		}
	}
}
