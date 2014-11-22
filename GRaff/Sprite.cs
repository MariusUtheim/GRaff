using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GRaff.Synchronization;

namespace GRaff
{
	/// <summary>
	/// Represents a sprite resource. This includes the image file, and metadata such as origin. 
	/// The texture itself does not have to be loaded into memory at the time this class is instantiated.
	/// <remarks>Supported file formats are BMP, GIF, EXIF, JPG, PNG and TIFF</remarks>
	/// </summary>
	public sealed class Sprite : IAsset
	{
		private TextureBuffer _texture;
		private IntVector? _origin;
		private int _width;
		private int _height;
		private bool _hasCustomMask;
		private MaskShape _maskShape;
		private AnimationStrip _animationStrip;

		/// <summary>
		/// Initializes a new instance of the GRaff.Sprite class.
		/// </summary>
		/// <param name="filename">The texture file that is loaded from disk.</param>
		/// <param name="subimages">The number of subimages if the texture is an animation strip. The default value is 1.</param>
		/// <param name="origin">A GRaff.IntVector? representing the origin of the image. If null, the origin will be automatically set to the center of the loaded texture. The default value is null.</param>
		/// <param name="preload">If true, the texture is automatically loaded when the instance is initialized. The default value is true.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">subimages is less than or equal to 0.</exception>
		/// <exception cref="System.IO.FileNotFoundException">preload is true and the file is not found.</exception>
		public Sprite(string filename, int subimages = 1, IntVector? origin = default(IntVector?), AnimationStrip customAnimationStrip = null)
		{
			if (subimages < 1) throw new ArgumentOutOfRangeException("subimages", "Must be greater than or equal to 1");

			this.ImageCount = subimages;
			this.FileName = filename;
			this._origin = origin;
			this._hasCustomMask = false;
			this._maskShape = null;
			this._animationStrip = customAnimationStrip ?? new AnimationStrip(subimages);
		}

		public static Sprite Load(string path, int subimages, IntVector? origin)
		{
			var result = new Sprite(path, subimages, origin);
			result.Load();
			return result;
		}

		/// <summary>
		/// Gets a string indicating the filename from which the texture of this GRaff.Sprite is loaded.
		/// </summary>
		public string FileName { get; private set; }

		public bool IsLoaded { get { return AssetState == AssetState.Loaded; } } /*C#6.0*/

		/// <summary>
		/// Gets whether the texture of this GRaff.Sprite is loaded.
		/// </summary>
		public AssetState AssetState { get; private set; }

		/// <summary>
		/// Gets the width of this GRaff.Sprite.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The texture is not loaded.</exception>
		public int Width
		{
			get 
			{
				if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _width;
			}
		}

		/// <summary>
		/// Gets the height of this GRaff.Sprite.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The texture is not loaded.</exception>
		public int Height
		{
			get
			{
				if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _height;
			}
		}

		/// <summary>
		/// Gets the size of this GRaff.Sprite. 
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The texture is not loaded.</exception>
		public IntVector Size
		{
			get
			{
				if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return new IntVector(_width, _height);
			}
		}

		/// <summary>
		/// Gets or sets the origin of the image.
		/// If the value is null, the origin is automatically set to the center of the image. In this case,
		/// if the texture is loaded, the returned value is the center of that image; otherwise, it is null.
		/// </summary>
		public IntVector? Origin
		{
			get
			{
				if (_origin.HasValue)
					return _origin.Value;
				else if (AssetState != AssetState.Loaded)
					return new IntVector(_width / 2, _height / 2);
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the x-coordinate of the origin. If the origin is automatically centered and the texture is not
		/// loaded, this value is equal to zero.
		/// </summary>
		public int XOrigin
		{
			get
			{
				if (_origin.HasValue)
					return _origin.Value.X;
				else if (AssetState == AssetState.Loaded)
					return _width / 2;
				else
					return 0;
			}
		}

		/// <summary>
		/// Gets the y-coordinate of the origin. If the origin is automatically centered and the texture is not
		/// loaded, this value is equal to zero.
		/// </summary>
		public int YOrigin
		{
			get
			{
				if (_origin.HasValue)
					return _origin.Value.Y;
				else if (AssetState == AssetState.Loaded)
					return _height / 2;
				else
					return 0;
			}
		}

		public MaskShape MaskShape
		{
			get
			{
				if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _maskShape;
			}
		}

		private void _load(TextureBuffer texture)
		{  // This is the actual load event. Load, LoadAsync and Unload basically provide thread-safe access to this method.
			_texture = texture;
			_width = _texture.Width / ImageCount;
			_height = _texture.Height;
			if (!_hasCustomMask)
				_maskShape = MaskShape.Rectangle(-XOrigin, -YOrigin, _width, _height);
			AssetState = AssetState.Loaded;
		}

		/// <summary>
		/// Loads the texture.
		/// </summary>
		/// <exception cref="System.IO.FileNotFoundException">The texture file does not exists.</exception>
		/// <remarks>If the texture is already loading asynchronously, calling GRaff.Sprite.Load blocks until loading completes.</remarks>
		public void Load()
		{
			TextureBuffer textureBuffer;
			lock (this)
			{
				if (AssetState == AssetState.Loaded)
					return;
				textureBuffer = TextureBuffer.Load(FileName);
			}

			_load(textureBuffer);
		}

		public async Task LoadAsync()
		{
			if (AssetState != AssetState.NotLoaded)
				return;

			AssetState = AssetState.LoadingAsync;

			var textureBuffer = await TextureBuffer.LoadAsync(FileName);

			lock (this)
			{
				if (AssetState == AssetState.LoadingAsync)
					_load(textureBuffer);
			}
		}

		public void Unload()
		{
			lock (this)
			{
				if (AssetState == AssetState.NotLoaded)
					return;
				AssetState = AssetState.NotLoaded;
			}

			if (_texture != null)
			{
				_texture.Dispose();
				_texture = null;
			}
		}


		public TextureBuffer Texture
		{
			get
			{
				if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _texture;//[imageIndex % _subimages];
			}
		}

		public Texture SubImage(int index)
		{
			index %= ImageCount;
			float r = 1.0f / ImageCount;
			return new Texture(Texture, r * index, 0, r * (index + 1), 1);
		}

		/// <summary>
		/// Gets the number of subimages in the animation.
		/// </summary>
		public int ImageCount 
		{
			get; private set;
		}
	}
}
