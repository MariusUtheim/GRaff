using System.Collections;
using System.Collections.Generic;
using GRaff.OpenGL;

namespace GRaff.Particles
{
	public class ParticleSystem : GameElement, IEnumerable<Particle>
	{
		private MutableList<Particle> particles;

		// Hiding this from subclasses 
		private GLRenderSystem _renderSystem;

		public int Count { get { return particles.Count; } }

		public ParticleSystem(ParticleType type)
		{
			particles = new MutableList<Particle>();
			_renderSystem = new GLRenderSystem();
			this.ParticleType = type;
		}
		
		public ParticleType ParticleType { get; private set; }

		public IEnumerator<Particle> GetEnumerator()
		{
			return particles.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected void Remove(Particle particle)
		{
			particles.Remove(particle);
		}

		public void Create(double x, double y, int count)
		{
			for (int i = 0; i < count; i++)
				particles.Add(new Particle(x, y, ParticleType));
		}

		public void Create(Point location, int count)
		{
			Create(location.X, location.Y, count);
		}

		public override void OnStep()
		{
			foreach (var particle in this)
				if (!particle.Update())
					particles.Remove(particle);
		}

		public override void OnDraw()
		{
			Point[] vertices = new Point[4 * particles.Count];
			Color[] colors = new Color[4 * particles.Count];

			Point
				tl = new Point(-ParticleType.Sprite.XOrigin, -ParticleType.Sprite.YOrigin),
				tr = new Point(ParticleType.Sprite.XOrigin, -ParticleType.Sprite.YOrigin),
				bl = new Point(-ParticleType.Sprite.XOrigin, ParticleType.Sprite.YOrigin),
				br = new Point(ParticleType.Sprite.XOrigin, ParticleType.Sprite.YOrigin);

			int c = 0;
			foreach (var particle in particles)
			{
				vertices[c] = particle.TransformationMatrix * tl;
				vertices[c + 1] = particle.TransformationMatrix * tr;
				vertices[c + 2] = particle.TransformationMatrix * br;
				vertices[c + 3] = particle.TransformationMatrix * bl;
				colors[c] = colors[c + 1] = colors[c + 2] = colors[c + 3] = particle.Color;
				c += 4;
			}

			_renderSystem.SetVertices(UsageHint.StreamDraw, vertices);
			_renderSystem.SetColors(UsageHint.StreamDraw, colors);
			_renderSystem.QuadTexCoords(UsageHint.StreamDraw, particles.Count);

			ShaderProgram.Current = ShaderProgram.DefaultTextured;
			ParticleType.Sprite.Texture.Bind();
			_renderSystem.Render(PrimitiveType.Quads, vertices.Length);
		}
	}
}
