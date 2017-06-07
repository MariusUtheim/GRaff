using System;
using System.Linq;
using System.Threading;
using GRaff.Graphics;
using GRaff.Particles;
using GRaff.Synchronization;
using OpenTK;
using OpenTK.Graphics;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif

namespace GRaff
{
	/// <summary>
	/// A static class handling the overhead control of the game.
	/// </summary>
	public static class Giraffe
	{
		/// <summary>
		/// The OpenTK.GameWindow instance.
		/// </summary>
		internal static GameWindow Window { get; set; }
		public static bool IsRunning { get; private set; }

		public static void Run()
		{
			Run(1024, 768, 60.0, null);
		}

		public static void Run(int windowWidth, int windowHeight, Action gameStart)
		{
			Run(windowWidth, windowHeight, 60.0, gameStart);
		}

		/// <summary>
		/// Runs the game.
		/// </summary>
		/// <param name="initialRoom">The initial room that is entered when the game begins.</param>
		/// <param name="fps">The framerate at which the game runs. The default value is 60.</param>
		/// <param name="gameStart">An action that is performed when the game begins. If omitted or set to null, no action is performed.</param>
		public static void Run(int windowWidth, int windowHeight, double fps, Action gameStart)
		{
			Time.StartTime = Time.MachineTime;
			Window = new GameWindow(windowWidth, windowHeight, GraphicsMode.Default, "Giraffe", GameWindowFlags.Default, DisplayDevice.Default);
			Window.WindowBorder = WindowBorder.Fixed;

			Window.UpdateFrame += (sender, e) => Giraffe.Loop();
			Window.Closing += (sender, e) => IsRunning = false;

			Window.KeyDown += (sender, e) => { Keyboard.Press((Key)e.Key); };
			Window.KeyUp += (sender, e) => { Keyboard.Release((Key)e.Key); };
			Window.MouseMove += (sender, e) => { Mouse.WindowX = e.X; Mouse.WindowY = e.Y;  };
			Window.MouseDown += (sender, e) => { Mouse.Press((MouseButton)e.Button); };
			Window.MouseUp += (sender, e) => { Mouse.Release((MouseButton)e.Button); };
            Window.MouseWheel += (sender, e) => { Mouse.Wheel(e.ValuePrecise, e.DeltaPrecise); };

            Window.RenderFrame += (sender, e) => {

				GL.Clear(ClearBufferMask.ColorBufferBit);

				ShaderProgram.CurrentColored.UpdateUniformValues();
				ShaderProgram.CurrentTextured.UpdateUniformValues();

				Giraffe.Redraw();

				Window.SwapBuffers();
			};

			Window.Load += (sender, e) => {
				Async.MainThreadDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
				Draw.CurrentSurface = new Surface(windowWidth, windowHeight);

				var initialRoom = new Room(windowWidth, windowHeight);
				initialRoom._Enter();

				Graphics._Graphics.Initialize();
				Audio._Audio.Initialize();
				Window.VSync = VSyncMode.On;
				IsRunning = true; 
				
				/// ANY DEVELOPER LOGIC MAY COME AFTER THIS POINT

				if (gameStart != null)
					gameStart();
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
		/// Performs a game loop. This includes the following phases, in order:
		/// - Begin step
		/// - Handle queued asynchronous actions
		/// - Keyboard, key press, key release
		/// - Mouse, mouse press, mouse release
		/// - Step
		/// - Collision
		/// - End step
		/// </summary>
		public static void Loop()
		{

			GlobalEvent.OnBeginStep();
			Instance<GameObject>.Do(instance => instance.OnBeginStep());

			Async.HandleEvents();

			_handleInput();

			foreach (var instance in Instance.All)
				instance.OnStep();
			GlobalEvent.OnStep();

			_detectCollisions();

			Instance<GameObject>.Do(instance => instance.OnEndStep());
			GlobalEvent.OnEndStep();

			Instance.Sort();
		}

		private static void _detectCollisions()
		{
			foreach (var gen in Instance<GameObject>.Where(obj => obj is ICollisionListener))
			{
				var interfaces = gen.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollisionListener<>));
				foreach (var collisionInterface in interfaces)
				{
					var arg = collisionInterface.GetGenericArguments().First();
					foreach (var other in Instance<GameObject>.Where(i => i.GetType() == arg || arg.IsAssignableFrom(i.GetType())))
					{
						if (gen.Intersects(other))
							collisionInterface.GetMethods().First().Invoke(gen, new object[] { other });
					}
				}
			}
		}

