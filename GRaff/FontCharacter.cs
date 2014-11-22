using System;

namespace GRaff
{
	public class FontCharacter
	{
	//	public Texture Texture { get; private set; }

		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int XOffset { get; set; }
		public int YOffset { get; set; }
		public int XAdvance { get; set; }

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