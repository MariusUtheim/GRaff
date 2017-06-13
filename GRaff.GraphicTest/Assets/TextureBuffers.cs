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
		public static TextureBuffer Giraffe;
        //public static TextureBuffer Animation = TextureBuffer.Load(@"C:\test\Animation.png");
        public static TextureBuffer Star;

		public static void LoadAll()
		{
            Giraffe = TextureBuffer.Load(@"Assets/Giraffe.jpg");
			Star = TextureBuffer.Load(@"Assets/Particle.png");
		}
	}
}
