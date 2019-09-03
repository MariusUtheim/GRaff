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
        public TweenTest()
        {
            OnKeyPress(Key.Number1);
        }

		private static Dictionary<Key, TweenFunction> functions = new Dictionary<Key, TweenFunction>
		{
            { Key.Number1, TweenFunctions.Linear },
            { Key.Number2, TweenFunctions.Quadratic },
			{ Key.Number3, TweenFunctions.Cubic },
			{ Key.Number4, TweenFunctions.Quartic },
			{ Key.Number5, TweenFunctions.Quintic },
			{ Key.Number6, TweenFunctions.Sine },
			{ Key.Number7, TweenFunctions.Circle },
			{ Key.Number8, TweenFunctions.Bounce(3) },
			{ Key.Number9, TweenFunctions.Spring(3, 4) },
			{ Key.Number0, TweenFunctions.Elastic(3, 4) }
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

		private static Marker _marker { get; set; }
		private TweenFunction f = functions[Key.Number1];
		private Color _color = Colors.DarkRed;

		public override void OnDraw()
		{
            Draw.Clear(Colors.LightGray);
			Draw.FillCircle(Location, 8, _color);
			Draw.Line((Window.Center.X, 0), (Window.Center.X, Window.Height), Colors.Black);
            Draw.Line((0, Window.Center.Y), (Window.Width, Window.Center.Y), Colors.Black);
		}

		public void OnGlobalMouse(MouseButton button)
		{
			_color = Colors.Red;

			_marker = Instance<Marker>.Create(new Point(GRandom.Integer(Window.Width), GRandom.Integer(Window.Height)));

			if (button == MouseButton.Left)
				Tween.Animate(f, 90, () => this.Location, Mouse.ViewLocation, () => _color = Colors.DarkRed);
			else if (button == MouseButton.Right)
				Tween.Animate(f.Inverse(), 90, () => this.Location, Mouse.ViewLocation, () => _color = Colors.DarkRed);
		}

		public void OnKeyPress(Key key)
		{
			if (functions.ContainsKey(key))
			{
				Window.Title = $"TweenTest - Fps: {Time.Fps} - {functionNames[key]}";
				f = functions[key];
			}
		}
	}
}
