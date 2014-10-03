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
		private List<double> _vertices = new List<double>();
		private List<byte> _colors = new List<byte>();
		private int _array;
		private int _vertexBuffer, _colorBuffer;

		public Surface(int width, int height)
		{
			_array = GL.GenVertexArray();
			_vertexBuffer = GL.GenBuffer();
			_colorBuffer = GL.GenBuffer();
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
#warning OPTIMIZE: Move stuff to initialization
#warning OPTIMIZE: Use sequential layout on points and colors to reduce conversions
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertices.Count * sizeof(double)), _vertices.ToArray(), BufferUsageHint.DynamicDraw);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Double, false, 0, 0);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_colors.Count * sizeof(byte)), _colors.ToArray(), BufferUsageHint.DynamicDraw);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, false, 0, 0);

			int offset = 0, count = 0;
			while (offset < _vertices.Count / 2)
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

		public void DrawCircle(Color color, double x, double y, double radius)
		{
			throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");
		}

		public void DrawPolygon(Color color, Polygon polygon)
		{
			_counts.Enqueue(polygon.Length);
			_primitives.Enqueue(PrimitiveType.LineLoop);
			throw new NotImplementedException();

/*			if (polygon == null) return;
			GL.Begin(PrimitiveType.LineLoop);
			GL.Color4(color.ToOpenGLColor());
			foreach (Point p in polygon.Vertices)
				GL.Vertex2(p.X, p.Y);
	
			GL.End();
			*/
		}

		#endregion

		public void DrawLine(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			_counts.Enqueue(2);
			_primitives.Enqueue(PrimitiveType.Lines);
			_vertices.AddRange(new[] {
					x1, y1,
					x2, y2
			});
			_colors.AddRange(new[] {
					col1.R, col1.G, col1.B, col1.A,
					col2.R, col2.G, col2.B, col2.A,
			});
		}

		public void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{

			_counts.Enqueue(4);
			_primitives.Enqueue(PrimitiveType.LineLoop);

			_vertices.AddRange(new[] {
					x, y,
					x + width, y,
					x + width, y + height,
					x, y + height
			});

			_colors.AddRange(new[] {
					col1.R, col1.G, col1.B, col1.A,
					col2.R, col2.G, col2.B, col2.A,
					col3.R, col3.G, col3.B, col3.A,
					col4.R, col4.G, col4.B, col4.A,
			});

		}



		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			double left = x, top = y, right = x + width, bottom = y + height;

			_vertices.AddRange(new[] {
					left, top,
					right, top,
					right, bottom,
					left, bottom
			});

			_colors.AddRange(new[] {
					col1.R, col1.G, col1.B, col1.A,
					col2.R, col2.G, col2.B, col2.A,
					col3.R, col3.G, col3.B, col3.A,
					col4.R, col4.G, col4.B, col4.A,
			});

			_counts.Enqueue(4);
			_primitives.Enqueue(PrimitiveType.Quads);
		}

		public void FillCircle(Color col1, Color col2, double x, double y, double radius)
		{
			byte r1 = col1.R, g1 = col1.G, b1 = col1.B, a1 = col1.A;
			byte r2 = col2.R, g2 = col2.G, b2 = col2.B, a2 = col2.A;


			Point[] pts = Polygon.EnumerateCircle(new Point(x, y), radius).ToArray();
			_primitives.Enqueue(PrimitiveType.TriangleFan);
			_counts.Enqueue(pts.Length + 2);

			_vertices.AddRange(new[] { x, y });
			_colors.AddRange(new[] { col1.R, col1.G, col1.B, col1.A });

			for (int i = 0; i < pts.Length; i++)
			{
				_vertices.AddRange(new[] { pts[i].X, pts[i].Y });
				_colors.AddRange(new[] {
						r2, g2, b2, a2
				});
			}

			_vertices.AddRange(new[] { x + radius, y });
			_colors.AddRange(new[] { r2, g2, b2, a2 });
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
