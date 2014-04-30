using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Image
	{
		private IAnimationEndListener _animationEndListener;

		public Image(GameObject instance)
		{
			this.Instance = instance;
			this.XScale = 1;
			this.YScale = 1;
			this.Rotation = Angle.Zero;
			this.Blend = Color.White;
			this.Count = 1;
			this.Index = 0;

			this._animationEndListener = instance as IAnimationEndListener; // It won't cause any problems if this is set to null
		}

		public double XScale { get; set; }
		public double YScale { get; set; }
		public Angle Rotation { get; set; }
		public Color Blend { get; set; }
		public GameObject Instance { get; set; }

		public double Alpha
		{
			get { return Blend.A / 255.0; }
			set { Blend = new Color((int)(value * 255), Blend); }
		}

		public Sprite Sprite
		{
			get { return Instance.Sprite; }
		}

		public Transform Transform
		{
			get { return new Transform(XScale, YScale, Rotation, Sprite.Origin); }
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

		public Texture CurrentTexture
		{
			get { return Sprite.GetTexture(Index); }
		}

		public void Animate()
		{
			if (Sprite == null)
				_index = 0;
			else
			{
				_index += Speed;
				if (_index >= Sprite.ImageCount)
				{
					if (_animationEndListener != null)
						_animationEndListener.AnimationEnd();
					_index -= Sprite.ImageCount;
				}
			}
		}

		public int XOrigin 
		{
			get { return Sprite.XOrigin; }
			set { Sprite.XOrigin = value; }
		}

		public int YOrigin 
		{
			get { return Sprite.YOrigin; }
			set { Sprite.YOrigin = value; }
		}
	}
}
