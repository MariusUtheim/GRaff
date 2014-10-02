using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace GameMaker
{
	public sealed class Texture : IDisposable
	{
		private bool _disposed = false;
		private int _id;

		internal Texture(Bitmap bmp)
		{
			_id = GL.GenTexture();

			Width = bmp.Width;
			Height = bmp.Height;
			var textureData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height),
				ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.BindTexture(TextureTarget.Texture2D, _id);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			bmp.UnlockBits(textureData);

		}

		/// <summary>
		/// Loads a texture from the specified file. 
		/// </summary>
		/// <param name="filename">The texture filename and path.</param>
		/// <returns>A new GameMaker.Texture representing the loaded file.</returns>
		public static Texture Load(string filename)
		{
			Texture result = null;

			FileStream stream = null;
			Bitmap bmp = null;
			try
			{
				stream = new FileStream(filename, FileMode.Open);
				using (bmp = new Bitmap(stream))
				{
					stream = null;
					return new Texture(bmp);
				}
			}
			catch
			{
				if (result != null)
					result.Dispose();
				throw;
			}
			finally
			{
				if (stream != null)
					stream.Dispose();
			}
		}

		public static async Task<Texture> LoadAsync(string filename)
		{
			Texture result;
			using (var bmp = await Task.Run(() => new Bitmap(filename)))
				result = new Texture(bmp);

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
			get
			{
				if (_disposed)
					throw new ObjectDisposedException(nameof(Texture));
				return _id;
			}
		}

		~Texture()
		{
		   if (Game.Window.Exists)
				Dispose();
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				GL.DeleteTexture(_id);
				_disposed = true;
				GC.SuppressFinalize(this);
			}
		}


	}
}
