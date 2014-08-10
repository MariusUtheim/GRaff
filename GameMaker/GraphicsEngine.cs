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
					if (gameStart != null)
						gameStart();
					game.VSync = VSyncMode.On;
				};

				game.Resize += (sender, e) => { GL.Viewport(0, 0, game.Width, game.Height); };

				game.UpdateFrame += (sender, e) => {
					Game.Loop();
				};

				game.KeyDown += (sender, e) => { Keyboard.Press((Key)e.Key); };
				game.KeyUp += (sender, e) => { Keyboard.Release((Key)e.Key); };
				game.Mouse.Move += (sender, e) => { Mouse.X = e.X; Mouse.Y = e.Y; };
				game.Mouse.ButtonDown += (sender, e) => { Mouse.Press((MouseButton)e.Button); };
				game.Mouse.ButtonUp += (sender, e) => { Mouse.Release((MouseButton)e.Button); };

				game.RenderFrame += (sender, e) =>
				{

					// render graphics
					GL.Clear(ClearBufferMask.ColorBufferBit);

				//	GL.MatrixMode(MatrixMode.Projection);
					GL.LoadIdentity();
					GL.Ortho(0, game.Width, game.Height, 0, 0.0, 1.0);

					Game.Redraw();
					Refresh();
				};

				game.Run(30, 30);
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

		public bool IsBorderVisible
		{
			get { return game.WindowBorder == WindowBorder.Fixed; }

			set { game.WindowBorder = value ? WindowBorder.Fixed : WindowBorder.Hidden; }
		}

		public string Title
		{
			get { return game.Title; }
			set { game.Title = value; }
		}
	}

}
