using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.Particles
{
	internal class TexturedParticleRenderer : IParticleRenderer
	{
		private readonly RenderSystem _renderSystem = new RenderSystem();
		private readonly double _animationSpeed;
		private double _frame = 0;

		public TexturedParticleRenderer(Sprite sprite, double animationSpeed)
		{
			Contract.Requires<ArgumentNullException>(sprite != null);
			this.Sprite = sprite;
			this._animationSpeed = animationSpeed;
		}

		public Sprite Sprite { get; private set; }

		public void Render(IEnumerable<Particle> particles)
		{
			if (particles == null) return;
			int count = particles.Count();
			GraphicsPoint[] vertices = new GraphicsPoint[4 * count];
			Color[] colors = new Color[4 * count];
			GraphicsPoint[] texCoords = new GraphicsPoint[4 * count];

#warning
			//throw new NotImplementedException();

			GraphicsPoint
				tl = new GraphicsPoint(-Sprite.XOrigin, -Sprite.YOrigin),
				tr = new GraphicsPoint(Sprite.Width - Sprite.XOrigin, -Sprite.YOrigin),
				br = new GraphicsPoint(Sprite.Width - Sprite.XOrigin, Sprite.Height - Sprite.YOrigin),
				bl = new GraphicsPoint(-Sprite.XOrigin, Sprite.Height - Sprite.YOrigin);

			Parallel.ForEach(particles, (particle, loopState, index) =>
			{
				index *= 4;
				vertices[index] = (GraphicsPoint)(particle.TransformationMatrix * tl + particle.Location);
				vertices[index + 1] = (GraphicsPoint)(particle.TransformationMatrix * tr + particle.Location);
				vertices[index + 2] = (GraphicsPoint)(particle.TransformationMatrix * br + particle.Location);
				vertices[index + 3] = (GraphicsPoint)(particle.TransformationMatrix * bl + particle.Location);
				colors[index] = colors[index + 1] = colors[index + 2] = colors[index + 3] = particle.Blend;

				var baseCoords = Sprite.SubImage(_frame).QuadCoords;
				for (var i = 0; i < 4; i++)
					texCoords[index + i] = baseCoords[i];
			});

			

			_renderSystem.SetVertices(UsageHint.StreamDraw, vertices);
			_renderSystem.SetColors(UsageHint.StreamDraw, colors);
			_renderSystem.SetTexCoords(UsageHint.StreamDraw, texCoords);
            
            _renderSystem.Render(Sprite.SubImage(_frame).Buffer, PrimitiveType.Quads);

			_frame += -_animationSpeed;
		}
	}
}
