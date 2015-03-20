

namespace GRaff.Particles
{
	public class ScalingBehavior : IParticleBehavior
	{
		private double _xScaleFactor, _yScaleFactor;

		public ScalingBehavior(double scaleFactor)
		{
			_xScaleFactor = _yScaleFactor = scaleFactor;
		}

		public ScalingBehavior(double xScaleFactor, double yScaleFactor)
		{
			_xScaleFactor = xScaleFactor;
			_yScaleFactor = yScaleFactor;
		}


		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new ScalingProperty(_xScaleFactor, _yScaleFactor));
		}
	}
}
