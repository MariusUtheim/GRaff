

namespace GRaff.Graphics.Particles
{
	public class AccelerationDescriptor : IParticleTypeDescriptor
	{
		public AccelerationDescriptor(Vector acceleration)
			:this(acceleration, 0) { }

		public AccelerationDescriptor(double friction)
			: this(Vector.Zero, friction) { }

		public AccelerationDescriptor(Vector acceleration, double friction)
		{
			this.Acceleration = acceleration; 
			this.Friction = friction;
		}

		public double Friction { get; set; }

		public Vector Acceleration { get; set; }

        class AccelerationBehavior : IParticleBehavior

        {
            public AccelerationDescriptor Descriptor;

            public void Initialize(Particle particle) { }

            public void Update(Particle particle)
            {
                particle.Velocity += Descriptor.Acceleration;

                if (particle.Velocity.Magnitude > Descriptor.Friction)
                    particle.Velocity = Vector.Zero;
                else
                    particle.Velocity -= particle.Velocity.UnitVector * Descriptor.Friction;
            }
        }

        public IParticleBehavior MakeBehavior() => new AccelerationBehavior { Descriptor = this };
	}
}
