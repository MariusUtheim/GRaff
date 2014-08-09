using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public class Sprite
	{
		private int _subimages;
		private string _filename;
		private bool _isLoaded;
		private Texture _texture;
		private IntVector _origin;

		public Sprite(string filename, int subimages = 1, OriginMode originMode = GameMaker.OriginMode.Manual, bool preload = true)
		{
			this._subimages = subimages;
			this._filename = filename;
			this.OriginMode = originMode;
			if (preload)
				Load();
		}
		
		public IntVector Size
		{
			get { Load(); return new IntVector(_width, _height); }
		}

		private int _width;
		public int Width
		{
			get { Load(); return _width; }
		}

		private int _height;
		public int Height
		{
			get { Load(); return _height; }
		}

		public IntVector Origin
		{
			get { return _origin; }
			set { _origin = value; }
		}

		public int XOrigin
		{
			get { return _origin.X; }
			set { _origin.X = value; }
		}

		public int YOrigin
		{
			get { return _origin.Y; }
			set { _origin.Y = value; }
		}

		public OriginMode OriginMode { get; set; }

		public void Load()
		{
			if (_isLoaded)
				return;
			_isLoaded = true;
			_texture = new Texture(_filename);
			_width = _texture.Width;
			_height = _texture.Height;

			if (_texture == null)
				return;

			if (OriginMode == GameMaker.OriginMode.UpperLeft)
				_origin = new IntVector(0, 0);
			else if (OriginMode == GameMaker.OriginMode.Center)
				_origin = new IntVector(_width / 2, _height / 2);
		}

		public Texture GetTexture(int imageIndex)
		{
			Load();
#warning TODO: Return texture based on imageIndex
			return _texture;//[imageIndex % _subimages];
		}

		public int ImageCount 
		{
			get { return _subimages; }
		}
	}
}
