using System;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;


namespace GameMaker
{
	public static class Background
	{
		static Background()
		{
			Color = Color.LightGray;
			Sprite = null;
			DrawColor = true;
		}

		public static Color Color { get; set; }

		public static Sprite Sprite { get; set; }

		public static bool IsTiled { get; set; }

		public static bool DrawColor { get; set; }

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
			if (DrawColor)
				Draw.Clear(Color);

			if (Sprite != null)
			{
				if (IsTiled)
				{
					double uw = Room.Width / (double)Sprite.Width, vh = Room.Height / (double)Sprite.Height;
					double u0 = -XOffset / (double)Sprite.Width, v0 = -YOffset / (double)Sprite.Height;
					
					GL.Enable(EnableCap.Texture2D);
					GL.BindTexture(TextureTarget.Texture2D, Sprite.Texture.Id);
					throw new NotImplementedException(MethodInfo.GetCurrentMethod().Name + " is not implemented");
/*
					GL.Begin(PrimitiveType.Quads);
					GL.Color3(1.0f, 1.0f, 1.0f);
					{
						GL.TexCoord2(u0, v0);
						GL.Vertex2(0, 0);
						GL.TexCoord2(u0 + uw, v0);
						GL.Vertex2(Room.Width, 0);
						GL.TexCoord2(u0 + uw, v0 + vh);
						GL.Vertex2(Room.Width, Room.Height);
						GL.TexCoord2(u0, v0 + vh);
						GL.Vertex2(0, Room.Height);
					}
					GL.End();
					*/
					GL.Disable(EnableCap.Texture2D);
				}
				else
					Draw.Sprite(0, 0, Sprite, 0);
			}
		}
	}
}
