using System;
using GRaff.Randomness;


namespace GRaff.Particles.Behaviors
{
	public class RotationFeature : IParticleFeature
	{
		private IDistribution<Angle> _direction; 

		public RotationFeature()
			: this(new AngleDistribution())
		{ }


		public RotationFeature(IDistribution<Angle> initialDirection)
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
