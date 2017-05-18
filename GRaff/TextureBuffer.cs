using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Synchronization;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
using GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
#else
using OpenTK.Graphics.ES30;
using GLPixelFormat = OpenTK.Graphics.ES30.PixelFormat;
#endif


namespace GRaff
{
	/// <summary>
	/// Represents a texture buffer.
	/// </summary>
	/// <remarks>
	/// The following file types are supported: BMP, GIF, EXIF, JPG, PNG and TIFF
	/// </remarks>
	#warning Review class
	public sealed class TextureBuffer : IDisposable
	{
		public TextureBuffer(int width, int height, IntPtr data)
		{
			Contract.Requires<ArgumentOutOfRangeException>(width > 0 && height > 0);

			Id = GL.GenTexture();
			Width = width;
			Height = height;
#if OpenGL4
			GL.BindTexture(TextureTarget.Texture2D, Id);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, GLPixelFormat.Bgra, PixelType.UnsignedByte, data);
#else
			GL.BindTexture(TextureTarget.Texture2D, Id);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, Width, Height, 0, GLPixelFormat.Rgba, PixelType.UnsignedByte, textureData.Scan0);
			GL.GenerateMipmap(TextureTarget.Texture2D);
#endif


			var err = GL.GetError();
			if (err != ErrorCode.NoError)
				throw new Exception(Enum.GetName(typeof(ErrorCode), err));
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(Width > 0);
			Contract.Invariant(Height > 0);
			Contract.Invariant(Texture != null);
		}

		public bool IsDisposed { get; private set; }

		~TextureBuffer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				Async.Run(Id, id =>
				{
					if (Giraffe.IsRunning)
						GL.DeleteTexture(id);
					Console.WriteLine($"[TextureBuffer] Disposed Id={Id}");
					var err = GL.GetError();
					if (err != ErrorCode.NoError)
						throw new Exception();
				});
				IsDisposed = true;
			}
		}

		public static TextureBuffer Load(string path)
		{
			Contract.Ensures(Contract.Result<TextureBuffer>() != null);
			return LoadAsync(path).Wait();
		}

		public static IAsyncOperation<TextureBuffer> LoadAsync(string path)
		{
			return Async.RunAsync(async () =>
			{
				byte[] buffer;

				using (var inputStream = new FileStream(path, FileMode.Open))
				{
					buffer = new byte[inputStream.Length];
					using (var outputStream = new MemoryStream(buffer))
						await inputStream.CopyToAsync(outputStream);
				}


				using (var bitmapStream = new MemoryStream(buffer))
				{

					using (var sourceImage = new Bitmap(bitmapStream))
					{
						var bitmap = new Bitmap(sourceImage.Width, sourceImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
						using (var gr = System.Drawing.Graphics.FromImage(bitmap))
							gr.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, sourceImage.Width, sourceImage.Height));
						return bitmap;
                    }
				}
			})
			.ThenQueue(bitmap =>
			{
				var textureData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
												  ImageLockMode.ReadOnly,
												  bitmap.PixelFormat);
				
				return new TextureBuffer(textureData.Width, textureData.Height, textureData.Scan0);
			});
		}

		public int Id { get; private set; }

		private Texture _texture = null;
		public Texture Texture => _texture ?? (_texture = new Texture(this, new GraphicsPoint(0, 0), new GraphicsPoint(1, 0), new GraphicsPoint(0, 1), new GraphicsPoint(1, 1)));


		public int Width { get;}

		public int Height { get; }

		public Texture Subtexture(Rectangle region)
			=> Texture.FromTexCoords(this, region);

		public void Save(string path)
		{
			var img = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			var data = img.LockBits(new System.Drawing.Rectangle(0, 0, img.Width, img.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			var dataSize = new IntPtr(System.Runtime.InteropServices.Marshal.SizeOf(typeof(Color)) * Width * Height);
            GL.GetBufferSubData(BufferTarget.TextureBuffer, IntPtr.Zero, dataSize, data.Scan0);
			img.UnlockBits(data);
			
			img.Save(path);
		}
	}

}
