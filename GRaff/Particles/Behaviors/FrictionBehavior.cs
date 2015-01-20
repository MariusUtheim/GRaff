 

namespace GRaff.Particles.Behaviors
{
	internal class FrictionBehavior : IParticleBehavior
	{
		public FrictionBehavior(double friction)
		{
			this.Friction = friction;
		}

		public double Friction { get; set; }

		public void Update(Particle particle)
		{
			if (particle.Velocity.Magnitude > Friction)
				particle.Velocity = Vector.Zero;
			else
				particle.Velocity -= particle.Velocity.UnitVector * Friction;
		}
	}
}