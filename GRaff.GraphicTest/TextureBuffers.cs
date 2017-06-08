using GRaff.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	static class TextureBuffers
	{
		public static TextureBuffer Giraffe = TextureBuffer.Load(@"C:\test\Giraffe.jpg");
		//public static TextureBuffer Animation = TextureBuffer.Load(@"C:\test\Animation.png");
		public static TextureBuffer Star = TextureBuffer.Load(@"C:\test\particle.png");

		public static void LoadAll()
		{
			//Giraffe.Load();
			//Animation.Load();
			//Star.Load();
		}
	}
}
