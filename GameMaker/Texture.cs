using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace GameMaker
{
	public class Texture : IDisposable
	{
		private Texture()
		{
			Id = GL.GenTexture();
		}


		/// <summary>
		/// Loads a texture from the specified file. 
		/// </summary>
		/// <param name="filename">The texture filename and path.</param>
		/// <returns>A new GameMaker.Texture representing the loaded file.</returns>
		public static Texture Load(string filename)
		{
			Texture result = new Texture();
	
			using (var stream = new FileStream(filename, FileMode.Open))
			using (var bmp = new Bitmap(stream))
			{
				result.Width = bmp.Width;
				result.Height = bmp.Height;
				var textureData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, result.Width, result.Height),
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				GL.BindTexture(TextureTarget.Texture2D, result.Id);

				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, result.Width, result.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				bmp.UnlockBits(textureData);
			}

			var err = GL.GetError();
			if (err != ErrorCode.NoError)
				throw new NotImplementedException();

			return result;
		}

		public static async Task<Texture> LoadAsync(string filename)
		{
			Texture result;
			using (var bmp = await Task.Run(() => new Bitmap(filename)))
			{
				result = new Texture();
				result.Width = bmp.Width;
				result.Height = bmp.Height;
				var textureData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, result.Width, result.Height),
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				GL.BindTexture(TextureTarget.Texture2D, result.Id);

				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, result.Width, result.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

				bmp.UnlockBits(textureData);
			}

			return result;
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

		public int Id
		{
			get;
			private set;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
