

namespace GRaff.Particles.Behaviors
{
	public class DynamicScaleFeature : IParticleFeature
	{
		private double _xScaleFactor, _yScaleFactor;

		public DynamicScaleFeature(double scaleFactor)
			: this(scaleFactor, scaleFactor)
		{ }

		public DynamicScaleFeature(double xScaleFactor, double yScaleFactor)
		{
			this._xScaleFactor = xScaleFactor;
			this._yScaleFactor = yScaleFactor;
		} 
		
		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new ScalingBehavior(_xScaleFactor, _yScaleFactor));
		}
	}
}
