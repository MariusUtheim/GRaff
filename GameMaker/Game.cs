using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameMaker
{
	public static class Game
	{
		internal static GameWindow Window { get; set; }

		public static void Run(int windowWidth, int windowHeight, Action gameStart = null)
		{
			throw new NotImplementedException();
		}


		[STAThread]
		public static void Run(Room initialRoom, double fps = 60, Action gameStart = null)
		{
			Time.StartTime = Time.MachineTime;
			Window = new GameWindow(initialRoom.GetWidth(), initialRoom.GetHeight());

			Window.UpdateFrame += (sender, e) => {
				try
				{
					Game.Loop();
				}
				catch (Exception err)
				{
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
				GL.LoadIdentity();

				Rectangle rect = View.ActualView;
				GL.Ortho(rect.Left, rect.Right, rect.Bottom, rect.Top, 0.0, 1.0);

				try
				{
					Game.Redraw();
				}
				catch (Exception err)
				{
					throw;
				}
				Window.SwapBuffers();
			};

			Window.Load += (sender, e) => {
				initialRoom.Enter();
				if (gameStart != null)
					gameStart();
				//	Window.VSync = VSyncMode.On;5
			};

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			Window.Run(fps, fps);
		}

		public static void Quit()
		{
			Window.Exit();
		}

		public static void Loop()
		{
			// Begin step
			// Alarm events
			// Keyboard, key press, key release
			// Mouse
			// Normal step 
			//  (MovingObjects will update their positions)
			// Collision events
			// End step
			// Draw events

			foreach (var instance in Instance._objects)
				instance.OnBeginStep();

			Alarm.TickAll();

			_handleInput();

			foreach (var instance in Instance._objects)
				instance.OnStep();
			GlobalEvent.OnStep();

			_detectCollisions();

			foreach (var instance in Instance._objects)
				instance.OnEndStep();
		}

		private static void _detectCollisions()
		{
			foreach (var gen in Instance.Where(obj => obj is ICollisionListener))
			{
				var interfaces = gen.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollisionListener<>));
				foreach (var collisionInterface in interfaces)
				{
					var arg = collisionInterface.GetGenericArguments().First();
					foreach (var other in Instance.Where(i => i.GetType() == arg || arg.IsSubclassOf(i.GetType())))
					{
						if ((gen as GameObject).Intersects(other))
							collisionInterface.GetMethods().First().Invoke(gen, new object[] { other });
					}
				}
			}
		}

		private static void _handleInput()
		{
			foreach (var key in Keyboard.Down)
				foreach (var instance in Instance.Where(obj => obj is IKeyListener))
					(instance as IKeyListener).OnKey(key);
			foreach (var key in Keyboard.Down)
				GlobalEvent.OnKey(key);

			foreach (var key in Keyboard.Pressed)
				foreach (var instance in Instance.Where(obj => obj is IKeyPressListener))
					(instance as IKeyPressListener).OnKeyPress(key);
			foreach (var key in Keyboard.Down)
				GlobalEvent.OnKeyPressed(key);

			foreach (var key in Keyboard.Released)
				foreach (var instance in Instance.Where(obj => obj is IKeyReleaseListener))
					(instance as IKeyReleaseListener).OnKeyRelease(key);
			foreach (var key in Keyboard.Released)
				GlobalEvent.OnKeyReleased(key);


			foreach (var button in Mouse.Down)
				foreach (var instance in Instance.Where(obj => obj is IMouseListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMouseListener).OnMouse(button);

			foreach (var button in Mouse.Pressed)
				foreach (var instance in Instance.Where(obj => obj is IMousePressListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMousePressListener).OnMousePress(button);

			foreach (var button in Mouse.Released)
				foreach (var instance in Instance.Where(obj => obj is IMouseReleaseListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMouseReleaseListener).OnMouseRelease(button);


			foreach (var button in Mouse.Down)
				foreach (var instance in Instance.Where(obj => obj is IGlobalMouseListener))
					(instance as IGlobalMouseListener).OnGlobalMouse(button);
			foreach (var button in Mouse.Down)
				GlobalEvent.OnMouse(button);

			foreach (var button in Mouse.Pressed)
				foreach (var instance in Instance.Where(obj => obj is IGlobalMousePressListener))
					(instance as IGlobalMousePressListener).OnGlobalMousePress(button);
			foreach (var button in Mouse.Pressed)
				GlobalEvent.OnMousePressed(button);

			foreach (var button in Mouse.Released)
				foreach (var instance in Instance.Where(obj => obj is IGlobalMouseReleaseListener))
					(instance as IGlobalMouseReleaseListener).OnGlobalMouseRelease(button);
			foreach (var button in Mouse.Released)
				GlobalEvent.OnMouseReleased(button);

			Keyboard.Update();
			Mouse.Update();
		}

		public static void Redraw()
		{
			Background.Redraw();

			GlobalEvent.OnDrawBackground();

			foreach (var instance in Instance._objects)
				instance.OnDraw();

			GlobalEvent.OnDrawForeground();
			Time.Frame();

		}

		public static void Sleep(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}

	}
}
