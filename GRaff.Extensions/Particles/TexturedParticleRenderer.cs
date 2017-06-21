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
		private readonly SerialRenderSystem _renderSystem = new SerialRenderSystem();
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
			if (particles == null || !particles.Any()) return;
			int count = particles.Count();
			GraphicsPoint[] vertices = new GraphicsPoint[6 * count];
            //TODO// Allow more advanced blending
			GraphicsPoint[] texCoords = new GraphicsPoint[6 * count];

			GraphicsPoint
				tl = new GraphicsPoint(-Sprite.XOrigin, -Sprite.YOrigin),
				tr = new GraphicsPoint(Sprite.Width - Sprite.XOrigin, -Sprite.YOrigin),
				br = new GraphicsPoint(Sprite.Width - Sprite.XOrigin, Sprite.Height - Sprite.YOrigin),
				bl = new GraphicsPoint(-Sprite.XOrigin, Sprite.Height - Sprite.YOrigin);

			Parallel.ForEach(particles, (particle, loopState, index) =>
			{
				index *= 6;
				vertices[index] = (GraphicsPoint)(particle.TransformationMatrix * tl + particle.Location);
				vertices[index + 1] = vertices[index + 3] = (GraphicsPoint)(particle.TransformationMatrix * tr + particle.Location);
				vertices[index + 2] = vertices[index + 4] = (GraphicsPoint)(particle.TransformationMatrix * bl + particle.Location);
                vertices[index + 5] = (GraphicsPoint)(particle.TransformationMatrix * br + particle.Location);
				
                var texture = Sprite.SubImage(_frame);
                texCoords[index] = texture.TopLeft;
                texCoords[index + 1] = texCoords[index + 3] = texture.TopRight;
                texCoords[index + 2] = texCoords[index + 4] = texture.BottomLeft;
                texCoords[index + 5] = texture.BottomRight;
			});

			
			_renderSystem.SetVertices(vertices);
			_renderSystem.SetColor(Colors.White);
			_renderSystem.SetTexCoords(texCoords);
            
            _renderSystem.Render(Sprite.SubImage(_frame).Texture, PrimitiveType.Triangles);

			_frame += -_animationSpeed;
		}
	}
}
