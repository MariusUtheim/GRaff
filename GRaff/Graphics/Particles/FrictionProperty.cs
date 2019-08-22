 

namespace GRaff.Graphics.Particles
{
	internal class FrictionProperty : IParticleProperty
	{
		public FrictionProperty(double friction)
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