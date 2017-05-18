using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using GRaff.Graphics.Text;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
using GLPrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;
using coord = System.Double;
#else
using OpenTK.Graphics.ES30;
using GLPrimitiveType = OpenTK.Graphics.ES30.PrimitiveType;
using coord = System.Single;
#endif

namespace GRaff.Graphics
{
	public sealed class Surface
	{
		private static readonly int sizeofPoint = Marshal.SizeOf(typeof(GraphicsPoint)), sizeofColor = Marshal.SizeOf(typeof(Color));
		private static readonly Color[] white4 = { Colors.White, Colors.White, Colors.White, Colors.White };
		private static readonly GraphicsPoint[] defaultTexCoords = { new GraphicsPoint(0.0, 0.0), new GraphicsPoint(1.0, 0.0), new GraphicsPoint(0.0, 1.0), new GraphicsPoint(1.0, 1.0)};
		private readonly int _vertexArray;
		private readonly int _vertexBuffer, _colorBuffer, _textureBuffer;

		public Surface(int width, int height)
		{
			_vertexArray = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArray);

			_vertexBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, GraphicsPoint.PointerType, false, 0, 0);

			_colorBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

			_textureBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(4 * sizeofPoint), defaultTexCoords, BufferUsageHint.StreamDraw);
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, GraphicsPoint.PointerType, false, 0, 0);
		}



#region To be implemented


		public void Blit(Surface dest, IntRectangle srcRect, IntRectangle destRect)
		{
			throw new NotImplementedException();
		}

		public int Width
		{
			get { throw new NotImplementedException(); }
		}

		public int Height
		{
			get { throw new NotImplementedException(); }
		}

		public IntVector Size
		{
			get { return new IntVector(Width, Height); }
		}

#endregion

