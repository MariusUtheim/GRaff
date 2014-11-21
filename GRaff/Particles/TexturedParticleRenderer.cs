using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.Particles
{
	internal class TexturedParticleRenderer : IParticleRenderer
	{
		TexturedRenderSystem _renderSystem = new TexturedRenderSystem();

		public TexturedParticleRenderer(Sprite sprite)
		{
			Sprite = sprite;
		}

		public Sprite Sprite { get; set; }

		public void Render(IEnumerable<Particle> particles)
		{
			int count = particles.Count();
			Point[] vertices = new Point[4 * count];
			Color[] colors = new Color[4 * count];

			Point
				tl = new Point(-Sprite.XOrigin, -Sprite.YOrigin),
				tr = new Point(Sprite.XOrigin, -Sprite.YOrigin),
				bl = new Point(-Sprite.XOrigin, Sprite.YOrigin),
				br = new Point(Sprite.XOrigin, Sprite.YOrigin);

			Parallel.ForEach(particles, (particle, loopState, index) =>
			{
				index *= 4;
				vertices[index] = particle.TransformationMatrix * tl + particle.Location;
				vertices[index + 1] = particle.TransformationMatrix * tr + particle.Location;
				vertices[index + 2] = particle.TransformationMatrix * br + particle.Location;
				vertices[index + 3] = particle.TransformationMatrix * bl + particle.Location;
				colors[index] = colors[index + 1] = colors[index + 2] = colors[index + 3] = particle.Color;
			});

			_renderSystem.SetVertices(UsageHint.StreamDraw, vertices);
			_renderSystem.SetColors(UsageHint.StreamDraw, colors);
			_renderSystem.QuadTexCoords(UsageHint.StreamDraw, count);

			ShaderProgram.Current = ShaderProgram.DefaultTextured;
			Sprite.Texture.Bind();
			_renderSystem.Render(PrimitiveType.Quads, vertices.Length);
		}
	}
}
