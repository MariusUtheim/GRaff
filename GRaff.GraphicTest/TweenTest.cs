using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;

namespace GRaff.GraphicTest
{
	[Test]
	class TweenTest : GameObject, IGlobalMouseListener, IKeyPressListener
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

		private static Dictionary<Key, string> functionNames = new Dictionary<Key, string>
		{
			{ Key.Number1, "Linear" },
			{ Key.Number2, "Quadratic" },
			{ Key.Number3, "Cubic" },
			{ Key.Number4, "Quartic" },
			{ Key.Number5, "Quintic" },
			{ Key.Number6, "Sine" },
			{ Key.Number7, "Circle" },
			{ Key.Number8, "Bounce" },
			{ Key.Number9, "Spring" },
			{ Key.Number0, "Elastic" }
		};

		private static Marker marker { get; set; }
		private TweeningFunction f = functions[Key.Number1];
		private Color _color = Colors.DarkRed;

		public override void OnDraw()
		{
			Draw.FillCircle(_color, Location, 8);
			Draw.Line(Colors.White, (Room.Current.Center.X, 0), (Room.Current.Center.X, Room.Current.Height));
		}

		public void OnGlobalMouse(MouseButton button)
		{
			_color = Colors.Red;

			marker = Instance<Marker>.Create(new Point(GRandom.Integer(Room.Current.Width), GRandom.Integer(Room.Current.Height)));

			if (button == MouseButton.Left)
				Tween.Animate(90, f, () => this.Location, Mouse.Location, () => _color = Colors.DarkRed);
			else if (button == MouseButton.Right)
				Tween.Animate(90, f.Out(), () => this.Location, Mouse.Location, () => _color = Colors.DarkRed);
		}

		public void OnKeyPress(Key key)
		{
			if (functions.ContainsKey(key))
			{
				Window.Title = functionNames[key];
				f = functions[key];
			}
		}
	}
}
