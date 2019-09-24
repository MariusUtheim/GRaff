using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using GRaff.Graphics;
using GRaff.Synchronization;
using OpenTK.Graphics.OpenGL4;
using GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;


namespace GRaff
{
    /// <summary>
    /// Represents a texture buffer.
    /// </summary>
    /// <remarks>
    /// The following file types are supported: BMP, GIF, EXIF, JPG, PNG and TIFF
    /// </remarks>
    public sealed class Texture : IDisposable
	{
		private Texture(int width, int height)
		{
			Id = GL.GenTexture();
			Width = width;
			Height = height;

			GL.BindTexture(TextureTarget.Texture2D, Id);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            _Graphics.ErrorCheck();
        }

        public Texture(Color[,] colors)
			: this(colors.GetLength(1), colors.GetLength(0))
		{
			Contract.Requires<ArgumentNullException>(colors != null);


			var handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
			try
			{
				var data = handle.AddrOfPinnedObject();
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, GLPixelFormat.Rgba, PixelType.UnsignedByte, data);
			}
			finally
			{
				if (handle.IsAllocated)
					handle.Free();
			}

            _Graphics.ErrorCheck();
        }

        public Texture(int width, int height, IntPtr data)
			: this(width, height)
		{
			Contract.Requires<ArgumentOutOfRangeException>(width > 0 && height > 0);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, GLPixelFormat.Bgra, PixelType.UnsignedByte, data);
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(Width > 0);
			Contract.Invariant(Height > 0);
		}

        public static Texture Load(string path)
		{
			Contract.Ensures(Contract.Result<Texture>() != null);

           // byte[] buffer = File.ReadAllBytes(path);
            Bitmap bitmap;

            //var bitmapStream = new MemoryStream(buffer);
            using (var sourceImage = Image.FromFile(path))
            {
                bitmap = new Bitmap(sourceImage.Width, sourceImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var gr = System.Drawing.Graphics.FromImage(bitmap))
                    gr.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, sourceImage.Width, sourceImage.Height));
            }

            var textureData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                  ImageLockMode.ReadOnly,
                                  bitmap.PixelFormat);
            try
            {
                return new Texture(textureData.Width, textureData.Height, textureData.Scan0);
            }
            finally
            {
                bitmap.UnlockBits(textureData);
            }
        }

		public static IAsyncOperation<Texture> LoadAsync(string path)
		{
            Contract.Ensures(Contract.Result<IAsyncOperation<Texture>>() != null);
			return Async.RunAsync(async () =>
			{
				byte[] buffer;

				using (var inputStream = new FileStream(path, FileMode.Open))
				{
					buffer = new byte[inputStream.Length];
					using (var outputStream = new MemoryStream(buffer))
						await inputStream.CopyToAsync(outputStream);
				}


                var bitmapStream = new MemoryStream(buffer);
				using (var sourceImage = new Bitmap(bitmapStream))
				{
					var bitmap = new Bitmap(sourceImage.Width, sourceImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					using (var gr = System.Drawing.Graphics.FromImage(bitmap))
						gr.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, sourceImage.Width, sourceImage.Height));
					return bitmap;
                }
			})
			.ThenQueue(bitmap =>
			{
				var textureData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
												  ImageLockMode.ReadOnly,
												  bitmap.PixelFormat);
                try
                {
                    return new Texture(textureData.Width, textureData.Height, textureData.Scan0);
                }
                finally
                {
                    bitmap.UnlockBits(textureData);
                }
			});
		}


        public static Texture FromScreen()
        {
            using (var buffer = Framebuffer.CopyFromScreen())
                return buffer.Texture;
		}


		public int Id { get; private set; }

		public int Width { get;}

		public int Height { get; }

        public IntVector Size => (Width, Height);

		public SubTexture SubTexture(Rectangle region)
            => GRaff.Graphics.SubTexture.FromTexCoords(this, region);

        public void SetWrapMode(TextureRepeatMode horizontal, TextureRepeatMode vertical)
        {
            GL.BindTexture(TextureTarget.Texture2D, Id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)horizontal);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)vertical);
        }

		public void SetWrapMode(TextureRepeatMode mode)
		{
			SetWrapMode(mode, mode);
		}

		public void SetWrapMode(TextureRepeatMode horizontal, TextureRepeatMode vertical, Color borderColor)
        {
            SetWrapMode(horizontal, vertical);
            GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, new[] { borderColor.Rgba });
        }

        public TextureScaleFilter MagnificationFilter
        {
            get
            {
                this.Bind();
                GL.GetTexParameterI(TextureTarget.Texture2D, GetTextureParameter.TextureMagFilter, out int value);
                
                return (TextureScaleFilter)value;
            }

            set
            {
                this.Bind();
                GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new[] { (int)value });
            }
        }

        public TextureScaleFilter MinificationFilter
        {
            get
            {
                this.Bind();
                GL.GetTexParameterI(TextureTarget.Texture2D, GetTextureParameter.TextureMinFilter, out int value);

                return (TextureScaleFilter)value;
            }

            set
            {
                this.Bind();
                GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new[] { (int)value });
            }
        }

        public Color[,] ToColorArray()
        {
            var colors = new Color[Height, Width];

            var handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            try
            {
                var data = handle.AddrOfPinnedObject();
                GL.BindTexture(TextureTarget.Texture2D, Id);
                GL.GetTexImage(TextureTarget.Texture2D, 0, GLPixelFormat.Rgba, PixelType.UnsignedByte, data);
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }

            return colors;
        }


        public void Save(string path)
		{
            var img = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			var imgData = img.LockBits(new System.Drawing.Rectangle(0, 0, img.Width, img.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			var dataSize = new IntPtr(Marshal.SizeOf(typeof(Color)) * Width * Height);

            GL.BindTexture(TextureTarget.Texture2D, Id);
            GL.GetTexImage(TextureTarget.Texture2D, 0, GLPixelFormat.Bgra, PixelType.UnsignedByte, imgData.Scan0);

			img.UnlockBits(imgData);

            img.Save(path);
		}


        #region IDisposable implementation

        public bool IsDisposed { get; private set; }

        ~Texture()
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
                Async.Capture(Id).ThenQueue(id =>
                {
                    if (_Graphics.IsContextActive)
                    {
                        GL.DeleteTexture(id);
                        _Graphics.ErrorCheck();
                    }
                });
                IsDisposed = true;
            }
        }

        #endregion

    }

}
