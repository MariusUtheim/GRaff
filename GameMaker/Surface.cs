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
		private List<float> _vertices = new List<float>();
		private List<float> _colors = new List<float>();
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
			_vertices.Clear();
			_colors.Clear();
		}

		public void Refresh()
		{
			GL.BindVertexArray(_array);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertices.Count * sizeof(float)), _vertices.ToArray(), BufferUsageHint.DynamicDraw);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_colors.Count * sizeof(float)), _colors.ToArray(), BufferUsageHint.DynamicDraw);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

			GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Count / 2);
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
			throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");

/*			if (polygon == null) return;
			GL.Begin(PrimitiveType.LineLoop);
			GL.Color4(color.ToOpenGLColor());
			foreach (Point p in polygon.Vertices)
				GL.Vertex2(p.X, p.Y);
	
			GL.End();
			*/
		}

		public void FillCircle(Color col1, Color col2, double x, double y, double radius)
		{
			float fx = (float)x, fy = (float)y;
			float r1 = col1.R / 255.0f, g1 = col1.G / 255.0f, b1 = col1.B / 255.0f, a1 = col1.A / 255.0f;
			float r2 = col2.R / 255.0f, g2 = col2.G / 255.0f, b2 = col2.B / 255.0f, a2 = col2.A / 255.0f;

			Point[] pts = Polygon.EnumerateCircle(new Point(x, y), radius).ToArray();
			for (int i = 0; i < pts.Length - 1; i++)
			{
				_vertices.AddRange(new[] {
						fx, fy,
						(float)pts[i].X, (float)pts[i].Y,
						(float)pts[i + 1].X, (float)pts[i + 1].Y
				});
				_colors.AddRange(new[] {
						r1, g1, b1, a1,
						r2, g2, b2, a2,
						r2, g2, b2, a2
				});
			}

			_vertices.AddRange(new[] {
						fx, fy,
						(float)pts[pts.Length - 1].X, (float)pts[pts.Length - 1].Y,
						(float)pts[0].X, (float)pts[0].Y
				});
			_colors.AddRange(new[] {
						r1, g1, b1, a1,
						r2, g2, b2, a2,
						r2, g2, b2, a2
				});


			/*
			GL.Begin(PrimitiveType.TriangleFan);
			GL.Color4(col1.ToOpenGLColor());
			GL.Vertex2(x, y);
			GL.Color4(col2.ToOpenGLColor());
			foreach (Point pt in Polygon.EnumerateCircle(new Point(x, y), radius))
				GL.Vertex2(pt.X, pt.Y);
			GL.Vertex2(x + radius, y);
			GL.End();
			*/

		}

		public void DrawRectangle(Color color, double x, double y, double width, double height)
		{
			throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");

			/*
			GL.Begin(PrimitiveType.LineLoop);
			GL.Color4(color.ToOpenGLColor());
			GL.Vertex2(x, y);
			GL.Vertex2(x + width, y);
			GL.Vertex2(x + width, y + height);
			GL.Vertex2(x, y + height);
			GL.End();
			*/
		}

		public void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			/*
			GL.Begin(PrimitiveType.Quads);

			GL.Color4(col1.ToOpenGLColor());
			GL.Vertex2(x, y);

			GL.Color4(col2.ToOpenGLColor());
			GL.Vertex2(x + width, y);

			GL.Color4(col3.ToOpenGLColor());
			GL.Vertex2(x + width, y + height);

			GL.Color4(col4.ToOpenGLColor());
			GL.Vertex2(x, y + height);

			GL.End();
			*/
			throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");

		}
#endregion



		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			float left = (float)x, top = (float)y, right = (float)(x + width), bottom = (float)(y + height);

			_vertices.AddRange(new[] {
					left, top,
					right, top,
					left, bottom,
					left, bottom,
					right, top,
					right, bottom,
			});

			_colors.AddRange(new[] {
					col1.R / 255.0f, col1.G / 255.0f, col1.B / 255.0f, col1.A / 255.0f,
					col2.R / 255.0f, col2.G / 255.0f, col2.B / 255.0f, col2.A / 255.0f,
					col4.R / 255.0f, col4.G / 255.0f, col4.B / 255.0f, col4.A / 255.0f,
					col4.R / 255.0f, col4.G / 255.0f, col4.B / 255.0f, col4.A / 255.0f,
					col2.R / 255.0f, col2.G / 255.0f, col2.B / 255.0f, col2.A / 255.0f,
					col3.R / 255.0f, col3.G / 255.0f, col3.B / 255.0f, col3.A / 255.0f,
			});
		}

		public void DrawLine(Color color, double x1, double y1, double x2, double y2)
		{
			throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");

			/*
			GL.Begin(PrimitiveType.Lines);
			GL.Color4(color.ToOpenGLColor());
			GL.Vertex2(x1, y1);
			GL.Vertex2(x2, y2);
			GL.End();
			*/
		}

		public void DrawLine(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			/*
			GL.Begin(PrimitiveType.Lines);
			GL.Color4(col1.ToOpenGLColor());
			GL.Vertex2(x1, y1);
			GL.Color4(col2.ToOpenGLColor());
			GL.Vertex2(x2, y2);
			GL.End();
			*/
			throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");

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