#region Drawing functions

		private void Render(GraphicsPoint[] vertices, Color[] colors, PrimitiveType type)
		{
			Contract.Requires(vertices != null && colors != null);
			Contract.Requires(vertices.Length == colors.Length);
			ShaderProgram.CurrentColored.SetCurrent();
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertices.Length);
		}

		internal void RenderTextured(Texture texture, GraphicsPoint[] vertices, Color[] colors)
		{
			Contract.Requires(texture != null && vertices != null && colors != null);
			Contract.Requires(vertices.Length == colors.Length);
			GL.BindVertexArray(_vertexArray);
			texture.Bind();

			ShaderProgram.CurrentTextured.SetCurrent();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), texture.QuadCoords, BufferUsageHint.StreamDraw);
			GL.DrawArrays(GLPrimitiveType.Quads, 0, vertices.Length);
		}

		public void Clear(Color color)
		{
			GL.ClearColor(color.ToOpenGLColor());
		}

		internal void DrawPrimitive(GraphicsPoint[] vertices, Color[] colors, PrimitiveType type)
		{
			Contract.Requires(vertices != null && colors != null);
			Contract.Requires(vertices.Length == colors.Length);
			Render(vertices, colors, type);
		}

		public Color GetPixel(int x, int y)
		{
			byte[] rgb = new byte[3];
			GL.ReadPixels(x, Room.Current.Height - y, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, rgb);
			return new Color(rgb[0], rgb[1], rgb[2]);
		}

		public void SetPixel(Color color, GraphicsPoint location)
		{
			Render(new[] { location }, new[] { color }, PrimitiveType.Points);
		}

		public void DrawLine(Color col1, Color col2, GraphicsPoint p1, GraphicsPoint p2)
		{
			Render(new[] { p1, p2 }, new[] { col1, col2 }, PrimitiveType.Lines);
		}

		public void DrawTriangle(Color col1, Color col2, Color col3, GraphicsPoint p1, GraphicsPoint p2, GraphicsPoint p3)
		{
			Render(new[] { p1, p2, p3, p1 }, new[] { col1, col2, col3, col1 }, PrimitiveType.LineStrip);
		}

		public void FillTriangle(Color col1, Color col2, Color col3, GraphicsPoint p1, GraphicsPoint p2, GraphicsPoint p3)
		{
			Render(new[] { p1, p2, p3 }, new[] { col1, col2, col3 }, PrimitiveType.Triangles);
		}

		public void DrawTexture(Texture texture, GraphicsPoint p)
		{
			Contract.Requires<ArgumentNullException>(texture != null);
			coord left = p.Xt, top = p.Yt;
			coord right = left + texture.PixelWidth, bottom = top + texture.PixelHeight;

			RenderTextured(texture, new[] { new GraphicsPoint(left, top), new GraphicsPoint(right, top), new GraphicsPoint(right, bottom), new GraphicsPoint(left, bottom) }, white4);
		}


		public void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double w, double h)
		{
			Render(new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x + w, y + h), new GraphicsPoint(x, y + h), new GraphicsPoint(x, y + h) },
				   new[] { col1, col2, col3, col4, col4 }, PrimitiveType.LineLoop);
		}

		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double w, double h)
		{
			Render(new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x, y + h), new GraphicsPoint(x + w, y + h) },
				   new[] { col1, col2, col4, col3 }, PrimitiveType.TriangleStrip);
		}

		public void DrawCircle(Color color, GraphicsPoint center, double radius)
		{
			var circle = Polygon.Circle(center, radius);
            Render(circle.Vertices.Select(v => (GraphicsPoint)v).ToArray(), Enumerable.Repeat(color, circle.Length).ToArray(), PrimitiveType.LineLoop);
		}

		public void FillCircle(Color innerColor, Color outerColor, GraphicsPoint center, double radius)
		{
			if (radius == 0)
			{
				SetPixel(innerColor, center);
				return;
			}
			int precision = (int)GMath.Ceiling(GMath.Tau * GMath.Abs(radius));
			var vertices = new GraphicsPoint[precision + 2];
			vertices[0] = center;
			int i = 1;
			foreach (var p in Polygon.Circle(center, radius).Vertices)
				vertices[i++] = (GraphicsPoint)p;
			vertices[vertices.Length - 1] = new GraphicsPoint(center.X, center.Y - radius);

			Color[] colors = new Color[precision + 2];
			colors[0] = innerColor;
			for (int j = 1; j < colors.Length; j++)
				colors[j] = outerColor;

			Render(vertices, colors, PrimitiveType.TriangleFan);
		}

		public void DrawEllipse(Color color, GraphicsPoint location, double width, double height)
		{
			var ellipse = Polygon.Ellipse(location + new Vector(width, height) / 2, width / 2, height / 2);
			Render(ellipse.Vertices.Select(v => (GraphicsPoint)v).ToArray(), Enumerable.Repeat(color, ellipse.Length).ToArray(), PrimitiveType.LineLoop);
		}

		public void FillEllipse(Color innerColor, Color outerColor, GraphicsPoint location, double width, double height)
		{
			var center = location + new GraphicsPoint(width / 2, height / 2);
			if (width == 0 && height == 0)
			{
				SetPixel(innerColor, center);
				return;
			}
			var ellipse = Polygon.Ellipse(center, width / 2, height / 2);
			var vertices = new GraphicsPoint[ellipse.Length + 2];
			int i = 0;
			vertices[i++] = center;
			foreach (var p in ellipse.Vertices)
				vertices[i++] = (GraphicsPoint)p;
			vertices[vertices.Length - 1] = new GraphicsPoint(location.X + width, center.Y);

			var colors = new Color[ellipse.Length + 2];
			colors[0] = innerColor;
			for (int j = 1; j < colors.Length; j++)
				colors[j] = outerColor;

			Render(vertices, colors, PrimitiveType.TriangleFan);
		}

		public void DrawPolygon(Color color, Polygon polygon)
		{
			Contract.Requires<ArgumentNullException>(polygon != null);
			var vertices = polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray();
			if (polygon.Length == 1)
				Render(vertices, new[] { color }, PrimitiveType.Points);
			else if (polygon.Length == 2)
				Render(vertices, new[] { color, color }, PrimitiveType.Lines);
			else
				Render(vertices, Enumerable.Repeat(color, polygon.Length).ToArray(), PrimitiveType.LineLoop);
		}

		public void FillPolygon(Color color, Polygon polygon)
		{
			Contract.Requires<ArgumentNullException>(polygon != null);
			var vertices = polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray();
			if (polygon.Length == 1)
				Render(vertices, new[] { color }, PrimitiveType.Points);
			else if (polygon.Length == 2)
				Render(vertices, new[] { color, color }, PrimitiveType.Lines);
			else
				Render(vertices, Enumerable.Repeat(color, polygon.Length).ToArray(), PrimitiveType.TriangleFan);
		}

		public void DrawSprite(Sprite sprite, int subimage, double x, double y)
		{
			Contract.Requires<ArgumentNullException>(sprite != null);
			coord left = (coord)(x - sprite.XOrigin), top = (coord)(y - sprite.YOrigin), right = left + (coord)sprite.Width, bottom = top + (coord)sprite.Height;
			RenderTextured(sprite.SubImage(subimage), new[] { new GraphicsPoint(left, top), new GraphicsPoint(right, top), new GraphicsPoint(right, bottom), new GraphicsPoint(left, bottom) }, white4);
		}

		public void DrawSprite(Sprite sprite, int subimage, Color blend, Matrix transform)
		{
			Contract.Requires<ArgumentNullException>(sprite != null && transform != null);
			var vertices = new[] {
				transform * new GraphicsPoint(-sprite.XOrigin, -sprite.YOrigin),
				transform * new GraphicsPoint(sprite.XOrigin, -sprite.YOrigin),
				transform * new GraphicsPoint(sprite.XOrigin, sprite.YOrigin),
				transform * new GraphicsPoint(-sprite.XOrigin, sprite.YOrigin),
			};

			RenderTextured(sprite.SubImage(subimage), vertices, new[] { blend, blend, blend, blend });
		}

		public void DrawImage(Image image)
		{
			Contract.Requires<ArgumentNullException>(image != null);
			Matrix t = image.Transform.GetMatrix();
			RenderTextured(image.CurrentTexture, new[] {
					t * new GraphicsPoint(-image.Sprite.XOrigin, -image.Sprite.YOrigin),
					t * new GraphicsPoint( image.Sprite.Width - image.Sprite.XOrigin, -image.Sprite.YOrigin),
					t * new GraphicsPoint( image.Sprite.Width -  image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
					t * new GraphicsPoint(-image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
			}, new[] { image.Blend, image.Blend, image.Blend, image.Blend });

		}

		public void DrawText(TextRenderer renderer, Color color, string text, Matrix transform)
		{
			Contract.Requires<ArgumentNullException>(renderer != null && transform != null);

			if (text == null)
				return;

			text = renderer.MultilineFormat(text);

			GraphicsPoint[] vertices;
			GraphicsPoint[] texCoords;

			var split = renderer.RenderCoords(text, out vertices);

			texCoords = new GraphicsPoint[vertices.Length];
			var offset = 0;
			for (var l = 0; l < split.Length; l++)
			{
				renderer.RenderTexCoords(split[l], offset, ref texCoords);
				offset += split[l].Length;
			}

			for (int i = 0; i < vertices.Length; i++)
				vertices[i] = transform * vertices[i];
			
			var colors = Enumerable.Repeat(color, vertices.Length).ToArray();

			GL.BindVertexArray(_vertexArray);
			GL.BindTexture(TextureTarget.Texture2D, renderer.Font.TextureBuffer.Id);

			ShaderProgram.CurrentTextured.SetCurrent();

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), texCoords, BufferUsageHint.StreamDraw);

			GL.DrawArrays(GLPrimitiveType.Quads, 0, vertices.Length);
		}



#endregion
	}
}
