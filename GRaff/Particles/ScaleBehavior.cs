using System;
using GRaff.Randomness;
using System.Diagnostics.Contracts;

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
			Contract.Requires(xScaleDistribution != null);
			Contract.Requires(yScaleDistribution != null);
			_xScale = xScaleDistribution;
			_yScale = yScaleDistribution;
		}

		public void AttachTo(Particle particle)
		{
			double xScale = _xScale.Generate();
			double yScale = _yScale?.Generate() ?? xScale;
			particle.TransformationMatrix = particle.TransformationMatrix.Scale(xScale, yScale);
		}
	}
}