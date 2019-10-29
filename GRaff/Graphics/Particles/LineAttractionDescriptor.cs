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
			private LineAttractionDescriptor _descriptor;

            public LineAttractionBehavior(LineAttractionDescriptor descriptor)
            {
                this._descriptor = descriptor;
            }

			public void Initialize(Particle particle) { }

			public void Update(Particle particle)
			{
				Vector d = particle.Location - _descriptor.Line.Origin;
				Vector r = _descriptor.Strength * _descriptor.Line.LeftNormal.Dot(d) * _descriptor.Line.RightNormal;
				particle.Velocity += r;
			}
		}

        public IParticleBehavior MakeBehavior() => new LineAttractionBehavior(this);
	}
}
