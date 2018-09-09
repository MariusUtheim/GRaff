using System;
using System.Diagnostics.Contracts;
using GRaff.Graphics;

namespace GRaff
{
	public sealed class Model
	{
		private readonly GameObject _parent;

		internal Model(GameObject parent)
		{
			Contract.Requires<ArgumentNullException>(parent != null);
			this._parent = parent;
			this.Blend = Colors.White;
			this.Index = 0;
			this.Speed = 1;
		}

		[ContractInvariantMethod]
		private void objectInvariants()
		{
			Contract.Invariant(_parent != null);
		}

		public Sprite Sprite
		{
			get; set;
		}

		public Color Blend { get; set; }
		
		public double Alpha
		{
			get { return Blend.A / 255.0; }
			set { Blend = Blend.Transparent(value); }
		}

		private double _index;
		public double Index
		{ 
			get { return _index; }
			set { _index = GMath.Remainder(value, Count); }
		}

		public double Speed
		{
			get;
			set;
		}

		public int Count => Sprite?.AnimationStrip.ImageCount ?? 1;

		public double Period
		{
			get
			{
				if (Sprite == null)
					return Double.NaN;
				else if (Speed == 0)
					return Double.PositiveInfinity;
				else
					return Sprite.AnimationStrip.Duration / Speed;
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
					throw new InvalidOperationException(String.Format("The {0}.{1} has no sprite.", "GRaff", "Image"));
				return Sprite.Width * GMath.Abs(Transform.XScale);
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
					throw new InvalidOperationException(String.Format("The {0}.{1} has no sprite.", "GRaff", "Image"));
				return Sprite.Height * GMath.Abs(Transform.YScale);
			}
		}

		/// <summary>
		/// Gets the current texture of this GRaff.Image.
		/// </summary>
		/// <exception cref="System.InvalidOperationException">GRaff.Image.Sprite is set to null.</exception>
		public SubTexture CurrentTexture
		{
			get
			{
				if (Sprite == null) throw new InvalidOperationException(String.Format("The {0}.{1} has no sprite.", "GRaff", "Image"));
				return Sprite.SubImage(Index);
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
				if (_index >= Count || _index < 0)
				{
					_index = GMath.Remainder(_index, Count);
					return true;
				}
			}

			return false;
		}
	}
}
