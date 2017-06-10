using System;
using System.Diagnostics.Contracts;

namespace GRaff.Particles
{
	public class PointAttractionBehavior : IParticleBehavior
	{
		class PointAttractionProperty : IParticleProperty
		{
			private readonly PointAttractionBehavior _source;

			public PointAttractionProperty(PointAttractionBehavior source)
			{
				Contract.Requires<ArgumentNullException>(source != null);
				_source = source;
			}

			public void Update(Particle particle)
			{
				Vector r = _source.Location - particle.Location;
				particle.Velocity += _source.Strength * r / GMath.Pow(r.Magnitude, 1);
			}
		}

		public PointAttractionBehavior(Point location, double strength)
		{
			this.Location = location;
			this.Strength = strength;
		}

		public Point Location { get; set; }

		public double Strength { get; set; }

		public void AttachTo(Particle particle)
		{
			particle.AttachBehavior(new PointAttractionProperty(this));
		}
	}
}
