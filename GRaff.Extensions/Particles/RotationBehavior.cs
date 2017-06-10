using System;
using GRaff.Randomness;
using System.Diagnostics.Contracts;

namespace GRaff.Particles
{
	public class RotationBehavior : IParticleBehavior
	{
		private readonly IDistribution<Angle> _direction; 

		public RotationBehavior()
			: this(new AngleDistribution())
		{ }


		public RotationBehavior(IDistribution<Angle> initialDirection)
		{
			Contract.Requires<ArgumentNullException>(initialDirection != null);
			_direction = initialDirection;
		}

		public void AttachTo(Particle particle)
		{
			particle.TransformationMatrix = particle.TransformationMatrix.Rotate(_direction.Generate());
		}
	}
}
