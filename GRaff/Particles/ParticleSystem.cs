using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.Particles
{
	public class ParticleSystem : GameElement
	{
		protected readonly LinkedList<Particle> particles = new LinkedList<Particle>();

		// Hiding this from subclasses 
		private readonly TexturedRenderSystem _renderSystem;

		public int Count { get { return particles.Count; } }

		public ParticleSystem(ParticleType type)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			_renderSystem = new TexturedRenderSystem();
			this.ParticleType = type;
		}
		
		public ParticleType ParticleType { get; private set; }

		protected void Remove(Particle particle)
		{
			Contract.Requires<ArgumentNullException>(particle != null);
			particles.Remove(particle);
		}

		public void Create(double x, double y, int count)
		{
			for (int i = 0; i < count; i++)
				particles.AddFirst(ParticleType.Generate(x, y));
		}

		public void Create(Point location, int count)
		{
			Create(location.X, location.Y, count);
		}

		public void Create(IEnumerable<Point> pts)
		{
			Contract.Requires<ArgumentNullException>(pts != null);
			foreach (var p in pts)
				Create(p.X, p.Y, 1);
		}

		public override void OnStep()
		{
			ConcurrentBag<Particle> removeBag = new ConcurrentBag<Particle>();
			Parallel.ForEach(particles, particle =>
			{
				if (!particle.Update())
					removeBag.Add(particle);
			});

			var toRemove = new HashSet<Particle>(removeBag.ToArray());

			for (var next = particles.First; next != null; )
			{
				var current = next;
				next = next.Next;
				if (toRemove.Contains(current.Value))
					particles.Remove(current);
			}
		}

		public override void OnDraw()
		{
			ParticleType.Render(particles);
		}
	}
}
