

namespace GRaff.Particles
{
	public class AccelerationBehavior : IParticleBehavior
	{
		public AccelerationBehavior(Vector gravity)
			:this(gravity, 0) { }

		public AccelerationBehavior(double friction)
			: this(Vector.Zero, friction) { }

		public AccelerationBehavior(Vector gravity, double friction)
		{
			this.Gravity = gravity; 
			this.Friction = friction;
		}

		public double Friction { get; set; }

		public Vector Gravity { get; set; }

		public void AttachTo(Particle particle)
		{
			if (Gravity != Vector.Zero)
				particle.AttachBehavior(new GravityProperty(Gravity));
			if (Friction != 0)
				particle.AttachBehavior(new FrictionProperty(Friction));
		}
	}
}
