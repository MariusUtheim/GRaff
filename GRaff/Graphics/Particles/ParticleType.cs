using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Randomness;
using System.Diagnostics.Contracts;

namespace GRaff.Graphics.Particles
{
	public class ParticleType
	{
		private readonly List<IParticleTypeDescriptor> _descriptors = new List<IParticleTypeDescriptor>();
		private readonly IParticleRenderer _renderer;

		public ParticleType(Sprite sprite, int lifetime, double animationSpeed = 1.0)
		{
			Contract.Requires<ArgumentNullException>(sprite != null);
			_renderer = new TexturedParticleRenderer(sprite, animationSpeed);
			this.Lifetime = new ConstantDistribution<int>(lifetime);
		}


		public ParticleType(Polygon polygon, int lifetime)
		{
			Contract.Requires<ArgumentNullException>(polygon != null);
			_renderer = new ColoredParticleRenderer(polygon);
			this.Lifetime = new ConstantDistribution<int>(lifetime);
		}

		public ParticleSystem Burst(Point location, int count)
		{
			var system = new ParticleSystem(this);
			system.DestroyAutomatically = true;
			system.Create(location, count);
			return Instance.Create(system);
		}

		public Particle Generate(double x, double y)
		{
			var result = new Particle(x, y, Lifetime.Generate());
			foreach (var descriptor in _descriptors)
				result.AttachBehavior(descriptor.MakeBehavior());
			return result;
		}

		public void AddDescriptors(IEnumerable<IParticleTypeDescriptor> descriptors)
		{
			foreach (var descriptor in descriptors)
				AddDescriptor(descriptor);
		}

		public void AddDescriptor(IParticleTypeDescriptor descriptor)
		{
			_descriptors.Add(descriptor);
		}

		public IDistribution<int> Lifetime { get; private set; }

		public BlendMode BlendMode { get; set; }

		internal void Render(IEnumerable<Particle> particles)
		{
			if (BlendMode == null || BlendMode == BlendMode.Current)
				_renderer.Render(particles);
			else
                using (BlendMode.Use())
				    _renderer.Render(particles);
		}
	}
}
