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

		public void SetPixel(double x, double y, Color color)
		{
			throw new NotImplementedException();
		}

		public void DrawSprite(double x, double y, Sprite sprite, int subimage)
		{
			double w = sprite.Width, h = sprite.Height;
			x -= sprite.XOrigin;
			y -= sprite.YOrigin;
			subimage %= sprite.ImageCount;
			double u1 = subimage / (double)sprite.ImageCount, u2 = (subimage + 1) / (double)sprite.ImageCount;
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, sprite.GetTexture(subimage).Id);

			GL.Begin(PrimitiveType.Quads);
			GL.Color4(Color.White.ToOpenGLColor());
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

		public void DrawImage(double x, double y, Image image)
		{
			double w = image.Sprite.Width, h = image.Sprite.Height;
			double u1 = image.Index / (double)image.Count, u2 = (image.Index + 1) / (double)image.Count;

			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, image.CurrentTexture.Id);

			GL.Translate(x, y, 0);
			GL.Rotate(image.Transform.Rotation.Degrees, 0, 0, 1);
			GL.Scale(image.Transform.XScale, image.Transform.YScale, 1.0);
			GL.Translate(-image.Sprite.XOrigin, -image.Sprite.YOrigin, 0);
		
			GL.Begin(PrimitiveType.Quads);
			GL.Color4(Color.White.ToOpenGLColor());
			{
				GL.TexCoord2(u1, 0);
				GL.Vertex2(0, 0);
				GL.TexCoord2(u2, 0);
				GL.Vertex2(w, 0);
				GL.TexCoord2(u2, 1);
				GL.Vertex2(w, h);
				GL.TexCoord2(u1, 1);
				GL.Vertex2(0, h);
			}
			GL.End();

			GL.Disable(EnableCap.Texture2D);
			//GL.Translate(x, y, 0);
			GL.LoadIdentity();
		}

		public void DrawCircle(Color color, double x, double y, double radius)
		{
			double c = GMath.Tau * radius;
			double dt = 2 / c;

#warning TODO: Optimize
			GL.Begin(PrimitiveType.LineLoop);
			for (double t = 0; t < GMath.Tau; t += dt)
				GL.Vertex2(x + radius * GMath.Cos(t), y + radius * GMath.Sin(t));
			GL.End();
		}

		public void FillCircle(Color color, Point location, double radius)
		{
			throw new NotImplementedException();
		}

		public void DrawRectangle(Color color, double x, double y, double width, double height)
		{
			GL.Begin(PrimitiveType.LineLoop);
			GL.Color4(color.ToOpenGLColor());
			GL.Vertex2((int)x, (int)y);
			GL.Vertex2((int)(x + width), (int)y);
			GL.Vertex2((int)(x + width), (int)(y + height));
			GL.Vertex2((int)x, (int)(y + height));
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
	}
}
