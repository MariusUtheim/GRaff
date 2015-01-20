 

namespace GRaff.Particles.Behaviors
{
	public class PointAttractionFeature : IParticleFeature
	{
		class PointAttractionBehavior : IParticleBehavior
		{
			private PointAttractionFeature _source;

			public PointAttractionBehavior(PointAttractionFeature source)
			{
				_source = source;
			}

			public void Update(Particle particle)
			{
				Vector r = _source.Location - particle.Location;
				particle.Velocity += _source.Strength * r / GMath.Pow(r.Magnitude, 1);
			}
		}

		public PointAttractionFeature(Point location, double strength)
		{
			this.Location = location;
			this.Strength = strength;
		}

		public Point Location { get; set; }

		public double Strength { get; set; }

		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new PointAttractionBehavior(this));
		}
	}
}
