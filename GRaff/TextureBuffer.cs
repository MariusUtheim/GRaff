using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using GRaff.Synchronization;
using OpenTK.Graphics.ES30;
using GLPixelFormat = OpenTK.Graphics.ES30.PixelFormat;

namespace GRaff
{
	/// <summary>
	/// Represents a texture buffer.
	/// </summary>
	/// <remarks>
	/// The following file types are supported: BMP, GIF, EXIF, JPG, PNG and TIFF
	/// </remarks>
	public sealed class TextureBuffer : IDisposable
	{
		private bool _disposed = false;
		private int _id;

		private TextureBuffer(int width, int height)
		{
			Debug.Assert(GRaff.Graphics.Context.IsAlive);

			GL.GenTextures(1, out _id);

			if (_id == 0)
				Console.WriteLine("[TextureBuffer] Got id 0");
			Debug.Assert(_id != 0);

			this.Width = width;
			this.Height = height;


			GL.BindTexture(TextureTarget.Texture2D, _id);

			//GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.ES30.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
			//GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			ErrorCode errorCode;
			if ((errorCode = GL.GetError()) != ErrorCode.NoError) 
				throw new Exception(String.Format("Loading a texture caused an error: {0} (code: {1})", Enum.GetName(typeof(ErrorCode), errorCode), errorCode));
		}

		public TextureBuffer(int width, int height, IntPtr textureData)
			: this(width, height)
		{
			GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, Width, Height, 0, GLPixelFormat.Rgba, PixelType.UnsignedByte, textureData);
			GL.GenerateMipmap(TextureTarget.Texture2D);
		}

		public TextureBuffer(int width, int height, byte[] data)
			: this(width, height)
		{
			Debug.Assert(data.Length == width * height * 4);
			GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, Width, Height, 0, GLPixelFormat.Rgba, PixelType.UnsignedByte, data);
			GL.GenerateMipmap(TextureTarget.Texture2D);
		}

		/// <summary>
		/// Loads a texture from the specified file. 
		/// </summary>
		/// <param name="filename">The texture filename and path.</param>
		/// <returns>A new GRaff.Texture representing the loaded file.</returns>
		public static TextureBuffer Load(string filename)
		{
			using (var stream = new FileStream(filename, FileMode.Open))
			using (var bmp = new Bitmap(stream))
			{
				var textureData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
												ImageLockMode.ReadOnly,
												System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				return new TextureBuffer(bmp.Width, bmp.Height, textureData.Scan0);
			}

		}

		private static async Task<Bitmap> LoadBitmapAsync(string filename)
		{
			byte[] buffer;

            using (var inputStream = new FileStream(filename, FileMode.Open))
			{
				buffer = new byte[inputStream.Length];
				using (var outputStream = new MemoryStream(buffer))
					await inputStream.CopyToAsync(outputStream);
			}

			Bitmap bitmap;
			using (var bitmapStream = new MemoryStream(buffer))
				bitmap = new Bitmap(bitmapStream);
			return bitmap;
		}

		public static IAsyncOperation<TextureBuffer> LoadAsync(string filename)
		{
			return Async.RunAsync(() => LoadBitmapAsync(filename))
				.ThenSync(bitmap => {
					var textureData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
														 ImageLockMode.ReadOnly,
														System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					return new TextureBuffer(bitmap.Width, bitmap.Height, textureData.Scan0);
                });
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
					throw new ObjectDisposedException("Texture");
				return _id;
			}
		}

		public Texture GetTexture()
		{
			return new Texture(this, 0, 0, 1.0f, 1.0f); 
		}

#region IDisposable implementation
		~TextureBuffer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_disposed = true;
				if (Graphics.Context.IsAlive)
					GL.DeleteTexture(_id);
				else
					Async.Run(() => GL.DeleteTexture(_id));
			}
		}
#endregion
	}
}
