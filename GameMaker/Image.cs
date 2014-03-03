using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Image
	{
		public Image(Sprite sprite)
		{
			this.Sprite = sprite;
			this.XScale = 1;
			this.YScale = 1;
			this.Rotation = Angle.Zero;
			this.Blend = Color.White;
			this.Count = 1;
			this.Index = 0;
		}

		public double XScale { get; set; }
		public double YScale { get; set; }
		public Angle Rotation { get; set; }
		public Color Blend { get; set; }
		public Sprite Sprite { get; set; }

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
			_index += Speed;
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
