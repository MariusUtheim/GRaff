using System;
using GRaff.Randomness;


namespace GRaff.Particles
{
	public class RotationBehavior : IParticleBehavior
	{
		private IDistribution<Angle> _direction; 

		public RotationBehavior()
			: this(new AngleDistribution())
		{ }


		public RotationBehavior(IDistribution<Angle> initialDirection)
		{
			if (initialDirection == null) throw new ArgumentNullException("initialDirection");   /*C#6.0*/
			_direction = initialDirection;
		}

		public void AttachTo(Particle particle)
		{
			particle.TransformationMatrix = particle.TransformationMatrix.Rotate(_direction.Generate());
		}
	}
}
