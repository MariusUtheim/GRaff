using System;
using System.Diagnostics.Contracts;

namespace GRaff.Graphics.Particles
{
	public class LineAttractionDescriptor : IParticleTypeDescriptor
	{

		public LineAttractionDescriptor(Line line, double strength)
		{
			this.Line = line;
			this.Strength = strength;
		}

		public Line Line { get; set; }

		public double Strength { get; set; }


		class LineAttractionBehavior : IParticleBehavior
      
		{
			public LineAttractionDescriptor Descriptor;

			public void Initialize(Particle particle) { }

			public void Update(Particle particle)
			{
				Vector d = particle.Location - Descriptor.Line.Origin;
				Vector r = Descriptor.Strength * Descriptor.Line.LeftNormal.Dot(d) * Descriptor.Line.RightNormal;
				particle.Velocity += r;
			}
		}

        public IParticleBehavior  MakeBehavior() => new LineAttractionBehavior { Descriptor = this };
	}
}
