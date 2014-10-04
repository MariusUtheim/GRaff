using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
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
		/// Gets the width of this GRaff.Image, taking scale into consideration.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">GRaff.Image.Sprite is set to null.</exception>
		public double Width
		{
			get
			{
				if (Sprite == null)
					throw new InvalidOperationException(String.Format("The {0}.{1} has no sprite.", nameof(GRaff), nameof(Image)));
				return CurrentTexture.Width * Transform.XScale;
			}
		}

		/// <summary>
		/// Gets the height of this GRaff.Image, taking scale into consideration.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">GRaff.Image.Sprite is set to null.</exception>
		public double Height
		{
			get
			{
				if (Sprite == null)
					throw new InvalidOperationException(String.Format("The {0}.{1} has no sprite.", nameof(GRaff), nameof(Image)));
				return CurrentTexture.Height * Transform.YScale;
			}
		}

		/// <summary>
		/// Gets the current texture of this GRaff.Image.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">GRaff.Image.Sprite is set to null.</exception>
		public Texture CurrentTexture
		{
			get
			{
				if (Sprite == null) throw new InvalidOperationException(String.Format("The {0}.{1} has no sprite.", nameof(GRaff), nameof(Image)));
				return Sprite.Texture;
			}
		}

		/// <summary>
		/// Gets the transformation of this GRaff.Image.
		/// </summary>
		public Transform Transform
		{
			get { return _parent.Transform; }
		}

		/// <summary>
		/// Animates this GRaff.Image. Returns whether the animation looped.
		/// </summary>
		/// <returns>true if the animation looped.</returns>
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
