using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.Particles
{
	internal class ColoredParticleRenderer : IParticleRenderer
	{
		private readonly GraphicsPoint[] _polygonVertices;
		private readonly int _verticesPerParticle;
		private readonly SerialRenderSystem _renderSystem = new SerialRenderSystem();

		public ColoredParticleRenderer(Polygon polygon)
		{
            if (polygon.Length < 3) throw new ArgumentException("The polygon must have at least three vertices.", nameof(polygon));
			_verticesPerParticle = 3 + (polygon.Length - 3) * 3;	// First three vertices contribute one triangle; each remaining vertex contributes one triangle
			_polygonVertices = new GraphicsPoint[_verticesPerParticle];
			_polygonVertices[0] = (GraphicsPoint)polygon.Vertex(0);
			_polygonVertices[1] = (GraphicsPoint)polygon.Vertex(1);
			_polygonVertices[2] = (GraphicsPoint)polygon.Vertex(2);
			for (int i = 3, c = 3; i < polygon.Length; i++)
			{
				_polygonVertices[c++] = _polygonVertices[0];
				_polygonVertices[c++] = (GraphicsPoint)polygon.Vertex(i - 1);
				_polygonVertices[c++] = (GraphicsPoint)polygon.Vertex(i);
			}
		}

		public void Render(IEnumerable<Particle> particles)
		{
            if (particles == null || !particles.Any()) return;
			int count = particles.Count();
			var vertices = new GraphicsPoint[_polygonVertices.Length * count];
			var colors = new Color[_polygonVertices.Length * count];

			Parallel.ForEach(particles, (particle, loopState, index) =>
			{
				index *= _verticesPerParticle;
				for (int c = 0; c < _verticesPerParticle; c++)
				{
					vertices[index + c] = (GraphicsPoint)(particle.TransformationMatrix * _polygonVertices[c] + particle.Location);
					colors[index + c] = particle.Blend;
				}
			});

			_renderSystem.SetVertices(vertices);
			_renderSystem.SetColors(colors);
            _renderSystem.Render(PrimitiveType.Triangles);
		}
	}
}
