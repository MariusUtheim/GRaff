using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using GRaff.Graphics.Text;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
using GLPrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;
#else
using OpenTK.Graphics.ES30;
using GLPrimitiveType = OpenTK.Graphics.ES30.PrimitiveType;
#endif

#warning TODO: Review class. In particular, optimize with graphics shader, optimize monocolored primitives
namespace GRaff.Graphics
{
	internal class RenderDevice : IRenderDevice
	{
		private readonly RenderSystem _renderSystem = new RenderSystem();

		public RenderDevice()
		{
            _renderSystem.QuadTexCoords(UsageHint.StaticDraw, 1);
		}

        //TODO// Copy to framebuffer

        
        public void Clear(Color color)
		{
			GL.ClearColor(color.ToOpenGLColor());
            GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void Draw(GraphicsPoint[] vertices, Color[] colors, PrimitiveType type)
		{
			Contract.Requires(vertices != null && colors != null);
			Contract.Requires(vertices.Length == colors.Length);
            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColors(colors);
            _renderSystem.Render(type);
		}

        public void Draw(GraphicsPoint[] vertices, Color color, PrimitiveType type)
        {
            Contract.Requires(vertices != null);
            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColor(color);
            _renderSystem.Render(type);
        }
        

        public void FillEllipse(Color color, GraphicsPoint location, double hRadius, double vRadius)
        {
            _renderSystem.SetVertices(Polygon.Ellipse(location, hRadius, vRadius).Tesselate());
            _renderSystem.SetColor(color);
            _renderSystem.Render(PrimitiveType.LineLoop);
        }
        
        public void FillEllipse(Color innerColor, Color outerColor, GraphicsPoint center, double hRadius, double vRadius)
        {
            if (hRadius == 0 && vRadius == 0)
            {
                Draw(new[] { center }, innerColor, PrimitiveType.Points);
                return;
            }
            var ellipse = Polygon.Ellipse(center, hRadius, vRadius);
            var vertices = new GraphicsPoint[ellipse.Length + 2];
            int i = 0;
            vertices[i++] = center;
            foreach (var p in ellipse.Vertices)
                vertices[i++] = (GraphicsPoint)p;
            vertices[vertices.Length - 1] = new GraphicsPoint(center.X + hRadius, center.Y);

            var colors = new Color[ellipse.Length + 2];
            colors[0] = innerColor;
            for (int j = 1; j < colors.Length; j++)
                colors[j] = outerColor;

            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColors(colors);
            _renderSystem.Render(PrimitiveType.TriangleFan);
        }


		public void DrawPolygon(Color color, Polygon polygon)
		{
			Contract.Requires<ArgumentNullException>(polygon != null);

            _renderSystem.SetVertices(polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray());
            _renderSystem.SetColor(color);

			if (polygon.Length == 1)
				_renderSystem.Render(PrimitiveType.Points);
			else if (polygon.Length == 2)
				_renderSystem.Render(PrimitiveType.Lines);
			else
				_renderSystem.Render(PrimitiveType.LineLoop);
		}

        public void FillPolygon(Color color, Polygon polygon)
        {
            Contract.Requires<ArgumentNullException>(polygon != null);

            _renderSystem.SetVertices(polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray());
            _renderSystem.SetColor(color);

            if (polygon.Length == 1)
                _renderSystem.Render(PrimitiveType.Points);
            else if (polygon.Length == 2)
                _renderSystem.Render(PrimitiveType.Lines);
            else
                _renderSystem.Render(PrimitiveType.TriangleFan);
        }


        public void DrawTexture(Texture texture, double xOrigin, double yOrigin, Color blend, Matrix transform)
        {
			Contract.Requires<ArgumentNullException>(texture != null && transform != null);

            _renderSystem.SetVertices(new[] {
                transform * new GraphicsPoint(-xOrigin, -yOrigin),
                transform * new GraphicsPoint(texture.Width - xOrigin, -yOrigin),
                transform * new GraphicsPoint(texture.Width - xOrigin, texture.Height - yOrigin),
                transform * new GraphicsPoint(-xOrigin, texture.Height - yOrigin),
            });
            _renderSystem.SetColor(blend);
            
            _renderSystem.SetTexCoords(UsageHint.StreamDraw, texture.QuadCoords);
            _renderSystem.Render(texture.Buffer, PrimitiveType.Quads);
        }

        public void DrawTexture(TextureBuffer buffer, GraphicsPoint[] vertices, Color blend, GraphicsPoint[] texCoords, PrimitiveType type)
        {
            Contract.Requires<ArgumentNullException>(buffer != null && vertices != null && texCoords != null);
            Contract.Requires<ObjectDisposedException>(!buffer.IsDisposed);

            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColor(blend);
            _renderSystem.SetTexCoords(texCoords);
            _renderSystem.Render(buffer, type);
        }


        public void DrawTexture(TextureBuffer buffer, GraphicsPoint[] vertices, Color[] colors, GraphicsPoint[] texCoords, PrimitiveType type)
        {
            Contract.Requires<ArgumentNullException>(buffer != null && vertices != null && texCoords != null);
            Contract.Requires<ObjectDisposedException>(!buffer.IsDisposed);

            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColors(colors);
            _renderSystem.SetTexCoords(texCoords);
            _renderSystem.Render(buffer, type);
        }
        public void DrawText(TextRenderer renderer, Color color, string text, Matrix transform)
		{
			Contract.Requires<ArgumentNullException>(renderer != null && transform != null);

			if (text == null)
				return;

            (var vertices, var texCoords) = renderer.RenderCoords(text);
            
			for (int i = 0; i < vertices.Length; i++)
				vertices[i] = transform * vertices[i];

            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColor(color);
            _renderSystem.SetTexCoords(texCoords);
            _renderSystem.Render(renderer.Font.Buffer, PrimitiveType.Quads);
		}
    }
}
