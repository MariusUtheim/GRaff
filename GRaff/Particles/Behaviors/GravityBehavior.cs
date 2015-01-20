 

namespace GRaff.Particles.Behaviors
{
	public class GravityBehavior : IParticleBehavior
	{
		public GravityBehavior(Vector gravity)
		{
			this.Gravity = gravity;
		}

		public Vector Gravity { get; private set; }

		public void Update(Particle particle)
		{
			particle.Velocity += Gravity;
		}
	}
}