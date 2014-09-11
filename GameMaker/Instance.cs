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

	public static class Instance<T> where T : GameObject
	{
		public static IEnumerable<T> All
		{
			get { return Instance._objects.OfType<T>(); }
		}

		public static void Do(Action<T> action)
		{
			foreach (var obj in All)
				action(obj);
		}

		public static T First
		{
			get { return All.First(); }
		}

		public static T Single
		{
			get { return All.Single(); }
		}

		public static IEnumerable<T> Where(Func<T, bool> predicate)
		{
			return All.Where(predicate);
		}
	}
}
