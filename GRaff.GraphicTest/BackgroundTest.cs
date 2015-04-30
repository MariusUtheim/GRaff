using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	class BackgroundTest : Test
	{

		public BackgroundTest()
		{
			Background.Default.Buffer = TextureBuffers.Giraffe;
			Background.Default.IsTiled = true;
			Background.Default.HSpeed = -1;
			Background.Default.VSpeed = -1.1;
		}

		public override void OnDestroy()
		{
			Background.Default.Buffer = null;
		}
	}
}
