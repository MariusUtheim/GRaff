using System;
using GRaff.Randomness;
using System.Diagnostics.Contracts;

namespace GRaff.Graphics.Particles
{
	public class ScaleBehavior : IParticleBehavior 
	{
		private readonly IDistribution<double> _xScale, _yScale;

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
			Contract.Requires<ArgumentNullException>(scaleDistribution != null);
			_xScale = scaleDistribution;
			_yScale = null;
		}
		
		public ScaleBehavior(IDistribution<double> xScaleDistribution, IDistribution<double> yScaleDistribution)
		{
			Contract.Requires<ArgumentNullException>(xScaleDistribution != null);
			Contract.Requires<ArgumentNullException>(yScaleDistribution != null);
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