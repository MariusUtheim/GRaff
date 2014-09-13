using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Represents a sprite resource. This includes the image file, and metadata such as origin. 
	/// The texture itself does not have to be loaded into memory at the time this class is instantiated.
	/// <remarks>Supported file formats are BMP, GIF, EXIF, JPG, PNG and TIFF</remarks>
	/// </summary>
	public class Sprite
	{
		private int _subimages;
		private string _filename;
		private bool _isLoaded;
		private Texture _texture;
		private IntVector? _origin;
		private int _width;
		private int _height;

		/// <summary>
		/// Initializes a new instance of the GameMaker.Sprite class.
		/// </summary>
		/// <param name="filename">The texture file that is loaded from disk.</param>
		/// <param name="subimages">The number of subimages if the texture is an animation strip. The default value is 1.</param>
		/// <param name="origin">A GameMaker.IntVector? representing the origin of the image. If null, the origin will be automatically set to the center of the loaded texture. The default value is null.</param>
		/// <param name="preload">If true, the texture is automatically loaded when the instance is initialized. The default value is true.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">subimages is less than or equal to 0.</exception>
		/// <exception cref="System.IO.FileNotFoundException">preload is true and the file is not found.</exception>
		public Sprite(string filename, int subimages = 1, IntVector? origin = default(IntVector?), bool preload = true)
		{
			if (subimages < 1) throw new ArgumentOutOfRangeException("subimages", "Must be greater than or equal to 1");

			this._subimages = subimages;
			this._filename = filename;
			this._origin = origin;
			if (preload)
				Load();
		}
		
		/// <summary>
		/// Gets the width of this GameMaker.Sprite.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The texture is not loaded.</exception>
		public int Width
		{
			get 
			{
				if (!_isLoaded) throw new InvalidOperationException("The texture is not loaded.");
				return _width;
			}
		}

		/// <summary>
		/// Gets the height of this GameMaker.Sprite.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The texture is not loaded.</exception>
		public int Height
		{
			get
			{
				if (!_isLoaded) throw new InvalidOperationException("The texture is not loaded.");
				return _height;
			}
		}

		/// <summary>
		/// Gets the size of this GameMaker.Sprite. 
		/// </summary>
		/// <exception cref="System.InvalidOperationException">The texture is not loaded.</exception>
		public IntVector Size
		{
			get
			{
				if (!_isLoaded) throw new InvalidOperationException("The texture is not loaded.");
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
				else if (_isLoaded)
					return new IntVector(_width / 2, _height / 2);
				else
					return null;
			}
			
			set { _origin = value; }
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
				else if (_isLoaded)
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
				else if (_isLoaded)
					return _height / 2;
				else
					return 0;
			}
		}

		/// <summary>
		/// Loads the texture.
		/// </summary>
		/// <exception cref="System.IO.FileNotFoundException">The texture file does not exists.</exception>
		public void Load()
		{
			if (_isLoaded)
				return;
			_isLoaded = true;
			_texture = new Texture(_filename);
			_width = _texture.Width / ImageCount;
			_height = _texture.Height;

			if (_texture == null)
				return;
		}

#warning TODO: LoadAsync()

#warning TODO: Unload()

		public Texture GetTexture(int imageIndex)
		{
			Load();
#warning TODO: Return texture based on imageIndex
			return _texture;//[imageIndex % _subimages];
		}

		/// <summary>
		/// Gets the number of subimages in the animation.
		/// </summary>
		public int ImageCount 
		{
			get { return _subimages; }
		}
	}
}
