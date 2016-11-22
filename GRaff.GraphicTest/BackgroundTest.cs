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
		private Background background;

		public BackgroundTest()
		{
			background = Instance.Create(new Background
			{
				Color = Colors.LightGray,
				Buffer = TextureBuffers.Giraffe,
				IsTiled = true,
				HSpeed = -1,
				VSpeed = -1.1,
			});
		}

		public override void OnDestroy()
		{
			background.Buffer = null;
		}

		public void OnKeyPress(Key key)
		{
			switch (key)
			{
				case Key.Number1:
					background.Velocity = Vector.Zero;
					background.Offset = Vector.Zero;
					background.IsTiled = false;
					break;

				case Key.Number2:
					background.HSpeed = -1;
					background.VSpeed = -1.1;
					background.IsTiled = true;
					break;
			}
		}
	}
}
