using GRaff.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Showcase
{
	static class Textures
	{
		public static Texture Giraffe;
        //public static TextureBuffer Animation = TextureBuffer.Load(@"C:\test\Animation.png");
        public static Texture Star;

		public static void LoadAll()
		{
            Giraffe = Texture.Load(@"Assets/Giraffe.jpg");
			Star = Texture.Load(@"Assets/Particle.png");
		}
	}
}
