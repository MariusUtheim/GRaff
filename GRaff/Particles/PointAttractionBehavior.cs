using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class PointAttractionBehavior : IParticleBehavior
	{
		class PointAttractionProperty : IParticleProperty
		{
			private PointAttractionBehavior _source;

			public PointAttractionProperty(PointAttractionBehavior source)
			{
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
			particle.AttachProperty(new PointAttractionProperty(this));
		}
	}
}
