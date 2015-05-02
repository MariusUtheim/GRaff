using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	class BackgroundTest : Test, IKeyPressListener
	{

		public BackgroundTest()
		{
			Background.Default.ClearColor = Colors.Orange;
			Background.Default.Buffer = TextureBuffers.Giraffe;
			Background.Default.IsTiled = true;
			Background.Default.HSpeed = -1;
			Background.Default.VSpeed = -1.1;
		}

		public override void OnDestroy()
		{
			Background.Default.Buffer = null;
		}

		public void OnKeyPress(Key key)
		{
			switch (key)
			{
				case Key.Number1:
					Background.Default.Velocity = Vector.Zero;
					Background.Default.Offset = Vector.Zero;
					Background.Default.IsTiled = false;
					break;

				case Key.Number2:
					Background.Default.HSpeed = -1;
					Background.Default.VSpeed = -1.1;
					Background.Default.IsTiled = true;
					break;
			}
		}
	}
}
