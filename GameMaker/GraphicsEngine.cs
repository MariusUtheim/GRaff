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

		public abstract void Run(Action gameStart);

		public abstract Texture LoadTexture(string file);

		public abstract Surface CreateSurface(int width, int height);

		public abstract void Refresh();

		public abstract bool IsFullscren { get; set; }

		public abstract bool IsBorderVisible { get; set; }

		public abstract bool IsOnTop { get; set; }

		public abstract bool IsResizable { get; set; }

		public abstract string Title { get; set; }

		public abstract int Width { get; set; }

		public abstract int Height { get; set; }

		public IntVector Size
		{
			get { return new IntVector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}

		public abstract void Quit();

	}

}
