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

		public override Texture LoadTexture(string file)
		{
			return new OpenGLTexture(file);
		}

		public override void DrawImage(double x, double y, Image image)
		{
			double w = image.Sprite.Width, h = image.Sprite.Height;
			x -= image.XOrigin;
			y -= image.YOrigin;
			float fx = (float)x, fy = (float)y;
			GL.Color3(image.Blend.ToGLColor());
			GL.BindTexture(TextureTarget.Texture2D, (image.CurrentTexture as OpenGLTexture).Id);
			GL.Enable(EnableCap.Texture2D);

			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(x, y);
			GL.TexCoord2(0, 1); GL.Vertex2(x, y + h);
			GL.TexCoord2(1, 1); GL.Vertex2(x + w, y + h);
			GL.TexCoord2(1, 0); GL.Vertex2(x + w, y);
			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}

		public override void DrawTexture(double x, double y, Texture texture)
		{
			GL.BindTexture(TextureTarget.Texture2D, (texture as OpenGLTexture).Id);
			GL.Enable(EnableCap.Texture2D);

			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(x, y);
			GL.TexCoord2(0, 1); GL.Vertex2(x, y + texture.Height);
			GL.TexCoord2(1, 1); GL.Vertex2(x + texture.Width, y + texture.Height);
			GL.TexCoord2(1, 0); GL.Vertex2(x + texture.Width, y);
			GL.End();

			GL.Disable(EnableCap.Texture2D);
		}

		public override void DrawCircle(Color color, Point location, double radius)
		{
			throw new NotImplementedException();
		}

		public override void DrawRectangle(Color color, double x, double y, double width, double height)
		{
			//GL.Ortho(x, y, x + width, y + height, 0, 1);
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(color.ToGLColor());
			GL.Vertex2(x, y);
			GL.Vertex2(x, y + height);
			GL.Vertex2(x + width, y + height);
			GL.Vertex2(x + width, y);
			GL.End();
		}

		public override void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			//GL.Ortho(x, y, x + width, y + height, 0, 1);
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(col1.ToGLColor());
			GL.Vertex2(x, y);
			GL.Color3(col2.ToGLColor());
			GL.Vertex2(x, y + height);
			GL.Color3(col3.ToGLColor());
			GL.Vertex2(x + width, y + height);
			GL.Color3(col4.ToGLColor());
			GL.Vertex2(x + width, y);
			GL.End();
		}

		public override void DrawLine(Color color, double x1, double y1, double x2, double y2)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Color3(color.ToGLColor());
			GL.Vertex2(x1, y1);
			GL.Vertex2(x2, y2);
			GL.End();
		}

		public override void DrawLine(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			GL.Begin(PrimitiveType.Lines);
			GL.Color3(col1.ToGLColor());
			GL.Vertex2(x1, y1);
			GL.Color3(col2.ToGLColor());
			GL.Vertex2(x2, y2);
			GL.End();
		}

		public override void Clear(Color color)
		{
			GL.ClearColor(color.ToGLColor());
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
