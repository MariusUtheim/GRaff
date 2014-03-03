using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{

	public abstract class GraphicsEngine
	{
		internal static GraphicsEngine Current { get; set; }

		protected void SetMouseLocation(double x, double y)
		{
			Mouse.X = x;
			Mouse.Y = y;
		}

		protected void MouseDown(MouseButton button)
		{
			Mouse.Press(button);
		}

		protected void MouseUp(MouseButton button)
		{
			Mouse.Release(button);
		}

		protected void KeyDown(Key key)
		{
			Keyboard.Press(key);
		}

		protected void KeyUp(Key key)
		{
			Keyboard.Release(key);
		}

		public abstract void Run();

		public abstract Texture LoadTexture(string file);

		public abstract void DrawImage(double x, double y, Image image);

		public abstract void DrawRectangle(Color color, double x, double y, double width, double height);

		public abstract void DrawLine(Color color, double x1, double y1, double x2, double y2);

		public abstract void DrawLine(Color col1, Color col2, double x1, double y1, double x2, double y2);

		public abstract void Clear(Color color);

		public abstract bool IsVisible { get; set; }

		public abstract bool IsFullscren { get; set; }

		public abstract bool IsBorderVisible { get; set; }

		public abstract bool IsOnTop { get; set; }

		public abstract bool IsResizable { get; set; }

		public abstract string Title { get; set; }

		public abstract int Width { get; set; }

		public abstract int Height { get; set; }

		public abstract void Quit();
	}

}
