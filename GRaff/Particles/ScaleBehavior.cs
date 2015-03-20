using System;
using GRaff.Randomness;


namespace GRaff.Particles
{
	public class ScaleBehavior : IParticleBehavior 
	{
		private IDistribution<double> _xScale, _yScale;

		public ScaleBehavior(double scale)
		{
			_xScale = new ConstantDistribution<double>(scale);
			_yScale = null;
		}

		public ScaleBehavior(double xScale, double yScale)
		{
			_xScale = new ConstantDistribution<double>(xScale);
			_yScale = new ConstantDistribution<double>(yScale);
		}

		public ScaleBehavior(IDistribution<double> scaleDistribution)
		{
			_xScale = scaleDistribution;
			_yScale = null;
		}
		
		public ScaleBehavior(IDistribution<double> xScaleDistribution, IDistribution<double> yScaleDistribution)
		{
			if (xScaleDistribution == null) throw new ArgumentNullException("xScaleDistribution"); /*C#6.0*/
			if (yScaleDistribution == null) throw new ArgumentNullException("yScaleDistribution");
			_xScale = xScaleDistribution;
			_yScale = yScaleDistribution;
		}

		public void AttachTo(Particle particle)
		{
			double xScale = _xScale.Generate();
			double yScale = _yScale != null ? _yScale.Generate() : xScale; /*C#6.0*/
			particle.TransformationMatrix = particle.TransformationMatrix.Scale(xScale, yScale);
		}
	}
}