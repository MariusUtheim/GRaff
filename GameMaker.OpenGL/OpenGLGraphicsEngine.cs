using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace GameMaker.OpenGL
{
    public class OpenGLGraphicsEngine : GraphicsEngine
    {
		GameWindow game;
		public OpenGLGraphicsEngine()
		{


		}
		
		[STAThread]
		public override void Run(Action gameStart)
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
					KeyDown(e.Key.ToGMKey());
				};
				game.KeyUp += (sender, e) => { KeyUp(e.Key.ToGMKey()); };
				game.Mouse.Move += (sender, e) => { SetMouseLocation(e.X, e.Y); };
				game.Mouse.ButtonDown += (sender, e) => { MouseDown(e.Button.ToGMMouseButton()); };
				game.Mouse.ButtonUp += (sender, e) => { MouseUp(e.Button.ToGMMouseButton()); };
				

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

		public override Texture[] LoadTexture(string file, int subimages)
		{
			if (subimages == 1)
				return new[] { new OpenGLTexture(file) };
			else
				throw new NotImplementedException("The OpenGL engine does not yet support opening textures with multiple subimages.");
		}

		public override Surface CreateSurface(int width, int height)
		{
			return new OpenGLSurface(width, height);
		}

#warning Should override something?
		public /*override*/ bool IsVisible
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool IsFullscren
		{
			get
			{
				return game.WindowState == WindowState.Fullscreen;
			}
			set
			{
#warning The WindowState should be returned to its previous value
				game.WindowState = value ? WindowState.Fullscreen : WindowState.Normal;
			}
		}

		public override void Refresh()
		{
			game.SwapBuffers();
		}

		public override bool IsBorderVisible
		{
			get
			{
				return game.WindowBorder != WindowBorder.Hidden;
			}
			set
			{
				game.WindowBorder = value ? WindowBorder.Fixed : WindowBorder.Hidden;
			}
		}

		public override bool IsOnTop
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool IsResizable
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override string Title
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override int Width
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

		public override int Height
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

		public override void Quit()
		{
			game.Exit();
		}
	}
}
