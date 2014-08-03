using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameMaker
{

	public class GraphicsEngine
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

		GameWindow game;
		public GraphicsEngine()
		{


		}
		
		[STAThread]
		public void Run(Action gameStart)
		{
			using (game = new GameWindow(Room.Current.Width, Room.Current.Height))
			{
				game.Load += (sender, e) =>
				{
					gameStart();
					game.VSync = VSyncMode.On;
				};

				game.Resize += (sender, e) => { GL.Viewport(0, 0, game.Width, game.Height); };

				game.UpdateFrame += (sender, e) => { };

				game.KeyDown += (sender, e) => {
					//KeyDown(e.Key.ToGMKey());
				};
				//game.KeyUp += (sender, e) => { KeyUp(e.Key.ToGMKey()); };
				game.Mouse.Move += (sender, e) => { SetMouseLocation(e.X, e.Y); };
				//game.Mouse.ButtonDown += (sender, e) => { MouseDown(e.Button.ToGMMouseButton()); };
				//game.Mouse.ButtonUp += (sender, e) => { MouseUp(e.Button.ToGMMouseButton()); };
				

				game.RenderFrame += (sender, e) =>
				{
					Game.Loop();

					// render graphics
					GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

					GL.MatrixMode(MatrixMode.Projection);
					GL.LoadIdentity();
					GL.Ortho(0, game.Width, game.Height, 0, 0.0, 1.0);

					Game.Redraw();
				};

				game.Run(Room.Current.Speed);
			}
		}

		public void Refresh()
		{
			game.SwapBuffers();
		}

		public int Width
		{
			get
			{
				return game.Width;
			}
			set
			{
				game.Width = value;
			}
		}

		public int Height
		{
			get
			{
				return game.Height;
			}
			set
			{
				game.Height = value;
			}
		}

		public void Quit()
		{
			game.Exit();
		}

		public IntVector Size
		{
			get { return new IntVector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}
	}

}
