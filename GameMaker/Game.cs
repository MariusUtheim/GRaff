using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace GRaff
{
	/// <summary>
	/// A static class handling the overhead control of the game.
	/// </summary>
	public static class Game
	{
		/// <summary>
		/// The OpenTK.GameWindow instance.
		/// </summary>
		internal static GameWindow Window { get; set; }

		/// <summary>
		/// Runs the game.
		/// </summary>
		/// <param name="initialRoom">The initial room that is entered when the game begins.</param>
		/// <param name="fps">The framerate at which the game runs. The default value is 60.</param>
		/// <param name="gameStart">An action that is performed when the game begins. If omitted or set to null, no action is performed.</param>
		[STAThread]
#warning CA1026
		public static void Run(string[] args, Room initialRoom, double fps = 60, Action gameStart = null)
		{
			Time.StartTime = Time.MachineTime;
			Window = new GameWindow(initialRoom.GetWidth(), initialRoom.GetHeight());
			Window.WindowBorder = WindowBorder.Fixed;

			Window.UpdateFrame += (sender, e) => {
				try
				{
					Game.Loop();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					throw;
				}
			};

			Window.KeyDown += (sender, e) => { Keyboard.Press((Key)e.Key); };
			Window.KeyUp += (sender, e) => { Keyboard.Release((Key)e.Key); };
			Window.Mouse.Move += (sender, e) => { Mouse.WindowX = e.X; Mouse.WindowY = e.Y; };
			Window.Mouse.ButtonDown += (sender, e) => { Mouse.Press((MouseButton)e.Button); };
			Window.Mouse.ButtonUp += (sender, e) => { Mouse.Release((MouseButton)e.Button); };

			Window.RenderFrame += (sender, e) => {

				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

				View.LoadMatrix();

				Game.Redraw();

				Window.SwapBuffers();
			};

			Window.Load += (sender, e) => {
				initialRoom.Enter();
				if (gameStart != null)
					gameStart();
				Window.VSync = VSyncMode.On;
				GL.Enable(EnableCap.Blend);
				GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			};

			Window.Run(fps, fps);
		}

		/// <summary>
		/// Quits the game.
		/// </summary>
		public static void Quit()
		{
			Window.Exit();
		}

		/// <summary>
		/// Performs a game loop. This includes the following events, in order:
		/// - Handle async exceptions
		/// - Begin step
		/// - Keyboard, key press, key release
		/// - Mouse, mouse press, mouse release
		/// - Step
		/// - Collisions
		/// - End step
		/// </summary>
		public static void Loop()
		{
			GlobalEvent.OnAsyncException();

			GlobalEvent.OnBeginStep();
			foreach (var instance in Instance.Objects)
				instance.OnBeginStep();

			_handleInput();

			foreach (var instance in Instance.Elements)
				instance.OnStep();
			GlobalEvent.OnStep();

			_detectCollisions();
			
			foreach (var instance in Instance.Objects)
				instance.OnEndStep();
			GlobalEvent.OnEndStep();

			if (Instance.NeedsSort)
				Instance.Sort();
		}

		private static void _detectCollisions()
		{
			foreach (var gen in Instance.Objects.Where(obj => obj is ICollisionListener))
			{
				var interfaces = gen.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollisionListener<>));
				foreach (var collisionInterface in interfaces)
				{
					var arg = collisionInterface.GetGenericArguments().First();
					foreach (var other in Instance.Objects.Where(i => i.GetType() == arg || arg.IsSubclassOf(i.GetType())))
					{
						if ((gen as GameObject).Intersects(other))
							collisionInterface.GetMethods().First().Invoke(gen, new object[] { other });
					}
				}
			}
		}

#warning CA1502
		private static void _handleInput()
		{
			foreach (var key in Keyboard.Down)
				foreach (var instance in Instance.Elements.Where(obj => obj is IKeyListener))
					(instance as IKeyListener).OnKey(key);
			foreach (var key in Keyboard.Down)
				GlobalEvent.OnKey(key);

			foreach (var key in Keyboard.Pressed)
				foreach (var instance in Instance.Elements.Where(obj => obj is IKeyPressListener))
					(instance as IKeyPressListener).OnKeyPress(key);
			foreach (var key in Keyboard.Down)
				GlobalEvent.OnKeyPressed(key);

			foreach (var key in Keyboard.Released)
				foreach (var instance in Instance.Elements.Where(obj => obj is IKeyReleaseListener))
					(instance as IKeyReleaseListener).OnKeyRelease(key);
			foreach (var key in Keyboard.Released)
				GlobalEvent.OnKeyReleased(key);


			foreach (var button in Mouse.Down)
				foreach (var instance in Instance.Objects.Where(obj => obj is IMouseListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMouseListener).OnMouse(button);

			foreach (var button in Mouse.Pressed)
				foreach (var instance in Instance.Objects.Where(obj => obj is IMousePressListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMousePressListener).OnMousePress(button);

			foreach (var button in Mouse.Released)
				foreach (var instance in Instance.Objects.Where(obj => obj is IMouseReleaseListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMouseReleaseListener).OnMouseRelease(button);


			foreach (var button in Mouse.Down)
				foreach (var instance in Instance.Elements.Where(obj => obj is IGlobalMouseListener))
					(instance as IGlobalMouseListener).OnGlobalMouse(button);
			foreach (var button in Mouse.Down)
				GlobalEvent.OnMouse(button);

			foreach (var button in Mouse.Pressed)
				foreach (var instance in Instance.Elements.Where(obj => obj is IGlobalMousePressListener))
					(instance as IGlobalMousePressListener).OnGlobalMousePress(button);
			foreach (var button in Mouse.Pressed)
				GlobalEvent.OnMousePressed(button);

			foreach (var button in Mouse.Released)
				foreach (var instance in Instance.Elements.Where(obj => obj is IGlobalMouseReleaseListener))
					(instance as IGlobalMouseReleaseListener).OnGlobalMouseRelease(button);
			foreach (var button in Mouse.Released)
				GlobalEvent.OnMouseReleased(button);

			Keyboard.Update();
			Mouse.Update();
		}

		/// <summary>
		/// Repaints the screen.
		/// </summary>
		public static void Redraw()
		{
		//	Draw.Clean();
			Background.Redraw();

			GlobalEvent.OnDrawBackground();

			foreach (var instance in Instance.Elements)
				instance.OnDraw();

			GlobalEvent.OnDrawForeground();
			Time.Frame();
		}

		/// <summary>
		/// Freezes the game for the specified number of milliseconds.
		/// </summary>
		/// <param name="milliseconds">How long to sleep.</param>
		public static void Sleep(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}

	}
}
