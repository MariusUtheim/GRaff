using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
    public static class Instance
    {
		internal static InstanceList _objects = new InstanceList();
		internal static MutableList<Controller> _controllers = new MutableList<Controller>();

		internal static void Sort()
		{
			_objects.Sort();
		}

		internal static void Add(GameObject instance)
		{
			_objects.Add(instance);
		}

		internal static void Add(Controller controller)
		{
			_controllers.Add(controller);
		}

		internal static void Remove(GameObject instance)
		{
			_objects.Remove(instance);
		}

		internal static void Remove(Controller controller)
		{
			_controllers.Remove(controller);
		}

		public static IEnumerable<GameObject> Noone { get { yield break; } }

		public static GameObject[] All
		{
			get
			{
				return _objects.ToArray();
			}
		}

		public static IEnumerable<GameObject> In(Rectangle rectangle)
		{
			return All.Where(i => i.BoundingBox.Intersects(rectangle));
		}

		public static IEnumerable<GameObject> Where(Func<GameObject, bool> predicate)
		{
			return All.Where(predicate);
		}
    }

	public static class Instance<T> where T : GameObject
	{
		public static T Create()
		{
			return Activator.CreateInstance<T>();
		}

		public static T Create(double x, double y)
		{
			var constructors = typeof(T).GetConstructors();
			foreach (var constructor in constructors)
			{
				if (constructor.GetParameters().Length == 2
				 && constructor.GetParameters()[0].ParameterType == typeof(Double)
				 && constructor.GetParameters()[1].ParameterType == typeof(Double))
					return constructor.Invoke(new object[] { x, y }) as T;
			}

			foreach (var constructor in constructors)
			{
				if (constructor.GetParameters().Length == 1
				 && constructor.GetParameters()[0].ParameterType == typeof(Point))
					return constructor.Invoke(new object[] { new Point(x, y) }) as T;
			}

			var newInstance = Activator.CreateInstance<T>();
			newInstance.X = x;
			newInstance.Y = y;
			return newInstance;
		}

		public static T Create(Point location)
		{
			var constructors = typeof(T).GetConstructors();

			foreach (var constructor in constructors)
			{
				if (constructor.GetParameters().Length == 1
				 && constructor.GetParameters()[0].ParameterType == typeof(Point))
					return constructor.Invoke(new object[] { location }) as T;
			}

			foreach (var constructor in constructors)
			{
				if (constructor.GetParameters().Length == 2
				 && constructor.GetParameters()[0].ParameterType == typeof(Double)
				 && constructor.GetParameters()[1].ParameterType == typeof(Double))
					return constructor.Invoke(new object[] { location.X, location.Y }) as T;
			}

			var newInstance = Activator.CreateInstance<T>();
			newInstance.Location = location;
			return newInstance;
		}

		public static IEnumerable<T> All
		{
			get { return Instance._objects.OfType<T>(); }
		}

		public static void Do(Action<T> action)
		{
			foreach (var obj in All)
				action(obj);
		}

		public static IEnumerable<T> In(Rectangle rectangle)
		{
			return All.Where(i => i.BoundingBox.Intersects(rectangle));
		}

		public static T First
		{
			get { return All.First(); }
		}

		public static IEnumerable<T> Where(Func<T, bool> predicate)
		{
			return All.Where(predicate);
		}
	}
}