		/*
		private static void _handleKeyInput<TSource>(IEnumerable<Key> keys, Action<TSource, KeyEventArgs> localAction, Action<KeyEventArgs> globalAction)
			where TSource : class
		{
			foreach (var e in keys.Select(key => new KeyEventArgs(key)))
			{
				if (e.IsCanceled)
					break;
				if (_handleKey(e, localAction))
					globalAction(e);
			}
		}

		private static bool _handleKey<TLocalSource>(KeyEventArgs e, Action<TLocalSource, KeyEventArgs> localAction)
			where TLocalSource : class
		{
			foreach (var instance in Instance.Elements.Where(obj => obj is TLocalSource).Select(obj => obj as TLocalSource))
            {
				if (e.IsCanceled)
					return false;
				localAction(instance, e);
			}
			return true;
		}
		*/

		private static void _handleInput()
		{
			foreach (var key in Keyboard.Down)
			{
				foreach (var instance in Instance.All.Where(obj => obj is IKeyListener))
					(instance as IKeyListener).OnKey(key);
				GlobalEvent.OnKey(key);
			}

			foreach (var key in Keyboard.Pressed)
			{
				Room.Current?.OnKeyPressed(key);
		        foreach (var instance in Instance.All.Where(obj => obj is IKeyPressListener))
					(instance as IKeyPressListener).OnKeyPress(key);
				GlobalEvent.OnKeyPressed(key);
			}

			foreach (var key in Keyboard.Released)
			{
				foreach (var instance in Instance.All.Where(obj => obj is IKeyReleaseListener))
					(instance as IKeyReleaseListener).OnKeyRelease(key);
				GlobalEvent.OnKeyReleased(key);
			}


			foreach (var button in Mouse.Down)
				foreach (var instance in Instance<GameObject>.Where(obj => obj is IMouseListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMouseListener).OnMouse(button);

			foreach (var button in Mouse.Pressed)
				foreach (var instance in Instance<GameObject>.Where(obj => obj is IMousePressListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMousePressListener).OnMousePress(button);

			foreach (var button in Mouse.Released)
				foreach (var instance in Instance<GameObject>.Where(obj => obj is IMouseReleaseListener && obj.Mask.ContainsPoint(Mouse.Location)))
					(instance as IMouseReleaseListener).OnMouseRelease(button);

            if (Mouse.WheelDelta != 0)
                foreach (var instance in Instance<GameObject>.Where(obj => obj is IMouseWheelListener && obj.Mask.ContainsPoint(Mouse.Location)))
                    (instance as IMouseWheelListener).OnMouseWheel(Mouse.WheelDelta);


			foreach (var button in Mouse.Down)
			{
				foreach (var instance in Instance.All.Where(obj => obj is IGlobalMouseListener))
					(instance as IGlobalMouseListener).OnGlobalMouse(button);
				GlobalEvent.OnMouse(button);
			}

			foreach (var button in Mouse.Pressed)
			{
				foreach (var instance in Instance.All.Where(obj => obj is IGlobalMousePressListener))
					(instance as IGlobalMousePressListener).OnGlobalMousePress(button);
				GlobalEvent.OnMousePressed(button);
			}

			foreach (var button in Mouse.Released)
			{
				foreach (var instance in Instance.All.Where(obj => obj is IGlobalMouseReleaseListener))
					(instance as IGlobalMouseReleaseListener).OnGlobalMouseRelease(button);
				GlobalEvent.OnMouseReleased(button);
			}

            if (Mouse.WheelDelta != 0)
            {
                foreach (var instance in Instance.All.Where(obj => obj is IGlobalMouseWheelListener))
                    (instance as IGlobalMouseWheelListener).OnGlobalMouseWheel(Mouse.WheelDelta);
                GlobalEvent.OnMouseWheel(Mouse.WheelDelta);
            }

			Keyboard.Update();
			Mouse.Update();
		}

		/// <summary>
		/// Repaints the screen.
		/// </summary>
		public static void Redraw()
		{
			GlobalEvent.OnDrawBackground();

			foreach (var instance in Instance<IVisible>.Where(element => element.IsVisible))
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
