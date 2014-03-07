using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using System.Drawing;
using System.Drawing.Imaging;

namespace GameMaker.OpenGL
{
	public class OpenGLTexture : Texture
	{
		private int id;
		private Bitmap _bmp;
		private int _w, _h;

		public OpenGLTexture(string path)
		{
			try
			{
				_bmp = new Bitmap(path);
				var textureData = _bmp.LockBits(new System.Drawing.Rectangle(0, 0, _bmp.Width, _bmp.Height),
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				GL.GenTextures(1, out id);
				GL.BindTexture(TextureTarget.Texture2D, id);
				GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

				Glu.Build2DMipmap(TextureTarget.Texture2D, (int)PixelInternalFormat.Three, _bmp.Width, _bmp.Height, OpenTK.Graphics.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);

				GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out _w);
				GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out _h);

				_bmp.UnlockBits(textureData);
			}
			catch (Exception e)
			{
				throw;
			}
		}

		internal int Id { get { return id; } }

		public override int Width
		{
			get 
			{
				return _w;
			}
		}

		public override int Height
		{
			get
			{
				return _h;
			}
		}
	}
}
