using System;
using System.Diagnostics.Contracts;
using GRaff.Randomness;


namespace GRaff.Graphics.Particles
{
	public class LinearMotionDescriptor : IParticleTypeDescriptor
	{
		private IDistribution<Vector> _velocity;

		public LinearMotionDescriptor(double speed)
			: this(new VectorDistribution(speed))
		{ }

		public LinearMotionDescriptor(double minSpeed, double maxSpeed)
			: this(new VectorDistribution(GRandom.Source, minSpeed, maxSpeed))
		{
			Contract.Requires<ArgumentOutOfRangeException>(minSpeed >= 0 && maxSpeed >= minSpeed);
		}

		public LinearMotionDescriptor(Vector velocity)
			: this(new ConstantDistribution<Vector>(velocity))
		{ }

		public LinearMotionDescriptor(IDistribution<Vector> distribution)
		{
			Contract.Requires<ArgumentNullException>(distribution != null);
			this._velocity = distribution;
		}

        public static LinearMotionDescriptor Uniform(double speed)
            => new LinearMotionDescriptor(new VectorDistribution(speed));

        public static LinearMotionDescriptor Uniform(double minSpeed, double maxSpeed)
            => new LinearMotionDescriptor(new VectorDistribution(minSpeed, maxSpeed));

		public IDistribution<Vector> VelocityDistribution { get; set; }


        class LinearMotionBehavior : IParticleBehavior

        {
            public Vector Velocity;
            public void Initialize(Particle particle) => particle.Velocity += Velocity;
            public void Update(Particle particle) { }
        }

        public IParticleBehavior  MakeBehavior() => new LinearMotionBehavior { Velocity = VelocityDistribution.Generate() };
	}
}
