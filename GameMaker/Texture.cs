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

				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				bmp.UnlockBits(textureData);
			}

			var err = GL.GetError();
			if (err != ErrorCode.NoError)
			{
				throw new NotImplementedException();
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
