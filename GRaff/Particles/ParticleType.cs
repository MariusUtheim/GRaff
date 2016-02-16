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
		private readonly List<IParticleBehavior> _behaviors = new List<IParticleBehavior>();
		private readonly IParticleRenderer _renderer;

#warning Need some better Sprite logic
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
			Contract.Requires<ArgumentNullException>(behavior != null);
			_behaviors.Add(behavior);
		}

		public IDistribution<int> Lifetime { get; private set; }

		public BlendMode BlendMode { get; set; }

		public void Initialize(Particle particle)
		{
			Contract.Requires<ArgumentNullException>(particle != null);
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
