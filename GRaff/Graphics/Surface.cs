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
		private static readonly Color[] white4 = { Color.White, Color.White, Color.White, Color.White };
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

		private void RenderTextured(Texture texture, PointF[] vertices, Color[] colors, PrimitiveType type)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindTexture(TextureTarget.Texture2D, texture.Id);

			ShaderProgram.CurrentTextured.SetCurrent();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), texture.GetTexCoords(), BufferUsageHint.StreamDraw);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertices.Length);
		}

		public void Clear(Color color)
		{
			GL.ClearColor(color.ToOpenGLColor());
		}

		public Color GetPixel(double x, double y)
		{
			Color c = 0;
			GL.ReadPixels((int)x, (int)(Room.Height - y), 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref c);
			return c;
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
			Render(new[] { new PointF(x, y), new PointF(x + w, y), new PointF(x + w, y + h), new PointF(x, y + h) },
				   new[] { col1, col2, col3, col4 }, PrimitiveType.LineLoop);
		}

		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, float x, float y, float w, float h)
		{
			Render(new[] { new PointF(x, y), new PointF(x + w, y), new PointF(x, y + h), new PointF(x + w, y + h) },
				   new[] { col1, col2, col4, col3 }, PrimitiveType.TriangleStrip);
		}

#warning Too unoptimized. Maybe just drop it?
		public void DrawCircle(Color color, PointF center, double radius)
		{
			int precision = (int)(GMath.Tau * radius);
			Render(Polygon.Circle(center, radius).Vertices.Select(v => (PointF)v).ToArray(), Enumerable.Repeat(color, precision).ToArray(), PrimitiveType.LineLoop);
		}

		public void FillCircle(Color col1, Color col2, PointF center, double radius)
		{
			int precision = (int)GMath.Ceiling(GMath.Tau * radius);
			PointF[] vertices = new PointF[precision + 2];
			int i = 0;
			vertices[i++] = center;
			foreach (PointF p in Polygon.Circle(center, radius).Vertices)
				vertices[i++] = p;
			vertices[i] = new PointF(center.X + (float)radius, center.Y);

			Color[] colors = new Color[precision + 2];
			colors[0] = col1;
			for (int j = 1; j < colors.Length; j++)
				colors[j] = col2;

			Render(vertices, colors, PrimitiveType.TriangleFan);
		}

		public void DrawPolygon(Color color, Polygon polygon)
		{
			Render(polygon.Vertices.Select(v => (PointF)v).ToArray(), Enumerable.Repeat(color, polygon.Length).ToArray(), PrimitiveType.LineLoop);
		}

		public void DrawSprite(Sprite sprite, int subimage, float x, float y)
		{
			float left = (float)(x - sprite.XOrigin), top = (float)(y - sprite.YOrigin), right = left + sprite.Width, bottom = top + sprite.Height;
			RenderTextured(sprite.SubImage(subimage), new[] { new PointF(left, top), new PointF(right, top), new PointF(left, bottom), new PointF(right, bottom) },
				   new[] { Color.White, Color.White, Color.White, Color.White }, PrimitiveType.TriangleStrip);
		}

		public void DrawSprite(Sprite sprite, int subimage, Color blend, AffineMatrix transform)
		{
			PointF[] vertices = {
				transform * new PointF(-sprite.XOrigin, -sprite.YOrigin),
				transform * new PointF(sprite.XOrigin, -sprite.YOrigin),
				transform * new PointF(-sprite.XOrigin, sprite.YOrigin),
				transform * new PointF(sprite.XOrigin, sprite.YOrigin)
			};

			RenderTextured(sprite.SubImage(subimage), vertices, new[] { blend, blend, blend, blend }, PrimitiveType.TriangleStrip);
		}

		public void DrawImage(Image image)
		{
			AffineMatrix t = image.Transform.GetMatrix();
			RenderTextured(image.CurrentTexture, new[] {
					t * new PointF(-image.Sprite.XOrigin, -image.Sprite.YOrigin),
					t * new PointF(image.Sprite.Width - image.Sprite.XOrigin, -image.Sprite.YOrigin),
					t * new PointF(-image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
					t * new PointF(image.Sprite.Width - image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
			}, new[] { image.Blend, image.Blend, image.Blend, image.Blend }, PrimitiveType.TriangleStrip);

		}

		public void DrawText(Font font, FontAlignment alignment, Color color, string text, PointF location)
		{
			PointF[] vertices;
			PointF[] texCoords;

			font.Render(text, alignment, out vertices, out texCoords);

			for (int i = 0; i < vertices.Length; i++)
				vertices[i] += location;
			
			Color[] colors = Enumerable.Repeat(color, vertices.Length).ToArray();

			GL.BindVertexArray(_vertexArray);
			GL.BindTexture(TextureTarget.Texture2D, font.TextureBuffer.Id);

			ShaderProgram.CurrentTextured.SetCurrent();

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), texCoords, BufferUsageHint.StreamDraw);

			GL.DrawArrays(GLPrimitiveType.Quads, 0, vertices.Length);
		}

		public void DrawText(Font font, FontAlignment alignment, Color color, string text, Transform transform)
		{
			PointF[] vertices;
			PointF[] texCoords;

			font.Render(text, alignment, out vertices, out texCoords);

			var matrix = transform.GetMatrix();
			Parallel.For(0, vertices.Length, i => vertices[i] = matrix * vertices[i]);

			Color[] colors = Enumerable.Repeat(color, vertices.Length).ToArray();

			GL.BindVertexArray(_vertexArray);
			GL.BindTexture(TextureTarget.Texture2D, font.TextureBuffer.Id);

			ShaderProgram.CurrentTextured.SetCurrent();

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), texCoords, BufferUsageHint.StreamDraw);

			GL.DrawArrays(GLPrimitiveType.Quads, 0, vertices.Length);


		}
	}
}
