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
				Color = Colors.Bisque,
				Texture = Textures.Giraffe,
				IsRepeated = true
			});
            Window.Title = "BackgroundTest - Tiled background";
        }

		protected override void OnDestroy()
		{
			background.Texture = null;
		}

		public void OnKeyPress(Key key)
		{
			switch (key)
			{
                case Key.Number1:
					background.Velocity = Vector.Zero;
					background.Offset = Vector.Zero;
					background.IsRepeated = true;
                    background.Texture = Textures.Giraffe;
                    Window.Title = "BackgroundTest - Tiled background";
					break;

                case Key.Number2:
                    background.Velocity = Vector.Zero;
                    background.Offset = Vector.Zero;
                    background.IsRepeated = false;
                    background.Texture = Textures.Giraffe;
                    Window.Title = "BackgroundTest - Static background";
                    break;

                case Key.Number3:
					background.HSpeed = -1;
					background.VSpeed = -1.1;
					background.IsRepeated = true;
                    background.Texture = Textures.Giraffe;
                    Window.Title = "BackgroundTest - Moving background";
					break;

                case Key.Number4:
                    background.Velocity = Vector.Zero;
                    background.Texture = null;
                    Window.Title = "BackgroundTest - Colored";
                    break;
			}
		}

    }
}
