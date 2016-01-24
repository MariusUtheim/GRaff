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

		private static Marker marker { get; set; }
		private TweeningFunction f = functions[Key.Number1];
		private Color _color = Colors.DarkRed;

		public TweenTest()
		{
			Room.Current.Background.Color = null;
		}

		public override void OnDraw()
		{
			Draw.FillCircle(_color, Location, 8);
			Draw.Line(Colors.White, Room.Current.Center.X, 0, Room.Current.Center.X, Room.Current.Height);
		}

		public void OnGlobalMouse(MouseButton button)
		{
			_color = Colors.Red;

			marker = Instance<Marker>.Create(new Point(GRandom.Integer(Room.Current.Width), GRandom.Integer(Room.Current.Height)));

			Tween.Animate(90, f, () => this.Location, Mouse.Location, () => _color = Colors.DarkRed);
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
