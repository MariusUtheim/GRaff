using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Background
	{
		static Background()
		{
			Color = Color.LightGray;
			Image = null;
		}

		public static Color Color { get; set; }

		public static Sprite Image { get; set; }

		public static bool Tiled { get; set; }

		public static double XOffset { get; set; }
		public static double YOffset { get; set; }
		public static Vector Offset
		{
			get { return new Vector(XOffset, YOffset); }
			set { XOffset = value.X; YOffset = value.Y; }
		}

		public static double HSpeed { get; set; }
		public static double VSpeed { get; set; }
		public static Vector Velocity
		{
			get { return new Vector(HSpeed, VSpeed); }
			set { HSpeed = value.X; VSpeed = value.Y; }
		}

		public static void Redraw()
		{
			Draw.Clear(Color);
			if (Image != null)
			{
				throw new NotImplementedException();
				if (Tiled)
				{
					int x0 = (int)(XOffset % Image.Width) - Image.Width,
						y0 = (int)(YOffset % Image.Height) - Image.Height;
					for (int x = x0; x < Room.Width; x += Image.Width)
						for (int y = y0; y < Room.Height; y += Image.Height)
							Draw.Sprite(x, y, Image, 0);
					Offset += Velocity;
				}
				else
					Draw.Sprite(0, 0, Image, 0);
			}
		}
	}
}
