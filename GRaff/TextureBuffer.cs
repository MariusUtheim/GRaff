using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using GRaff.Graphics;
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
	public sealed class TextureBuffer : IAsset
	{
		private IAsyncOperation _loadingOperation;

		public TextureBuffer(string path)
		{
			Debug.Assert(GRaff.Graphics.Context.IsAlive);
			Id = GL.GenTexture();
			this.Path = path;
			Texture = new Texture(this, new Graphics.PointF(0, 0), new Graphics.PointF(1, 0), new Graphics.PointF(0, 1), new Graphics.PointF(1, 1));
		}

		~TextureBuffer()
		{
			_loadingOperation?.Abort();
			Async.Run(() =>
			{
				if (GL.IsBuffer(Id))
					GL.DeleteBuffer(Id);
			});
		}

		public static TextureBuffer Load(string path)
		{
			var buffer = new TextureBuffer(path);
			buffer.Load();
			return buffer;
		}

		public static IAsyncOperation<TextureBuffer> LoadAsync(string path)
			=> Async.Run(() => new TextureBuffer(path)).ThenRun(buffer => buffer.LoadAsync().Then(() => buffer));

		public int Id { get; private set; }

		public Texture Texture { get; private set; }

		public bool IsLoaded { get; private set; }

		public string Path { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public IAsyncOperation LoadAsync()
		{
			return Async.RunAsync(async () =>
			{
				byte[] buffer;

				using (var inputStream = new FileStream(Path, FileMode.Open))
				{
					buffer = new byte[inputStream.Length];
					using (var outputStream = new MemoryStream(buffer))
						await inputStream.CopyToAsync(outputStream);
				}

				using (var bitmapStream = new MemoryStream(buffer))
					return new Bitmap(bitmapStream);
			})
			.Then(bitmap =>
			{
				var textureData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
													ImageLockMode.ReadOnly,
													System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				this.Width = bitmap.Width;
				this.Height = bitmap.Height;

				GL.BindTexture(TextureTarget.Texture2D, Id);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
				GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, Width, Height, 0, GLPixelFormat.Rgba, PixelType.UnsignedByte, textureData.Scan0);
				GL.GenerateMipmap(TextureTarget.Texture2D);

				IsLoaded = true;
				_loadingOperation = null;

				var err = GL.GetError();
				if (err != ErrorCode.NoError)
					throw new Exception(Enum.GetName(typeof(ErrorCode), err));
			});
		}

		public void Unload()
		{
			_loadingOperation?.Abort();
			_loadingOperation = null;
			if (IsLoaded)
			{
				GL.DeleteBuffer(Id);
				Width = 0;
				Height = 0;
				IsLoaded = false;
			}
		}

		public Texture Subtexture(Rectangle region)
			=> new Texture(this, region);
	}

}
