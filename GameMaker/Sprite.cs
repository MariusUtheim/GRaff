using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public class Sprite
	{
		private string _path;
		private bool _isLoaded;
		private Texture _texture; ///TEMPORARY/// Support multiple textures
		private IntVector _origin;

		public Sprite(string path, OriginMode originMode, bool preload)
		{
			this._path = path;
			this.OriginMode = originMode;
			if (preload)
				Load();
		}

		public Sprite(string path)
		{
			this._path = path;
			this._isLoaded = false;
		}
		   
		public Sprite(string path, bool preload)
		{
			this._path = path;
			if (preload)
				Load();
		}

		public Vector Size
		{
			get { Load(); return new Vector(_texture.Width, _texture.Height); }
		}

		public int Width
		{
			get { Load(); return _texture.Width; }
		}

		public int Height
		{
			get { Load(); return _texture.Height; }
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
			_texture = Draw.LoadTexture(_path);
			if (OriginMode == GameMaker.OriginMode.UpperLeft)
				_origin = new IntVector(0, 0);
			else if (OriginMode == GameMaker.OriginMode.Center)
				_origin = new IntVector(_texture.Width / 2, _texture.Height / 2);
		}

		public Texture GetTexture(int ImageIndex)
		{
			Load();
			return _texture; ///TEMPORARY///
		}
	}
}
