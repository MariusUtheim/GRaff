using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GameMaker
{
	public class Surface
	{
		private int _id;
		private readonly OpenTK.Graphics.Color4 GLWhite = Color.White.ToOpenGLColor();

		public Surface(int width, int height)
		{
			//_id = GL.GenBuffer();
		}

		public Color GetPixel(double x, double y)
		{
			Color c = Color.Black;
			GL.ReadPixels<Color>((int)x, (int)y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedInt, ref c);
			return c;
		}

		public void SetPixel(Color color, double x, double y)
		{
			GL.Begin(PrimitiveType.Points);
			GL.Color4(color.ToOpenGLColor());
			GL.Vertex2(x, y);
			GL.End();
		}

		public void DrawSprite(double x, double y, Sprite sprite, int subimage)
		{
			double w = sprite.Width, h = sprite.Height;
			x -= sprite.XOrigin;
			y -= sprite.YOrigin;
			subimage %= sprite.ImageCount;
			double u1 = subimage / (double)sprite.ImageCount, u2 = (subimage + 1) / (double)sprite.ImageCount;
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, sprite.Texture.Id);

			GL.Begin(PrimitiveType.Quads);
			GL.Color4(GLWhite);
			{
				GL.TexCoord2(u1, 0);
				GL.Vertex2(x, y);
				GL.TexCoord2(u2, 0);
				GL.Vertex2(x + w, y);
				GL.TexCoord2(u2, 1);
				GL.Vertex2(x + w, y + h);
				GL.TexCoord2(u1, 1);
				GL.Vertex2(x, y + h);
			}
			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}

#warning TODO: Draw blended image
		public void DrawImage(Image image)
		{
			Point p1 = image.Transform.Point(-image.Sprite.XOrigin, -image.Sprite.YOrigin),
				  p2 = image.Transform.Point(image.Sprite.Width - image.Sprite.XOrigin, -image.Sprite.YOrigin),
				  p3 = image.Transform.Point(image.Sprite.Width - image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
				  p4 = image.Transform.Point(-image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin);

			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, image.CurrentTexture.Id);

			double u1 = image.Index / (double)image.Count, u2 = (image.Index + 1) / (double)image.Count;

			GL.Begin(PrimitiveType.Quads);
			GL.Color4(image.Blend.R, image.Blend.G, image.Blend.B, image.Blend.A);
			{
				GL.TexCoord2(u1, 0);
				GL.Vertex2(p1.X, p1.Y);
				GL.TexCoord2(u2, 0);
				GL.Vertex2(p2.X, p2.Y);
				GL.TexCoord2(u2, 1);
				GL.Vertex2(p3.X, p3.Y);
				GL.TexCoord2(u1, 1);
				GL.Vertex2(p4.X, p4.Y);
			}
			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}

		public void DrawCircle(Color color, double x, double y, double radius)
		{
			double dt = 2 * GMath.Tau / radius;

#warning TODO: Optimize
			GL.Begin(PrimitiveType.LineLoop);
			GL.Color4(color.ToOpenGLColor());
			for (double t = 0; t < GMath.Tau; t += dt)
				GL.Vertex2(x + radius * GMath.Cos(t), y + radius * GMath.Sin(t));
			GL.End();
		}

		public void FillCircle(Color color, double x, double y, double radius)
		{
			double c = GMath.Tau * radius;
			double dt = 1 / GMath.Tau;

#warning TODO: Optimize
			GL.Begin(PrimitiveType.TriangleFan);
			GL.Color4(color.ToOpenGLColor());
			for (double t = 0; t < GMath.Tau; t += dt)
				GL.Vertex2(x + radius * GMath.Cos(t), y + radius * GMath.Sin(t));

			GL.End();
		}

		public void FillCircle(Color col1, Color col2, double x, double y, double radius)
		{
			double dt = 2 * GMath.Tau / radius;

#warning TODO: Optimize
			GL.Begin(PrimitiveType.TriangleFan);
			GL.Color4(col1.ToOpenGLColor());
			GL.Vertex2(x, y);
			GL.Color4(col2.ToOpenGLColor());
			for (double t = 0; t <= GMath.Tau; t += dt)
				GL.Vertex2(x + radius * GMath.Cos(t), y - radius * GMath.Sin(t));
			GL.End();
		}

		public void FillCircle(Color col1, Color col2, double x, double y, double radius, double cx, double cy)
		{
			double dt = 2 * GMath.Tau / radius;

#warning TODO: Optimize
			GL.Begin(PrimitiveType.TriangleFan);
			GL.Color4(col1.ToOpenGLColor());
			GL.Vertex2(cx, cy);
			GL.Color4(col2.ToOpenGLColor());
			for (double t = 0; t <= GMath.Tau; t += dt)
				GL.Vertex2(x + radius * GMath.Cos(t), y - radius * GMath.Sin(t));
			GL.End();
			
		}

		public void DrawRectangle(Color color, double x, double y, double width, double height)
		{
			GL.Begin(PrimitiveType.LineLoop);
			GL.Color4(color.ToOpenGLColor());
			GL.Vertex2(x, y);
			GL.Vertex2(x + width, y);
			GL.Vertex2(x + width, y + height);
			GL.Vertex2(x, y + height);
			GL.End();
		}

		public void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
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
		}

		public void FillRectangle(Color color, double x, double y, double width, double height)
		{
			GL.Color4(color.ToOpenGLColor());
			GL.Begin(PrimitiveType.Quads);

			GL.Vertex2(x, y);
			GL.Vertex2(x + width, y);
			GL.Vertex2(x + width, y + height);
			GL.Vertex2(x, y + height);

			GL.End();
		}

		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
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
		}

		public void DrawLine(Color color, double x1, double y1, double x2, double y2)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Color4(color.ToOpenGLColor());
			GL.Vertex2(x1, y1);
			GL.Vertex2(x2, y2);
			GL.End();
		}

		public void DrawLine(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Color4(col1.ToOpenGLColor());
			GL.Vertex2(x1, y1);
			GL.Color4(col2.ToOpenGLColor());
			GL.Vertex2(x2, y2);
			GL.End();
		}

		public void DrawText(Color color, Object text, double x, double y)
		{
			throw new NotImplementedException();
		}

		public void Clear(Color color)
		{
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

		internal void DrawSprite(Transform transform, Color blend, Sprite sprite, int imageIndex)
		{
			Point p1 = transform.Point(-sprite.XOrigin, -sprite.YOrigin),
				  p2 = transform.Point(sprite.Width - sprite.XOrigin, -sprite.YOrigin),
				  p3 = transform.Point(sprite.Width - sprite.XOrigin, sprite.Height - sprite.YOrigin),
				  p4 = transform.Point(-sprite.XOrigin, sprite.Height - sprite.YOrigin);

			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, sprite.Texture.Id);

			double u1 = imageIndex / (double)sprite.ImageCount, u2 = (imageIndex + 1) / (double)sprite.ImageCount;

			GL.Begin(PrimitiveType.Quads);
			GL.Color4(blend.R, blend.G, blend.B, blend.A);
			{
				GL.TexCoord2(u1, 0);
				GL.Vertex2(p1.X, p1.Y);
				GL.TexCoord2(u2, 0);
				GL.Vertex2(p2.X, p2.Y);
				GL.TexCoord2(u2, 1);
				GL.Vertex2(p3.X, p3.Y);
				GL.TexCoord2(u1, 1);
				GL.Vertex2(p4.X, p4.Y);
			}
			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}
	}
}
