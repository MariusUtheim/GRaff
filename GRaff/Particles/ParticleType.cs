using System.Collections.Generic;


namespace GRaff.Particles
{
	public class ParticleType
	{
		private List<IParticleBehavior> _behaviors;

		public ParticleType(Sprite sprite, int lifetime)
		{
			this.Sprite = sprite;
			this.Lifetime = lifetime;
			_behaviors = new List<IParticleBehavior>();
		}

		public ParticleSystem Burst(Point location, int count)
		{
			var system = new ParticleSystem(this);
			system.Create(location, count);
			return system;
		}

		public void AddBehavior(IParticleBehavior behavior)
		{
			_behaviors.Add(behavior);
		}

		public int Lifetime { get; set; }

		public Sprite Sprite { get; set; }

		public void Initialize(Particle particle)
		{
			foreach (var behavior in _behaviors)
			{
				var property = behavior.Generate();
				if (property != null)
					particle.AddProperty(property);
				behavior.Initialize(particle);
			}
		}
	}
}
