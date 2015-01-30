using System;
using GRaff.Randomness;


namespace GRaff.Particles.Behaviors
{
	public class ScaleFeature : IParticleFeature 
	{
		private IDistribution<double> _xScale, _yScale;

		public ScaleFeature(double scale)
		{
			_xScale = _yScale = new ConstantDistribution<double>(scale);
		}

		public ScaleFeature(double xScale, double yScale)
		{
			_xScale = new ConstantDistribution<double>(xScale);
			_yScale = new ConstantDistribution<double>(yScale);
		}

		public ScaleFeature(IDistribution<double> xScaleDistribution, IDistribution<double> yScaleDistribution)
		{
			if (xScaleDistribution == null) throw new ArgumentNullException("xScaleDistribution"); /*C#6.0*/
			if (yScaleDistribution == null) throw new ArgumentNullException("yScaleDistribution");
			_xScale = xScaleDistribution;
			_yScale = yScaleDistribution;
		}

		public void AttachTo(Particle particle)
		{
			particle.TransformationMatrix = particle.TransformationMatrix.Scale(_xScale.Generate(), _yScale.Generate());
		}
	}
}