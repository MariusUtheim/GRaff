using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public class Texture
	{
		internal int Id { get; private set; }
		public int Width
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		public int Height
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public static Texture[] LoadTexture(string file, int subimages)
		{
			if (subimages == 1)
				throw new NotImplementedException(); //return new[] { new OpenGLTexture(file) };
			else
				throw new NotImplementedException("The OpenGL engine does not yet support opening textures with multiple subimages.");
		}

	}
}
