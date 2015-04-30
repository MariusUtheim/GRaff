using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public sealed class Particle
	{
		internal List<IParticleProperty> Properties = new List<IParticleProperty>();

		public Particle(double x, double y, int lifetime)
		{
			Location = new Point(x, y);
			TransformationMatrix = new LinearMatrix();
			Blend = Colors.White;
			TotalLifetime = lifetime;
		}

		public Point Location { get; set; }

		public Vector Velocity { get; set; }

		public LinearMatrix TransformationMatrix { get; set; }

		public Color Blend { get; set; }

		public int TotalLifetime { get; set; }

		public int Lifetime { get; set; }


		internal bool Update()
		{
			if (++Lifetime >= TotalLifetime)
				return false;

			Location += Velocity;
			foreach (var property in Properties)
				property.Update(this);

			return true;
		}

		public void AttachBehavior(IParticleProperty behavior)
		{
			Properties.Add(behavior);
		}
	}
}
