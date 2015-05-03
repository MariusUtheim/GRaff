using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;

namespace GRaff.GraphicTest
{
	class TweenTest : Test, IGlobalMouseListener, IKeyPressListener
	{
		private static Dictionary<Key, TweeningFunction> functions = new Dictionary<Key, TweeningFunction>
		{
            { Key.Number1, Tween.Linear },
            { Key.Number2, Tween.Quadratic },
			{ Key.Number3, Tween.Cubic },
			{ Key.Number4, Tween.Quartic },
			{ Key.Number5, Tween.Quintic },
			{ Key.Number6, Tween.Sine },
			{ Key.Number7, Tween.Circle },
			{ Key.Number8, Tween.Bounce(3) },
			{ Key.Number9, Tween.Spring(3, 4) },
			{ Key.Number0, Tween.Elastic(3, 4) }
		};

		private static Marker marker { get; set; }
		private TweeningFunction f = functions[Key.Number1];
		private Color _color = Colors.DarkRed;

		public TweenTest()
		{
			Background.Default.ClearColor = null;
		}

		public override void OnDraw()
		{
			Draw.FillCircle(_color, Location, 8);
			Draw.Line(Colors.White, Room.Center.X, 0, Room.Center.X, Room.Height);
		}

		public void OnGlobalMouse(MouseButton button)
		{
			_color = Colors.Red;

			marker = Instance<Marker>.Create(new Point(GRandom.Integer(Room.Width), GRandom.Integer(Room.Height)));

			Tween.Animate(() => this.Location, Mouse.Location, 90, f, () => _color = Colors.DarkRed);
		}

		public void OnKeyPress(Key key)
		{
			if (functions.ContainsKey(key))
			{
				Window.Title = Enum.GetName(typeof(Key), key);
				f = functions[key];
			}
		}
	}
}
