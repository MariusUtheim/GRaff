using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace GameMaker
{
	public class Surface
	{
		private Queue<PrimitiveType> _primitives = new Queue<PrimitiveType>();
		private Queue<int> _counts = new Queue<int>();
		private List<Point> _vertices = new List<Point>();
		private List<Color> _colors = new List<Color>();
		private int _array;
		private int _vertexBuffer, _colorBuffer;

		public Surface(int width, int height)
		{
			_array = GL.GenVertexArray();
			GL.BindVertexArray(_array);

			_vertexBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Double, false, 0, 0);

			_colorBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, false, 0, 0);

		}

		public void Clear()
		{
			_primitives.Clear();
			_counts.Clear();
			_vertices.Clear();
			_colors.Clear();
		}

		public void Refresh()
		{
			GL.BindVertexArray(_array);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertices.Count * 2 * sizeof(double)), _vertices.ToArray(), BufferUsageHint.DynamicDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_colors.Count * 4 * sizeof(byte)), _colors.ToArray(), BufferUsageHint.DynamicDraw);

			int offset = 0, count = 0;
			while (offset < _vertices.Count)
			{
				count = _counts.Dequeue();
				GL.DrawArrays(_primitives.Dequeue(), offset, count);
				offset += count;
			}
		}

#region 
		public Color GetPixel(double x, double y)
		{
			Color c = Color.Black;
			GL.ReadPixels<Color>((int)x, (int)y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedInt, ref c);
			return c;
		}

		public void SetPixel(Color color, double x, double y)
		{
			throw new NotImplementedException("GameMaker.Surface.SetPixel(Color, double, double) is not implemented");
		}

		public void DrawSprite(double x, double y, Sprite sprite, int subimage)
		{
			if (sprite == null) return;

			double w = sprite.Width, h = sprite.Height;
			x -= sprite.XOrigin;
			y -= sprite.YOrigin;
			subimage %= sprite.ImageCount;
			double u1 = subimage / (double)sprite.ImageCount, u2 = (subimage + 1.0) / (double)sprite.ImageCount;
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, sprite.Texture.Id);

			throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");

			GL.Disable(EnableCap.Texture2D);
		}

		public void DrawImage(Image image)
		{
			if (image == null) return;

			Point p1 = image.Transform.Point(-image.Sprite.XOrigin, -image.Sprite.YOrigin),
				  p2 = image.Transform.Point(image.Sprite.Width - image.Sprite.XOrigin, -image.Sprite.YOrigin),
				  p3 = image.Transform.Point(image.Sprite.Width - image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
				  p4 = image.Transform.Point(-image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin);

			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, image.CurrentTexture.Id);

			double u1 = image.Index / (double)image.Count, u2 = (image.Index + 1) / (double)image.Count;

			throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");

			GL.Disable(EnableCap.Texture2D);
		}


		#endregion

		public void DrawLine(Color col1, Color col2, Point p1, Point p2)
		{
			_counts.Enqueue(2);
			_primitives.Enqueue(PrimitiveType.Lines);
			_vertices.AddRange(new[] { p1, p2 });
			_colors.AddRange(new[] { col1, col2 });
		}

		public void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			_counts.Enqueue(4);
			_primitives.Enqueue(PrimitiveType.LineLoop);

			_vertices.AddRange(new[] {
					new Point(x, y),
					new Point(x + width, y),
					new Point(x + width, y + height),
					new Point(x, y + height)
			});

			_colors.AddRange(new[] {
					col1, col2, col3, col4
			});

		}

		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			double left = x, top = y, right = x + width, bottom = y + height;

			_vertices.AddRange(new[] {
					new Point(left, top),
					new Point(right, top),
					new Point(right, bottom),
					new Point(left, bottom)
			});

			_colors.AddRange(new[] {
					col1, col2, col3, col4
			});

			_counts.Enqueue(4);
			_primitives.Enqueue(PrimitiveType.Quads);
		}

		public void DrawCircle(Color color, Point center, double radius)
		{
			int precision = (int)radius;
			_primitives.Enqueue(PrimitiveType.LineLoop);
			_counts.Enqueue(precision);
			_vertices.AddRange(Polygon.EnumerateCircle(center, radius, precision));
			_colors.AddRange(Enumerable.Repeat(color, precision));
		}

		public void FillCircle(Color col1, Color col2, Point center, double radius)
		{
			int precision = (int)radius;
			_primitives.Enqueue(PrimitiveType.TriangleFan);
			_counts.Enqueue(precision + 2);

			_vertices.Add(center);
			_colors.Add(col1);
			_vertices.AddRange(Polygon.EnumerateCircle(center, radius));
			_vertices.Add(new Point(center.X + radius, center.Y));
			_colors.AddRange(Enumerable.Repeat(col2, precision + 1));
		}

		public void DrawPolygon(Color color, Polygon polygon)
		{
			_counts.Enqueue(polygon.Length);
			_primitives.Enqueue(PrimitiveType.LineLoop);
			_vertices.AddRange(polygon.Vertices);
			_colors.AddRange(Enumerable.Repeat(color, polygon.Length));
		}


		public void DrawText(Color color, Object text, double x, double y)
		{
			throw new NotImplementedException();
		}

		public void Clear(Color color)
		{
			Clear();
			GL.ClearColor(color.ToOpenGLColor());
		}

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
	}
}
