

namespace GRaff.Particles
{
	public class ColorFeature : IParticleFeature
	{
		private Color _color;

		public ColorFeature(Color color)
		{
			this._color = color;
		}

		public void AttachTo(Particle particle)
		{ 
			particle.Blend = _color;
		}
	}
}
