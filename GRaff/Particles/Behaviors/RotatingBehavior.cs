
 
namespace GRaff.Particles.Behaviors
{
	public class RotatingBehavior : IParticleBehavior
	{
		private Angle _rotation;

		public RotatingBehavior(Angle rotation)
		{
			this._rotation = rotation;
		}

		public void Update(Particle particle)
		{
			particle.TransformationMatrix.Rotate(_rotation);
		}
	}
}
