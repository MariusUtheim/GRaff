using System;
using System.Diagnostics.Contracts;
using GRaff.Randomness;


namespace GRaff.Particles
{
	public class LinearMotionBehavior : IParticleBehavior
	{
		private IDistribution<Vector> _velocity;

		public LinearMotionBehavior(double speed)
			: this(new VectorDistribution(speed))
		{ }

		public LinearMotionBehavior(double minSpeed, double maxSpeed)
			: this(new VectorDistribution(GRandom.Source, minSpeed, maxSpeed))
		{
			Contract.Requires<ArgumentOutOfRangeException>(minSpeed >= 0 && maxSpeed >= minSpeed);
		}

		public LinearMotionBehavior(Vector velocity)
			: this(new ConstantDistribution<Vector>(velocity))
		{ }

		public LinearMotionBehavior(IDistribution<Vector> distribution)
		{
			Contract.Requires<ArgumentNullException>(distribution != null);
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
				Contract.Requires<ArgumentNullException>(value != null);
				_velocity = value;
			}
		}

		public void AttachTo(Particle particle)
		{
			particle.Velocity += VelocityDistribution.Generate();
		}
	}
}
