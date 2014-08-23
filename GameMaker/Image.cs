using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public sealed class Image
	{
		private GameObject _parent;

		internal Image(GameObject parent)
		{
			this._parent = parent;
			this.Blend = Color.White;
			this.Count = 1;
			this.Index = 0;
			this.Speed = 1;
		}

		public Sprite Sprite
		{
			get { return _parent.Sprite; }
		}

		public Color Blend { get; set; }
		
		public double Alpha
		{
			get { return Blend.A / 255.0; }
			set { Blend = Blend.Transparent(value); }
		}

		private double _index;
		public int Index
		{ 
			get { return (int)_index; }
			set { _index = value % Count; }
		}

		public double Speed
		{
			get;
			set;
		}

		public int Count { get; private set; }

		public int Width
		{
			get { return CurrentTexture.Width; }
		}

		public int Height
		{
			get { return CurrentTexture.Height; }
		}

		public Texture CurrentTexture
		{
			get { return Sprite.GetTexture(Index); }
		}

		public Transform Transform
		{
			get { return _parent.Transform; }
		}

		public bool Animate()
		{
			if (Sprite != null)
			{
				_index += Speed;
				if (_index >= Count)
				{
					_index %= Count;
					return true;
				}
			}

			return false;
		}
	}
}
