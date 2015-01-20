
 
namespace GRaff.Particles.Behaviors
{
	public class LineAttractionFeature : IParticleFeature
	{
		class LineAttractionBehavior : IParticleBehavior
		{
			private LineAttractionFeature _source;

			public LineAttractionBehavior(LineAttractionFeature source)
			{
				_source = source;
			}

			public void Update(Particle particle)
			{
				Vector d = particle.Location - _source.Line.Origin;
				Vector r = _source.Strength * _source.Line.LeftNormal.DotProduct(d) * _source.Line.RightNormal;
				particle.Velocity += r;
			}
		}

		public LineAttractionFeature(Line line, double strength)
		{
			this.Line = line;
			this.Strength = strength;
		}

		public Line Line { get; set; }

		public double Strength { get; set; }

		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new LineAttractionBehavior(this));
		}
	}
}
