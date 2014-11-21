using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using GRaff.Synchronization;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;

namespace GRaff
{
	public sealed class TextureBuffer : IDisposable
	{
		bool _disposed = false;
		int _id;

		internal TextureBuffer(int width, int height, IntPtr textureData)
		{

			GL.GenTextures(1, out _id);
			this.Width = width;
			this.Height = height;

			GL.BindTexture(TextureTarget.Texture2D, _id);
			
			//GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, OpenTK.Graphics.ES30.PixelFormat.Bgra, PixelType.UnsignedByte, textureData.Scan0);
			//GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, Width, Height, 0, OpenTK.Graphics.ES30.PixelFormat.Rgba, PixelType.UnsignedByte, textureData);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			GL.GenerateMipmap(TextureTarget.Texture2D);

			ErrorCode errorCode;
			if ((errorCode = GL.GetError()) != ErrorCode.NoError) /*C#6.0*/
				throw new Exception(String.Format("Loading a texture caused an error: {0} (code: {1})", Enum.GetName(typeof(ErrorCode), errorCode), errorCode));
		}

		/// <summary>
		/// Loads a texture from the specified file. 
		/// </summary>
		/// <param name="filename">The texture filename and path.</param>
		/// <returns>A new GRaff.Texture representing the loaded file.</returns>
		public static TextureBuffer Load(string filename)
		{
			Console.WriteLine("<{0}>", Thread.CurrentThread.Name);
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

		public static async Task<TextureBuffer> LoadAsync(string filename)
		{
			var bitmap = await LoadBitmapAsync(filename);
			var textureData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
												 ImageLockMode.ReadOnly,
												System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			return await Async.MainThreadDispatcher.InvokeAsync(() => new TextureBuffer(bitmap.Width, bitmap.Height, textureData.Scan0));
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
