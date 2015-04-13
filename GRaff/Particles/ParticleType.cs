using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Randomness;
using System.Diagnostics.Contracts;

namespace GRaff.Particles
{
	public class ParticleType
	{
		List<IParticleBehavior> _behaviors = new List<IParticleBehavior>();
		IParticleRenderer _renderer;

		public ParticleType(Sprite sprite, int lifetime)
		{
			_renderer = new TexturedParticleRenderer(sprite);			
			this.Lifetime = new ConstantDistribution<int>(lifetime);
		}

		public ParticleType(Polygon polygon, int lifetime)
		{
			_renderer = new ColoredParticleRenderer(polygon);
			this.Lifetime = new ConstantDistribution<int>(lifetime);
		}

		public ParticleSystem Burst(Point location, int count)
		{
			var system = new ParticleSystem(this);
			system.Create(location, count);
			return system;
		}

		public Particle Generate(double x, double y)
		{
			var result = new Particle(x, y, Lifetime.Generate());
			foreach (var behavior in _behaviors)
				behavior.AttachTo(result);
			return result;
		}

		public void AddBehavior(IParticleBehavior behavior)
		{
			Contract.Requires(behavior != null);
			_behaviors.Add(behavior);
		}

		public IDistribution<int> Lifetime { get; set; }

		public BlendMode BlendMode { get; set; }

		public void Initialize(Particle particle)
		{
			foreach (var behavior in _behaviors)
				behavior.AttachTo(particle);
		}

		internal void Render(IEnumerable<Particle> particles)
		{
			if (BlendMode == null || BlendMode == ColorMap.BlendMode)
				_renderer.Render(particles);
			else
			{
				var previousBlend = ColorMap.BlendMode;
				ColorMap.BlendMode = this.BlendMode;
				_renderer.Render(particles);
				ColorMap.BlendMode = previousBlend;
			}
		}
	}
}
