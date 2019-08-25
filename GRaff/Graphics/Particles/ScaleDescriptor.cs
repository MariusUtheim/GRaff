using System;
using GRaff.Randomness;
using System.Diagnostics.Contracts;

namespace GRaff.Graphics.Particles
{
	public class ScaleDescriptor : IParticleTypeDescriptor 
	{
		private readonly IDistribution<double> _xScale, _yScale;

		public ScaleDescriptor(double scale)
		{
			_xScale = new ConstantDistribution<double>(scale);
			_yScale = null;
		}

		public ScaleDescriptor(double xScale, double yScale)
		{
			_xScale = new ConstantDistribution<double>(xScale);
			_yScale = new ConstantDistribution<double>(yScale);
		}

		public ScaleDescriptor(IDistribution<double> scaleDistribution)
		{
			Contract.Requires<ArgumentNullException>(scaleDistribution != null);
			_xScale = scaleDistribution;
			_yScale = null;
		}
		
		public ScaleDescriptor(IDistribution<double> xScaleDistribution, IDistribution<double> yScaleDistribution)
		{
			Contract.Requires<ArgumentNullException>(xScaleDistribution != null);
			Contract.Requires<ArgumentNullException>(yScaleDistribution != null);
			_xScale = xScaleDistribution;
			_yScale = yScaleDistribution;
		}

        public IDistribution<double> InitialXScaleDistribution { get; set; }
        public IDistribution<double> InitialYScaleDistribution { get; set; }
        public IDistribution<double> XScaleFactorDistribution { get; set; }
        public IDistribution<double> YScaleFactorDistribution { get; set; }

        class ScaleBehavior : IParticleBehavior
        {
            private double _initialXScale, _initialYScale;
            private Matrix _scaler;

            public ScaleBehavior(double initialXScale, double initialYScale, double xScaleFactor, double yScaleFactor)
            {
                this._initialXScale = initialXScale;
                this._initialYScale = initialYScale;
                _scaler = (xScaleFactor == 1 && yScaleFactor == 1) ? null : Matrix.Scaling(xScaleFactor, yScaleFactor);
            }

            public void Initialize(Particle particle)
            {
                particle.TransformationMatrix = particle.TransformationMatrix.Scale(_initialXScale, _initialYScale);
            }

            public void Update(Particle particle)
            {
                if (_scaler != null)
                    particle.TransformationMatrix = _scaler * particle.TransformationMatrix;
            }
        }

        public IParticleBehavior MakeBehavior()
        {
            return new ScaleBehavior(InitialXScaleDistribution.Generate(), InitialYScaleDistribution.Generate(),
                XScaleFactorDistribution.Generate(), YScaleFactorDistribution.Generate());
        }
	}
}