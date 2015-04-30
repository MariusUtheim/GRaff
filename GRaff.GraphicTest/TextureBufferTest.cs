using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	class TextureBufferTest : Test
	{
		
		public TextureBufferTest()
		{
		}

		public override void OnDraw()
		{
			Draw.Texture(TextureBuffers.Giraffe.Texture, 0, 0);
		}
	}
}
