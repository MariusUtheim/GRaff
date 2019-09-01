using System;
using System.Diagnostics.Contracts;
using GRaff.Randomness;


namespace GRaff.Graphics.Particles
{
	public class LinearMotionDescriptor : IParticleTypeDescriptor
	{


		public LinearMotionDescriptor(IDistribution<Vector> distribution)
		{
			Contract.Requires<ArgumentNullException>(distribution != null);
			this.VelocityDistribution = distribution;
		}

        public static LinearMotionDescriptor Uniform(double speed)
            => new LinearMotionDescriptor(new PolarVectorDistribution(speed));

        public static LinearMotionDescriptor Uniform(double minSpeed, double maxSpeed)
            => new LinearMotionDescriptor(new PolarVectorDistribution(minSpeed, maxSpeed));

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
