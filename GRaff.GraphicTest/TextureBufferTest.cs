using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	[Test]
	class TextureBufferTest : GameElement
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
