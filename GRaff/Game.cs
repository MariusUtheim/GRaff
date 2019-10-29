using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GRaff.Synchronization;
using OpenTK;
using OpenTK.Graphics;

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
        internal static GameWindow? Window { get; private set; }
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
        public static void Run(int windowWidth, int windowHeight, double fps, Action? gameStart)
        {
            Time.StartTime = Time.MachineTime;
            Window = new GameWindow(windowWidth, windowHeight, GraphicsMode.Default, "Giraffe", GameWindowFlags.FixedWindow, DisplayDevice.Default, 4, 2, GraphicsContextFlags.ForwardCompatible);
            GRaff.Window.DisplayScale = new Vector(Window.Width / (double)windowWidth, Window.Height / (double)windowHeight);

            Window.UpdateFrame += (sender, e) => Game.Loop();
            Window.Closing += (sender, e) => IsRunning = false;
            Window.Closed += (sender, e) => Game.Window = null;

            Window.KeyDown += (sender, e) => { Keyboard.Press((Key)e.Key); };
            Window.KeyUp += (sender, e) => { Keyboard.Release((Key)e.Key); };
            Window.MouseMove += (sender, e) => { Mouse.X = (int)(e.X / GRaff.Window.DisplayScale.X); Mouse.Y = (int)(e.Y / GRaff.Window.DisplayScale.Y); };
            Window.MouseDown += (sender, e) => { Mouse.Press((MouseButton)e.Button); };
            Window.MouseUp += (sender, e) => { Mouse.Release((MouseButton)e.Button); };
            Window.MouseWheel += (sender, e) => { Mouse.Wheel(e.ValuePrecise, e.DeltaPrecise); };

            Window.RenderFrame += (sender, e) => {

                Game.Render();
                Window.SwapBuffers();
            };

            Window.Load += (sender, e) => {

                Graphics._Graphics.Initialize();
                Audio._Audio.Initialize();
                Window.VSync = VSyncMode.On;
                View.FullWindow().Bind();
                IsRunning = true;

                /// ANY DEVELOPER LOGIC MAY COME AFTER THIS POINT

                gameStart?.Invoke();
            };


            Window.Run(fps, fps);
        }


        /// <summary>
        /// Quits the game.
        /// </summary>
        public static void Quit()
        {
            if (Window == null)
                throw new InvalidOperationException("The game window does not exist.");
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
            Time.Loop();

            GlobalEvent.OnBeginStep();
            _do<GameObject>(obj => obj.OnBeginStep());

            Async.HandleEvents();

            _handleInput();

			GlobalEvent.OnStep();
            _do(obj => obj.OnStep());

            _detectCollisions();

			GlobalEvent.OnEndStep();
            _do<GameObject>(obj => obj.OnEndStep());

            Instance.Sort();
        }

        private static void _detectCollisions()
        {
            foreach (var gen in Instance<GameObject>.Where(obj => obj is ICollisionListener).ToList())
            {
                var interfaces = gen.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollisionListener<>));
                foreach (var collisionInterface in interfaces)
                {
                    var arg = collisionInterface.GetGenericArguments().First();
                    foreach (var other in Instance<GameObject>.Where(i => i.GetType() == arg || arg.IsAssignableFrom(i.GetType())).ToList())
                    {
                        if (gen.Intersects(other))
                            collisionInterface.GetMethods().First().Invoke(gen, new object[] { other });
                    }
                }
            }
        }

        private static void _do(Action<GameElement> action)
        {
            foreach (var instance in Instance.All)
                if (instance.Exists)
                    action(instance);
        }

        private static void _do<T>(Action<T> action) where T : class
        {
#warning Handle this more efficiently
            foreach (dynamic instance in Instance.OfType<T>().ToList())
                if (instance.Exists)
                    action(instance);
        }

        private static void _doMouseClick<T>(Action<T> action) where T : class
        {
            foreach (var instance in Instance.OfType<GameObject>().Where(obj => obj is T && obj.ContainsPoint(Mouse.ViewLocation)).ToList())
                if (instance.Exists && instance is T t)
                    action(t);
        }

        private static void _handleInput()
        {
            foreach (var key in Keyboard.Down)
            {
				GlobalEvent.OnKey(key);
                _do<IKeyListener>(i => i.OnKey(key));
            }

            foreach (var key in Keyboard.Pressed)
            {
				GlobalEvent.OnKeyPressed(key);
                _do<IKeyPressListener>(i => i.OnKeyPress(key));
            }

            foreach (var key in Keyboard.Released)
            {
				GlobalEvent.OnKeyReleased(key);
                _do<IKeyReleaseListener>(i => i.OnKeyRelease(key));
            }
            
            foreach (var button in Mouse.Down)
                _doMouseClick<IMouseListener>(i => i.OnMouse(button));

            foreach (var button in Mouse.Pressed)
                _doMouseClick<IMousePressListener>(i => i.OnMousePress(button));

            foreach (var button in Mouse.Released)
                _doMouseClick<IMouseReleaseListener>(i => i.OnMouseRelease(button));

            if (Mouse.WheelDelta != 0)
                _doMouseClick<IMouseWheelListener>(i => i.OnMouseWheel(Mouse.WheelDelta));
            
            foreach (var button in Mouse.Down)
            {
                _do<IGlobalMouseListener>(i => i.OnGlobalMouse(button));
                GlobalEvent.OnMouse(button);
            }

            foreach (var button in Mouse.Pressed)
            {
                _do<IGlobalMousePressListener>(i => i.OnGlobalMousePress(button));
                GlobalEvent.OnMousePressed(button);
            }

            foreach (var button in Mouse.Released)
            {
                _do<IGlobalMouseReleaseListener>(i => i.OnGlobalMouseRelease(button));
                GlobalEvent.OnMouseReleased(button);
            }

            if (Mouse.WheelDelta != 0)
            {
                _do<IGlobalMouseWheelListener>(i => i.OnGlobalMouseWheel(Mouse.WheelDelta));
                GlobalEvent.OnMouseWheel(Mouse.WheelDelta);
            }

            Keyboard.Update();
            Mouse.Update();
        }

        /// <summary>
        /// Repaints the screen. This performs the following actions in order:
        /// - GlobalEvent.DrawBackground events are raised
        /// - OnDrawBackground is called for the current room
        /// - OnDraw is called for each object, sorted according to Depth descending
        /// - OnDrawForeground is called for the current room
        /// - GlobalEvent.DrawForeground events are raised
        /// </summary>
        public static void Render()
        {
            View.Validate();

            GlobalEvent.OnDrawBackground();

            foreach (var instance in Instance.All)
                instance.OnDraw();

            GlobalEvent.OnDrawForeground();
            Time.UpdateFps();
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
