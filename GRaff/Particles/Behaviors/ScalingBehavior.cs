

namespace GRaff.Particles.Behaviors
{
	public class ScalingBehavior : IParticleBehavior
	{
		private double _xScaleFactor;
		private double _yScaleFactor;

		public ScalingBehavior(double scaleFactor) 
		{
			this._xScaleFactor = this._yScaleFactor = scaleFactor;
		}

		public ScalingBehavior(double xScaleFactor, double yScaleFactor)
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
