using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public sealed class Particle
	{
		public Particle(double x, double y, ParticleType type)
		{
			Properties = new List<IParticleProperty>();
			TransformationMatrix = Matrix.Translation(x, y);
			Color = Color.White;
			TotalLifetime = type.Lifetime;
			type.Initialize(this);
		}

		public Matrix TransformationMatrix { get; set; }

		public Color Color { get; set; }

		public int TotalLifetime { get; set; }

		public int Lifetime { get; set; }

		internal List<IParticleProperty> Properties;

		internal bool Update()
		{
			if (++Lifetime >= TotalLifetime)
				return false;
			foreach (var property in Properties)
				property.Update(this);
			return true;
		}

		internal void AddProperty(IParticleProperty property)
		{
			Properties.Add(property);
		}
	}
}
