using System;

namespace GRaff
{
	public class FontCharacter
	{
		public Texture Texture { get; private set; }

		public int X { get; private set; }
		public int Y { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int XOffset { get; private set; }
		public int YOffset { get; private set; }

		public double Height { get; private set; }

		public static double GetWidth(char c)
		{
			throw new NotImplementedException();
		}

		internal static void PushTexCoords(char c, ref int t, ref Point[] texCoords)
		{
#warning TODO: Remove this ugly method
			throw new NotImplementedException();
		}
	}
}