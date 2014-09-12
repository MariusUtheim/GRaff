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

		internal static void Sort()
		{
			_objects.Sort();
		}

		internal static void Add(GameObject instance)
		{
			_objects.Add(instance);
		}

		internal static void Remove(GameObject instance)
		{
			_objects.Remove(instance);
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

	/// <summary>
	/// Provides static methods to interact with the instances of a specific type.
	/// </summary>
	/// <typeparam name="T">The type of GameObject.</typeparam>
	public static class Instance<T> where T : GameObject
	{
		/// <summary>
		/// Returns all instances of the specified type.
		/// </summary>
		public static IEnumerable<T> All
		{
			get { return Instance._objects.OfType<T>(); }
		}

		/// <summary>
		/// Performs the action to each instance of the specified type.
		/// </summary>
		/// <param name="action">The action to perform</param>
		public static void Do(Action<T> action)
		{
			foreach (var obj in All)
				action(obj);
		}

		/// <summary>
		/// Returns one instance of the specified type.
		/// </summary>
		public static T First()
		{
			return All.First();
		}

		/// <summary>
		/// Filters all instances of the specified type based on a predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static IEnumerable<T> Where(Func<T, bool> predicate)
		{
			return All.Where(predicate);
		}
	}
}
