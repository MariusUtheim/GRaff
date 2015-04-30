using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	static class TextureBuffers
	{
		public static TextureBuffer Giraffe = new TextureBuffer(@"C:\test\Giraffe.jpg");

		public static void LoadAll()
		{
			Giraffe.Load();
		}
	}
}
