using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	[Test]
	class BackgroundTest : GameElement, IKeyPressListener
	{

		public BackgroundTest()
		{
			Room.Current.Background.Color = Colors.Orange;
			Room.Current.Background.Buffer = TextureBuffers.Giraffe;
			Room.Current.Background.IsTiled = true;
			Room.Current.Background.HSpeed = -1;
			Room.Current.Background.VSpeed = -1.1;
		}

		public override void OnDestroy()
		{
			Room.Current.Background.Buffer = null;
		}

		public void OnKeyPress(Key key)
		{
			switch (key)
			{
				case Key.Number1:
					Room.Current.Background.Velocity = Vector.Zero;
					Room.Current.Background.Offset = Vector.Zero;
					Room.Current.Background.IsTiled = false;
					break;

				case Key.Number2:
					Room.Current.Background.HSpeed = -1;
					Room.Current.Background.VSpeed = -1.1;
					Room.Current.Background.IsTiled = true;
					break;
			}
		}
	}
}
