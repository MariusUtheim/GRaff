using System;
using GRaff.Randomness;


namespace GRaff.Graphics.Particles
{
	public class ScaleDescriptor : IParticleTypeDescriptor 
	{
        private readonly IDistribution<double> _xScale;
        private readonly IDistribution<double>? _yScale;

        public ScaleDescriptor(IDistribution<double> scaleDistribution)
        {
            _xScale = scaleDistribution;
            _yScale = null;
        }

		public ScaleDescriptor(IDistribution<double> xScaleDistribution, IDistribution<double> yScaleDistribution)
		{
			_xScale = xScaleDistribution;
			_yScale = yScaleDistribution;
		}

        public static ScaleDescriptor Constant(double scale)
            => new ScaleDescriptor(ConstantDistribution.Create(scale));

        public static ScaleDescriptor Uniform(double scaleMin, double scaleMax)
            => new ScaleDescriptor(new DoubleDistribution(scaleMin, scaleMax));

        class ScaleBehavior : IParticleBehavior
        {
            private readonly Matrix _transform;

            public ScaleBehavior(double xScale, double yScale)
            {
                _transform = Matrix.Scaling(xScale, yScale);
            }

            public void Initialize(Particle particle)
                => particle.TransformationMatrix = _transform * particle.TransformationMatrix;

            public void Update(Particle particle) { }
        }

        public IParticleBehavior MakeBehavior()
        {
            double xScale = _xScale.Generate();
            double yScale = _yScale?.Generate() ?? xScale;
            return new ScaleBehavior(xScale, yScale);
        }
	}
}