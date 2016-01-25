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
		PointF[] _polygonVertices;
		int _verticesPerParticle;
		ColoredRenderSystem _renderSystem;

		public ColoredParticleRenderer(Polygon polygon)
		{
			if (polygon.Length < 3) throw new ArgumentException("The polygon must have at least three vertices.", "polygon");
			_verticesPerParticle = 3 + (polygon.Length - 3) * 3;	// First three vertices contribute one triangle; each remaining vertex contributes one triangle
			_polygonVertices = new PointF[_verticesPerParticle];
			_polygonVertices[0] = (PointF)polygon.Vertex(0);
			_polygonVertices[1] = (PointF)polygon.Vertex(1);
			_polygonVertices[2] = (PointF)polygon.Vertex(2);
			for (int i = 3, c = 3; i < polygon.Length; i++)
			{
				_polygonVertices[c++] = _polygonVertices[0];
				_polygonVertices[c++] = (PointF)polygon.Vertex(i - 1);
				_polygonVertices[c++] = (PointF)polygon.Vertex(i);
			}

			_renderSystem = new ColoredRenderSystem();
		}

		public void Render(IEnumerable<Particle> particles)
		{
			int count = particles.Count();
			PointF[] vertices = new PointF[_polygonVertices.Length * count];
			Color[] colors = new Color[_polygonVertices.Length * count];

			Parallel.ForEach(particles, (particle, loopState, index) =>
			{
				index *= _verticesPerParticle;
				for (int c = 0; c < _verticesPerParticle; c++)
				{
					vertices[index + c] = (PointF)(particle.TransformationMatrix * _polygonVertices[c] + particle.Location);
					colors[index + c] = particle.Blend;
				}
			});

			_renderSystem.SetVertices(UsageHint.StreamDraw, vertices);
			_renderSystem.SetColors(UsageHint.StreamDraw, colors);

			ShaderProgram.CurrentColored.SetCurrent();
			_renderSystem.Render(PrimitiveType.Triangles, vertices.Length);

		}
	}
}
