

namespace GRaff.Particles.Behaviors
{
	public class ScalingFeature : IParticleFeature
	{
		private double _xScaleFactor, _yScaleFactor;

		public ScalingFeature(double scaleFactor)
		{
			_xScaleFactor = _yScaleFactor = scaleFactor;
		}

		public ScalingFeature(double xScaleFactor, double yScaleFactor)
		{
			_xScaleFactor = xScaleFactor;
			_yScaleFactor = yScaleFactor;
		}


		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new ScalingBehavior(_xScaleFactor, _yScaleFactor));
		}
	}
}
