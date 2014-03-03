using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Window
	{
		public static bool IsVisible
		{
			get { return GraphicsEngine.Current.IsVisible; }
			set { GraphicsEngine.Current.IsVisible = value; }
		}

		public static bool IsFullscreen
		{
			get { return GraphicsEngine.Current.IsFullscren; }
			set { GraphicsEngine.Current.IsFullscren = value; }
		}

		public static bool IsBorderVisible
		{
			get { return GraphicsEngine.Current.IsBorderVisible; }
			set { GraphicsEngine.Current.IsBorderVisible = value; }
		}

		public static bool IsOnTop
		{
			get { return GraphicsEngine.Current.IsOnTop; }
			set { GraphicsEngine.Current.IsOnTop = value; }
		}

		public static bool IsResizable
		{
			get { return GraphicsEngine.Current.IsResizable; }
			set { GraphicsEngine.Current.IsResizable = value; }
		}

		public static string Title
		{
			get { return GraphicsEngine.Current.Title; }
			set { GraphicsEngine.Current.Title = value; }
		}

		public static int Width
		{
			get { return GraphicsEngine.Current.Width; }
			set { GraphicsEngine.Current.Width = value; }
		}

		public static int Height
		{
			get { return GraphicsEngine.Current.Height; }
			set { GraphicsEngine.Current.Height = value; }
		}

	}
}
