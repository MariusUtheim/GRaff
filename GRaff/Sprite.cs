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
		private IAsyncOperation _loadingOperation;

		/// <summary>
		/// Initializes a new instance of the GRaff.Sprite class. This only declares the sprite data, it will not be loaded until you call Sprite.Load.
		/// </summary>
		/// <param name="filename">The texture file that will be loaded from disk.</param>
		/// <param name="subimages">The number of subimages if the texture is an animation strip. The default value is 1.</param>
		/// <param name="origin">A GRaff.IntVector? representing the origin of the image. If null, the origin will be automatically set to the center of the loaded texture. The default value is null.</param>
		/// <param name="maskShape">Specifies the mask of the sprite. If the value is GRaff.MaskShape.Automatic, a rectangular mask filling the whole image will be used. If the value is null, it will default to GRaff.MaskShape.Automatic.</param>
		/// <param name="animationStrip">Specifies the animation strip of the sprite. If null, the strip will consist of all the subimages in order and with uniform duration. The default value is null.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">subimages is less than or equal to 0.</exception>
		public Sprite(string filename, int subimages = 1, IntVector? origin = default(IntVector?), MaskShape maskShape = null, AnimationStrip animationStrip = null)
		{
			if (subimages < 1) throw new ArgumentOutOfRangeException("subimages", "Must be greater than or equal to 1");
			this.ImageCount = subimages;
			this.FileName = filename;
			this._origin = origin;
			this._hasCustomMask = maskShape != null;
			this._maskShape = maskShape ?? MaskShape.Automatic;
			this._animationStrip = animationStrip ?? new AnimationStrip(subimages);
		}


		/// <summary>
		/// Loads a sprite from a file.
		/// </summary>
		/// <param name="path">The file to load.</param>
		/// <param name="subimages">The number of subimages if the texture is an animation strip. The default value is 1.</param>
		/// <param name="origin">A GRaff.IntVector? representing the origin of the image. If null, the origin will be automatically set to the center of the loaded texture. The default value is null.</param>
		/// <param name="maskShape">Specifies the mask of the sprite. If null, a rectangular mask filling the whole image will be used. The default value is null.</param>
		/// <param name="animationStrip">Specifies the animation strip of the sprite. If null, the strip will consist of all the subimages in order and with uniform duration. The default value is null.</param>
		/// <returns>The loaded sprite.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">subimages is less than or equal to 0.</exception>
		/// <exception cref="System.IO.IOException">The specified file does not exist.</exception>
		public static Sprite Load(string path, int subimages = 1, IntVector? origin = null, MaskShape maskShape = null, AnimationStrip animationStrip = null)
		{
			var result = new Sprite(path, subimages, origin, maskShape, animationStrip);
			result.Load();
			return result;
		}

		/// <summary>
		/// Asynchronously loads a sprite from a file.
		/// </summary>
		/// <param name="path">The file to load.</param>
		/// <param name="subimages">The number of subimages if the texture is an animation strip. The default value is 1.</param>
		/// <param name="origin">A GRaff.IntVector? representing the origin of the image. If null, the origin will be automatically set to the center of the loaded texture. The default value is null.</param>
		/// <param name="maskShape">Specifies the mask of the sprite. If null, a rectangular mask filling the whole image will be used. The default value is null.</param>
		/// <param name="animationStrip">Specifies the animation strip of the sprite. If null, the strip will consist of all the subimages in order and with uniform duration. The default value is null.</param>
		/// <returns>A System.Treading.Tasks.Task`1 that will return the loaded GRaff.Sprite upon completion.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">subimages is less than or equal to 0.</exception>
		/// <exception cref="System.IO.IOException">The specified file does not exist.</exception>
		public static IAsyncOperation<Sprite> LoadAsync(string path, int subimages = 1, IntVector? origin = null, MaskShape maskShape = null, AnimationStrip animationStrip = null)
		{
			var result = new Sprite(path, subimages, origin, maskShape, animationStrip);
			return result.LoadAsync().ThenSync(() => result);
		}

		/// <summary>
		/// Gets a string indicating the filename from which the texture of this GRaff.Sprite is loaded.
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		/// Gets whether the texture of this GRaff.Sprite is loaded.
		/// </summary>
		public bool IsLoaded { get { return AssetState == AssetState.Loaded; } } /*C#6.0*/

		/// <summary>
		/// Gets the asset state of the texture of this GRaff.Sprite.
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

		/// <summary>
		/// Gets or sets the MaskShape of this GRaff.Sprite. If set to MaskShape.Automatic, a rectangular mask filling the whole image will be used.
		/// </summary>
		public MaskShape MaskShape
		{
			get
			{
				return _maskShape;
			}

			set
			{
				if (value == MaskShape.Automatic)
				{
					_hasCustomMask = false;
					if (IsLoaded)
						_maskShape = MaskShape.Rectangle(_width, _height);
				}
				else
				{
					_hasCustomMask = true;
					_maskShape = value;
				}
			}
		}

		private void _load(TextureBuffer texture)
		{  // This is the actual load event. Load, LoadAsync and Unload basically provide thread-safe access to this method.
			_texture = texture;
			_width = _texture.Width / ImageCount;
			_height = _texture.Height;
			if (!_hasCustomMask)
				_maskShape = MaskShape.Rectangle(_width, _height);
			AssetState = AssetState.Loaded;
		}

		/// <summary>
		/// Loads the texture.
		/// </summary>
		/// <exception cref="System.IO.FileNotFoundException">The texture file does not exists.</exception>
		/// <remarks>If the texture is already loading asynchronously, calling GRaff.Sprite.Load blocks until loading completes.</remarks>
		public void Load()
		{
			if (AssetState == AssetState.Loaded)
				return;
			else
				LoadAsync().Wait();
		}

		/// <summary>
		/// Loads the texture asynchronously.
		/// </summary>
		/// <returns>A System.Threading.Tasks.Task that will complete when the texture is finished loading.</returns>
		public IAsyncOperation LoadAsync()
		{
			if (AssetState != AssetState.NotLoaded)
				return _loadingOperation;

			AssetState = AssetState.LoadingAsync;

			return TextureBuffer.LoadAsync(FileName)
								.ThenSync(textureBuffer => _load(textureBuffer));
		}

		/// <summary>
		/// Unloads the texture.
		/// </summary>
		public void Unload()
		{
			if (AssetState == AssetState.NotLoaded)
				return;
			if (AssetState == AssetState.LoadingAsync)
				_loadingOperation.Abort();
	
			AssetState = AssetState.NotLoaded;
			
			if (_texture != null)
			{
				_texture.Dispose();
				_texture = null;
			}
		}

		/// <summary>
		/// Gets the GRaff.TextureBuffer containing the texture of this GRaff.Sprite.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The texture is not loaded.</exception>
		public TextureBuffer Texture
		{
			get
			{
				if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
				return _texture;//[imageIndex % _subimages];
			}
		}

		/// <summary>
		/// Gets the GRaff.Texture containing the specified subimage of this GRaff.Sprite.
		/// </summary>
		/// <param name="index">The index of the subimage. If the value is not in the range [0, ImageCount), the modulus will be used.</param>
		/// <returns>A GRaff.Texture containing the specified subimage.</returns>
		/// <exception cref="System.InvalidOperationException">The texture is not loaded.</exception>
		public Texture SubImage(int index)
		{
			if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The texture is not loaded.");
			index = ((index % ImageCount) + ImageCount) % ImageCount;
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
