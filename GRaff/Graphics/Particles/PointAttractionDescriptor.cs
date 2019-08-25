using System;
using System.Diagnostics.Contracts;

namespace GRaff.Graphics.Particles
{
	public class PointAttractionDescriptor : IParticleTypeDescriptor
	{

		public PointAttractionDescriptor(Point location, double strength)
		{
			this.Location = location;
			this.Strength = strength;
		}

		public Point Location { get; set; }

		public double Strength { get; set; }


        class PointAttractionBehavior : IParticleBehavior

        {
            public PointAttractionDescriptor Descriptor;

            public void Initialize(Particle particle) { }

            public void Update(Particle particle)
            {
                Vector r = Descriptor.Location - particle.Location;
                particle.Velocity += Descriptor.Strength * r / GMath.Pow(r.Magnitude, 1);
            }
        }

        public IParticleBehavior  MakeBehavior() => new PointAttractionBehavior { Descriptor = this };
	}
}
