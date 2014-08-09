using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace GameMaker
{
	public class Texture
	{
		internal int Id { get; private set; }

		public Texture(string filename)
		{
			Id = GL.GenTexture();

			using (var bmp = new Bitmap(filename))
			{
				Width = bmp.Width;
				Height = bmp.Height;
				var textureData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height),
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				GL.BindTexture(TextureTarget.Texture2D, Id);
				GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Modulate);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

				OpenTK.Graphics.Glu.Build2DMipmap(OpenTK.Graphics.TextureTarget.Texture2D, (int)PixelInternalFormat.Three, Width, Height, OpenTK.Graphics.PixelFormat.Bgra, OpenTK.Graphics.PixelType.UnsignedByte, textureData.Scan0);

				bmp.UnlockBits(textureData);
			}

			var err = GL.GetError();
			if (err != ErrorCode.NoError)
			{
			}

		}

		public int Width
		{
			get;
			private set;
		}
		public int Height
		{
			get;
			private set;
		}
	}
}
