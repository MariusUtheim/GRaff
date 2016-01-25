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

		public TexturedParticleRenderer(Texture texture)
		{
			Texture = texture;
		}

		public Texture Texture { get; private set; }

		public void Render(IEnumerable<Particle> particles)
		{
			int count = particles.Count();
			GraphicsPoint[] vertices = new GraphicsPoint[4 * count];
			Color[] colors = new Color[4 * count];
			throw new NotImplementedException();
			/*
			PointF
				tl = new PointF(-(float)Texture.XOrigin, -(float)Texture.YOrigin),
				tr = new PointF( (float)Texture.XOrigin, -(float)Texture.YOrigin),
				bl = new PointF(-(float)Texture.XOrigin,  (float)Texture.YOrigin),
				br = new PointF( (float)Texture.XOrigin,  (float)Texture.YOrigin);

			Parallel.ForEach(particles, (particle, loopState, index) =>
			{
				index *= 4;
				vertices[index] = (PointF)(particle.TransformationMatrix * tl + particle.Location);
				vertices[index + 1] = (PointF)(particle.TransformationMatrix * tr + particle.Location);
				vertices[index + 2] = (PointF)(particle.TransformationMatrix * br + particle.Location);
				vertices[index + 3] = (PointF)(particle.TransformationMatrix * bl + particle.Location);
				colors[index] = colors[index + 1] = colors[index + 2] = colors[index + 3] = particle.Blend;
			});

			_renderSystem.SetVertices(UsageHint.StreamDraw, vertices);
			_renderSystem.SetColors(UsageHint.StreamDraw, colors);
			_renderSystem.QuadTexCoords(UsageHint.StreamDraw, count);

			ShaderProgram.CurrentTextured.SetCurrent();
			Sprite.Bind();
			_renderSystem.Render(PrimitiveType.Quads, vertices.Length);
			*/
		}
	}
}
