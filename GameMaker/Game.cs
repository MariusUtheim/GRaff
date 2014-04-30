﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Game
	{
#warning Remove this one - should specify initial room (or do something really clever)
		public static void Run<TGraphicsEngine>(Action gameStart = null)
			where TGraphicsEngine : GraphicsEngine, new()
		{
			Run<TGraphicsEngine>(new Room(1024, 768, null), gameStart);
		}

		public static void Run<TGraphicsEngine>(Room initialRoom)
			where TGraphicsEngine : GraphicsEngine, new()
		{
			Run<TGraphicsEngine>(initialRoom, null);
		}

		public static void Run<TGraphicsEngine>(Room initialRoom, Action gameStart)
			where TGraphicsEngine : GraphicsEngine, new()
		{
			GraphicsEngine.Current = new TGraphicsEngine();
			initialRoom.Enter();
			GraphicsEngine.Current.Run(gameStart);
		}

		public static void Run(GraphicsEngine engine, Room initialRoom, Action gameStart)
		{
			GraphicsEngine.Current = engine;
			initialRoom.Enter();
			engine.Run(gameStart);
		}

		public static void Quit()
		{
			GraphicsEngine.Current.Quit();
		}

		public static void Loop()
		{
			int timeLeft = 1000 / Room.Current.Speed + Environment.TickCount;
			// Begin step
			// Alarm events
			// Keyboard, key press, key release
			// Mouse
			// Normal step 
			//  (MovingObjects will update their positions)
			// Collision events
			// End step
			// Draw events
			foreach (var controller in Instance._controllers)
				controller.OnBeginStep();
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
			foreach (var controller in Instance._controllers)
				controller.OnEndStep();


			timeLeft -= Environment.TickCount;
			if (timeLeft > 0)
				Thread.Sleep(timeLeft);
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

		public static void Redraw(Surface surface)
		{
			Draw.Clear(Background.Color);
			if (Background.Image != null)
				Draw.Sprite(0, 0, Background.Image, 0);

			GlobalEvent.OnDrawBackground();

			foreach (var controller in Instance._controllers)
				controller.OnBeginDraw();
			foreach (var instance in Instance._objects)
				instance.OnDraw();
			foreach (var controller in Instance._controllers)
				controller.OnEndDraw();

			GlobalEvent.OnDrawForeground();

			GraphicsEngine.Current.Refresh();
		}

		public static void Sleep(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}
	}
}
