

namespace GRaff.Particles
{
	public class ScalingProperty : IParticleProperty
	{
		private double _xScaleFactor;
		private double _yScaleFactor;

		public ScalingProperty(double scaleFactor) 
		{
			this._xScaleFactor = this._yScaleFactor = scaleFactor;
		}

		public ScalingProperty(double xScaleFactor, double yScaleFactor)
		{
			this._xScaleFactor = xScaleFactor;
			this._yScaleFactor = yScaleFactor;
		}

		public void Update(Particle particle)
		{
			particle.TransformationMatrix = particle.TransformationMatrix.Scale(_xScaleFactor, _yScaleFactor);
		}
	}
}
