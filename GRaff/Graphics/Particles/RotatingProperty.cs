
 
namespace GRaff.Graphics.Particles
{
	public class RotatingProperty : IParticleProperty
	{
		private readonly Angle _rotation;

		public RotatingProperty(Angle rotation)
		{
			this._rotation = rotation;
		}

		public void Update(Particle particle)
		{
			particle.TransformationMatrix = particle.TransformationMatrix.Rotate(_rotation);
		}
	}
}
