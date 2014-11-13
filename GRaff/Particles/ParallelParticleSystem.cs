using System.Collections.Concurrent;
using System.Threading.Tasks;
using GRaff.OpenGL;

namespace GRaff.Particles
{
	public class ParallelParticleSystem : ParticleSystem
	{
		private GLRenderSystem _renderSystem = new GLRenderSystem();

		public ParallelParticleSystem(ParticleType type)
			: base(type) { }

		public override void OnStep()
		{
			ConcurrentBag<Particle> toRemove = new ConcurrentBag<Particle>();
			Parallel.ForEach(this, particle => {
				if (!particle.Update())
					toRemove.Add(particle);
			});

			foreach (var particle in toRemove)
				Remove(particle);
		}

		public override void OnDraw()
		{
			Point[] vertices = new Point[4 * Count];
			Color[] colors = new Color[4 * Count];

			Point
				tl = new Point(-ParticleType.Sprite.XOrigin, -ParticleType.Sprite.YOrigin),
				tr = new Point(ParticleType.Sprite.XOrigin, -ParticleType.Sprite.YOrigin),
				bl = new Point(-ParticleType.Sprite.XOrigin, ParticleType.Sprite.YOrigin),
				br = new Point(ParticleType.Sprite.XOrigin, ParticleType.Sprite.YOrigin);

			Parallel.ForEach(this, (particle, loopState, index) => {
				index *= 4;
				vertices[index] = particle.TransformationMatrix * tl;
				vertices[index + 1] = particle.TransformationMatrix * tr;
				vertices[index + 2] = particle.TransformationMatrix * br;
				vertices[index + 3] = particle.TransformationMatrix * bl;
				colors[index] = colors[index + 1] = colors[index + 2] = colors[index + 3] = particle.Color;
			});

			_renderSystem.SetVertices(UsageHint.StreamDraw, vertices);
			_renderSystem.SetColors(UsageHint.StreamDraw, colors);
			_renderSystem.QuadTexCoords(UsageHint.StreamDraw, Count);

			ShaderProgram.Current = ShaderProgram.DefaultTextured;
			ParticleType.Sprite.Texture.Bind();
			_renderSystem.Render(PrimitiveType.Quads, vertices.Length);
		}
	}
}
