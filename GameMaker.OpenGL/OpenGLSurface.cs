using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GameMaker.OpenGL
{
	public class OpenGLSurface : Surface
	{
		public OpenGLSurface(int width, int height)
		{
			ID = GL.GenBuffer();
		}

		public int ID { get; private set; }

		public override Color GetPixel(int x, int y)
		{
			Color c = Color.Black;
			GL.ReadPixels<Color>(x, y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedInt, ref c);
			return c;
		}

		public override void SetPixel(int x, int y, Color color)
		{
			throw new NotImplementedException();
		}

		public override void DrawImage(double x, double y, Image image)
		{
			double w = image.Sprite.Width, h = image.Sprite.Height;
			x -= image.XOrigin;
			y -= image.YOrigin;
			float fx = (float)x, fy = (float)y;
			GL.Color3(image.Blend.ToGLColor());
			GL.BindTexture(TextureTarget.Texture2D, (image.CurrentTexture as OpenGLTexture).Id);
			GL.Enable(EnableCap.Texture2D);

			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(x, y);
			GL.TexCoord2(0, 1); GL.Vertex2(x, y + h);
			GL.TexCoord2(1, 1); GL.Vertex2(x + w, y + h);
			GL.TexCoord2(1, 0); GL.Vertex2(x + w, y);
			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}

		public override void DrawTexture(double x, double y, Texture texture)
		{
			GL.BindTexture(TextureTarget.Texture2D, (texture as OpenGLTexture).Id);
			GL.Enable(EnableCap.Texture2D);

			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(x, y);
			GL.TexCoord2(0, 1); GL.Vertex2(x, y + texture.Height);
			GL.TexCoord2(1, 1); GL.Vertex2(x + texture.Width, y + texture.Height);
			GL.TexCoord2(1, 0); GL.Vertex2(x + texture.Width, y);
			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}

		public override void DrawCircle(Color color, Point location, double radius)
		{
			throw new NotImplementedException();
		}

		public override void FillCircle(Color color, Point location, double radius)
		{
			throw new NotImplementedException();
		}

		public override void DrawRectangle(Color color, double x, double y, double width, double height)
		{
			//GL.Ortho(x, y, x + width, y + height, 0, 1);
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(color.ToGLColor());
			GL.Vertex2(x, y);
			GL.Vertex2(x, y + height);
			GL.Vertex2(x + width, y + height);
			GL.Vertex2(x + width, y);
			GL.End();
		}

		public override void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			//GL.Ortho(x, y, x + width, y + height, 0, 1);
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(col1.ToGLColor());
			GL.Vertex2(x, y);
			GL.Color3(col2.ToGLColor());
			GL.Vertex2(x, y + height);
			GL.Color3(col3.ToGLColor());
			GL.Vertex2(x + width, y + height);
			GL.Color3(col4.ToGLColor());
			GL.Vertex2(x + width, y);
			GL.End();
		}

		public override void FillRectangle(Color color, double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		public override void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		public override void DrawLine(Color color, double x1, double y1, double x2, double y2)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Color3(color.ToGLColor());
			GL.Vertex2(x1, y1);
			GL.Vertex2(x2, y2);
			GL.End();
		}

		public override void DrawLine(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Color3(col1.ToGLColor());
			GL.Vertex2(x1, y1);
			GL.Color3(col2.ToGLColor());
			GL.Vertex2(x2, y2);
			GL.End();
		}

		public override void DrawText(Color color, object text, double x, double y)
		{
			throw new NotImplementedException();
		}

		public override void Clear(Color color)
		{
			GL.ClearColor(color.ToGLColor());
		}

		public override void Blit(Surface dest, IntRectangle srcRect, IntRectangle destRect)
		{
			throw new NotImplementedException();
		}

		public override int Width
		{
			get { throw new NotImplementedException(); }
		}

		public override int Height
		{
			get { throw new NotImplementedException(); }
		}
	}
}
