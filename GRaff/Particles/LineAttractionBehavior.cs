using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class LineAttractionBehavior : IParticleBehavior
	{
		class LineAttractionProperty : IParticleProperty
		{
			private LineAttractionBehavior _source;

			public LineAttractionProperty(LineAttractionBehavior source)
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

		public LineAttractionBehavior(Line line, double strength)
		{
			this.Line = line;
			this.Strength = strength;
		}

		public Line Line { get; set; }

		public double Strength { get; set; }

		public void AttachTo(Particle particle)
		{
			particle.AttachProperty(new LineAttractionProperty(this));
		}
	}
}
