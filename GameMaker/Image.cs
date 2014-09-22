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

		public int Count
		{
			get
			{
				if (_parent.Sprite == null)
					return 1;
				else
					return _parent.Sprite.ImageCount;
			}
		}

		/// <summary>
		/// Gets the width of this GameMaker.Image, taking scale into consideration.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">GameMaker.Image.Sprite is set to null.</exception>
		public double Width
		{
			get
			{
				if (Sprite == null)
					throw new InvalidOperationException("The GameMaker.Image has no sprite.");
				return CurrentTexture.Width * Transform.XScale;
			}
		}

		/// <summary>
		/// Gets the height of this GameMaker.Image, taking scale into consideration.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">GameMaker.Image.Sprite is set to null.</exception>
		public double Height
		{
			get
			{
				if (Sprite == null)
					throw new InvalidOperationException("The GameMaker.Image has no sprite.");
				return CurrentTexture.Height * Transform.YScale;
			}
		}

		/// <summary>
		/// Gets the current texture of this GameMaker.Image.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">GameMaker.Image.Sprite is set to null.</exception>
		public Texture CurrentTexture
		{
			get
			{
				if (Sprite == null) throw new InvalidOperationException("The GameMaker.Image has no sprite.");
				return Sprite.Texture;
			}
		}

		/// <summary>
		/// Gets the transformation of this GameMaker.Image.
		/// </summary>
		public Transform Transform
		{
			get { return _parent.Transform; }
		}

		/// <summary>
		/// Animates this GameMaker.Image. Returns whether the animation looped.
		/// </summary>
		/// <returns>True if the animation looped.</returns>
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
