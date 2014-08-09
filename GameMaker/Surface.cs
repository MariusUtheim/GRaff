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

		public void DrawImage(double x, double y, Transform transform, Image image)
		{
			double w = image.Sprite.Width, h = image.Sprite.Height;
			x -= image.Sprite.XOrigin;
			y -= image.Sprite.YOrigin;
			float fx = (float)x, fy = (float)y;
			GL.BindTexture(TextureTarget.Texture2D, image.CurrentTexture.Id);
			GL.Enable(EnableCap.Texture2D);

#warning I want to reverse the clockwiseness
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(x, y);
			GL.TexCoord2(0, 1); GL.Vertex2(x + w, y);
			GL.TexCoord2(1, 1); GL.Vertex2(x + w, y + h);
			GL.TexCoord2(1, 0); GL.Vertex2(x, y + h);
			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}

		public void DrawTexture(double x, double y, Texture texture)
		{
			GL.BindTexture(TextureTarget.Texture2D, texture.Id);
			GL.Enable(EnableCap.Texture2D);

			GL.Begin(PrimitiveType.Quads);
			GL.Color4(Color.White.ToOpenGLColor());
			{
				GL.TexCoord2(0, 0);
				GL.Vertex2(x, y);
				GL.TexCoord2(1, 0);
				GL.Vertex2(x + texture.Width, y);
				GL.TexCoord2(1, 1);
				GL.Vertex2(x + texture.Width, y + texture.Height);
				GL.TexCoord2(0, 1);
				GL.Vertex2(x, y + texture.Height);
			}
			GL.End();
			GL.Disable(EnableCap.Texture2D);
		}

		public void DrawCircle(Color color, double x, double y, double radius)
		{
			GL.Begin(PrimitiveType.LineLoop);
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
