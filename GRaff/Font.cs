using System;

namespace GRaff
{
	public class Font
	{
		public TextureBuffer Texture { get; private set; }

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