 

namespace GRaff.Particles
{
	public class GravityProperty : IParticleProperty
	{
		public GravityProperty(Vector gravity)
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