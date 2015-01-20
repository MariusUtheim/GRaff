using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Randomness;


namespace GRaff.Particles
{
	public class ParticleType
	{
		List<IParticleFeature> _behaviors = new List<IParticleFeature>();
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

		public void AddFeature(IParticleFeature feature)
		{
			if (feature == null) throw new ArgumentNullException("behavior");
			_behaviors.Add(feature);
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
			if (BlendMode == null)
				_renderer.Render(particles);
			else
				using (var blendMode = new BlendModeContext(BlendMode))
					_renderer.Render(particles);
		}
	}
}
