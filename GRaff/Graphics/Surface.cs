using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;
using GLPrimitiveType = OpenTK.Graphics.ES30.PrimitiveType;

namespace GRaff.Graphics
{
	public sealed class Surface
	{
		private static readonly int sizeofPoint = Marshal.SizeOf(typeof(PointF)), sizeofColor = Marshal.SizeOf(typeof(Color));
		private static readonly Color[] white4 = { Colors.White, Colors.White, Colors.White, Colors.White };
		private static readonly PointF[] defaultTexCoords = { new PointF(0.0f, 0.0f), new PointF(1.0f, 0.0f), new PointF(0.0f, 1.0f), new PointF(1.0f, 1.0f)};
		private int _vertexArray;
		private int _vertexBuffer, _colorBuffer, _textureBuffer;

		public Surface(int width, int height)
		{
			GL.GenVertexArrays(1, out _vertexArray);
			GL.BindVertexArray(_vertexArray);

			GL.GenBuffers(1, out _vertexBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

			GL.GenBuffers(1, out _colorBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);
			
			GL.GenBuffers(1, out _textureBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(8 * sizeof(float)), defaultTexCoords, BufferUsageHint.StreamDraw);
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
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

		private void Render(PointF[] vertices, Color[] colors, PrimitiveType type)
		{
			ShaderProgram.CurrentColored.SetCurrent();
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertices.Length);
		}

		internal void RenderTextured(Texture texture, PointF[] vertices, Color[] colors, PrimitiveType type)
		{
			GL.BindVertexArray(_vertexArray);
			texture.Bind();

			ShaderProgram.CurrentTextured.SetCurrent();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), texture.TexCoords, BufferUsageHint.StreamDraw);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertices.Length);
		}

		internal void FillEllipse(object color1, object color2, PointF topLeft, double left, double top)
		{
			throw new NotImplementedException();
		}

		public void Clear(Color color)
		{
			GL.ClearColor(color.ToOpenGLColor());
		}

		internal void DrawPrimitive(PointF[] vertices, Color[] colors, PrimitiveType type)
		{
			Render(vertices, colors, type);
		}

		public Color GetPixel(int x, int y)
		{
			byte[] rgb = new byte[3];
			GL.ReadPixels(x, Room.Current.Height - y, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, rgb);
			return new Color(rgb[0], rgb[1], rgb[2]);
		}

		public void SetPixel(Color color, PointF location)
		{
			Render(new[] { location }, new[] { color }, PrimitiveType.Points);
		}

		public void DrawLine(Color col1, Color col2, PointF p1, PointF p2)
		{
			Render(new[] { p1, p2 }, new[] { col1, col2 }, PrimitiveType.Lines);
		}

		public void DrawTriangle(Color col1, Color col2, Color col3, PointF p1, PointF p2, PointF p3)
		{
			Render(new[] { p1, p2, p3 }, new[] { col1, col2, col3 }, PrimitiveType.LineStrip);
		}

		public void FillTriangle(Color col1, Color col2, Color col3, PointF p1, PointF p2, PointF p3)
		{
			Render(new[] { p1, p2, p3 }, new[] { col1, col2, col3 }, PrimitiveType.Triangles);
		}

		public void DrawTexture(Texture texture, PointF p)
		{
			float left = p.X, top = p.Y;
			float right = left + texture.PixelWidth, bottom = top + texture.PixelHeight;

			RenderTextured(texture, new[] { new PointF(left, top), new PointF(right, top), new PointF(left, bottom), new PointF(right, bottom) },
						   white4, PrimitiveType.TriangleStrip);
		}


		public void DrawRectangle(Color col1, Color col2, Color col3, Color col4, float x, float y, float w, float h)
		{
			Render(new[] { new PointF(x, y), new PointF(x + w, y), new PointF(x + w, y + h), new PointF(x, y + h), new PointF(x, y + h) },
				   new[] { col1, col2, col3, col4, col4 }, PrimitiveType.LineLoop);
		}

		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, float x, float y, float w, float h)
		{
			Render(new[] { new PointF(x, y), new PointF(x + w, y), new PointF(x, y + h), new PointF(x + w, y + h) },
				   new[] { col1, col2, col4, col3 }, PrimitiveType.TriangleStrip);
		}

		public void DrawCircle(Color color, PointF center, double radius)
		{
			var circle = Polygon.Circle(center, radius);
            Render(circle.Vertices.Select(v => (PointF)v).ToArray(), Enumerable.Repeat(color, circle.Length).ToArray(), PrimitiveType.LineLoop);
		}

		public void FillCircle(Color innerColor, Color outerColor, PointF center, double radius)
		{
			if (radius == 0)
			{
				SetPixel(innerColor, center);
				return;
			}
			int precision = (int)GMath.Ceiling(GMath.Tau * GMath.Abs(radius));
			var vertices = new PointF[precision + 2];
			int i = 0;
			vertices[i++] = center;
			foreach (var p in Polygon.Circle(center, radius).Vertices)
				vertices[i++] = (PointF)p;
			vertices[i] = new PointF(center.X, center.Y - (float)radius);

			Color[] colors = new Color[precision + 2];
			colors[0] = innerColor;
			for (int j = 1; j < colors.Length; j++)
				colors[j] = outerColor;

			Render(vertices, colors, PrimitiveType.TriangleFan);
		}

		public void DrawEllipse(Color color, PointF location, double width, double height)
		{
			var ellipse = Polygon.Ellipse(location + new Vector(width, height) / 2, width / 2, height / 2);
			Render(ellipse.Vertices.Select(v => (PointF)v).ToArray(), Enumerable.Repeat(color, ellipse.Length).ToArray(), PrimitiveType.LineLoop);
		}

		public void FillEllipse(Color innerColor, Color outerColor, PointF location, double width, double height)
		{
			var center = location + new PointF(width / 2, height / 2);
			if (width == 0 && height == 0)
			{
				SetPixel(innerColor, center);
				return;
			}
			var ellipse = Polygon.Ellipse(center, width / 2, height / 2);
			var vertices = new PointF[ellipse.Length + 2];
			int i = 0;
			vertices[i++] = center;
			foreach (var p in ellipse.Vertices)
				vertices[i++] = (PointF)p;
			vertices[i] = new PointF(location.X + (float)width, center.Y);

			var colors = new Color[ellipse.Length + 2];
			colors[0] = innerColor;
			for (int j = 1; j < colors.Length; j++)
				colors[j] = outerColor;

			Render(vertices, colors, PrimitiveType.TriangleFan);
		}

		public void DrawPolygon(Color color, Polygon polygon)
		{
			var vertices = polygon.Vertices.Select(v => (PointF)v).ToArray();
			if (polygon.Length == 1)
				Render(vertices, new[] { color }, PrimitiveType.Points);
			else if (polygon.Length == 2)
				Render(vertices, new[] { color, color }, PrimitiveType.Lines);
			else
				Render(vertices, Enumerable.Repeat(color, polygon.Length).ToArray(), PrimitiveType.LineLoop);
		}

		public void FillPolygon(Color color, Polygon polygon)
		{
			var vertices = polygon.Vertices.Select(v => (PointF)v).ToArray();
			if (polygon.Length == 1)
				Render(vertices, new[] { color }, PrimitiveType.Points);
			else if (polygon.Length == 2)
				Render(vertices, new[] { color, color }, PrimitiveType.Lines);
			else
				Render(vertices, Enumerable.Repeat(color, polygon.Length).ToArray(), PrimitiveType.TriangleFan);
		}

		public void DrawSprite(Sprite sprite, int subimage, float x, float y)
		{
			float left = (float)(x - sprite.XOrigin), top = (float)(y - sprite.YOrigin), right = left + (float)sprite.Width, bottom = top + (float)sprite.Height;
			RenderTextured(sprite.SubImage(subimage), new[] { new PointF(left, top), new PointF(right, top), new PointF(left, bottom), new PointF(right, bottom) },
				   new[] { Colors.White, Colors.White, Colors.White, Colors.White }, PrimitiveType.TriangleStrip);
		}

		public void DrawSprite(Sprite sprite, int subimage, Color blend, AffineMatrix transform)
		{
			PointF[] vertices = {
				transform * new PointF(-(float)sprite.XOrigin, -(float)sprite.YOrigin),
				transform * new PointF((float)sprite.XOrigin, -(float)sprite.YOrigin),
				transform * new PointF(-(float)sprite.XOrigin, (float)sprite.YOrigin),
				transform * new PointF((float)sprite.XOrigin, (float)sprite.YOrigin)
			};

			RenderTextured(sprite.SubImage(subimage), vertices, new[] { blend, blend, blend, blend }, PrimitiveType.TriangleStrip);
		}

		public void DrawImage(Image image)
		{
			AffineMatrix t = image.Transform.GetMatrix();
			RenderTextured(image.CurrentTexture, new[] {
					t * new PointF(-(float)image.Sprite.XOrigin, -(float)image.Sprite.YOrigin),
					t * new PointF( (float)(image.Sprite.Width - image.Sprite.XOrigin), -(float)image.Sprite.YOrigin),
					t * new PointF(-(float)image.Sprite.XOrigin, (float)(image.Sprite.Height - image.Sprite.YOrigin)),
					t * new PointF( (float)image.Sprite.Width -  (float)image.Sprite.XOrigin, (float)(image.Sprite.Height - image.Sprite.YOrigin)),
			}, new[] { image.Blend, image.Blend, image.Blend, image.Blend }, PrimitiveType.TriangleStrip);

		}

		public void DrawText(TextRenderer renderer, Color color, string text, AffineMatrix transform)
		{
			PointF[] vertices;
			PointF[] texCoords;

			var split = renderer.RenderCoords(text, out vertices);

			texCoords = new PointF[vertices.Length];
			var offset = 0;
			for (var l = 0; l < split.Length; l++)
			{
				renderer.Font.RenderTexCoords(split[l], offset, ref texCoords);
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
