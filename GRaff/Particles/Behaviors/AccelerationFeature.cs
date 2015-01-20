

namespace GRaff.Particles.Behaviors
{
	public class AccelerationFeature : IParticleFeature
	{
		public AccelerationFeature(Vector gravity)
			:this(gravity, 0) { }

		public AccelerationFeature(double friction)
			: this(Vector.Zero, friction) { }

		public AccelerationFeature(Vector gravity, double friction)
		{
			this.Gravity = gravity; 
			this.Friction = friction;
		}

		public double Friction { get; set; }

		public Vector Gravity { get; set; }

		public void AttachTo(Particle particle)
		{
			if (Gravity != Vector.Zero)
				particle.AttachBehavior(new GravityBehavior(Gravity));
			if (Friction != 0)
				particle.AttachBehavior(new FrictionBehavior(Friction));
		}
	}
}
