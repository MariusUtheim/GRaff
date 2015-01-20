using System;
using GRaff.Randomness;


namespace GRaff.Particles
{
	public class LinearMotionFeature : IParticleFeature
	{
		private IDistribution<Vector> _velocity;

		public LinearMotionFeature(double speed)
			: this(new VectorDistribution(speed))
		{ }

		public LinearMotionFeature(double minSpeed, double maxSpeed)
			: this(new VectorDistribution(GRandom.Source, minSpeed, maxSpeed)) { }

		public LinearMotionFeature(Vector velocity)
			: this(new ConstantDistribution<Vector>(velocity))
		{ }

		public LinearMotionFeature(IDistribution<Vector> distribution)
		{
			this._velocity = distribution;
		}

		public IDistribution<Vector> VelocityDistribution
		{
			get
			{
				return _velocity;
			}

			set
			{
				if (value == null)
					throw new ArgumentNullException("value");
				_velocity = value;
			}
		}

		public void AttachTo(Particle particle)
		{
			particle.Velocity += VelocityDistribution.Generate();
		}
	}
}
