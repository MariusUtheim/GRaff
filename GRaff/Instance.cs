using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
    public static class Instance
    {
		private static InstanceList _elements = new InstanceList();

		internal static bool NeedsSort { get; set; }

		internal static void Sort()
		{
			_elements.Sort();
			NeedsSort = false;
		}

		public static TGameElement Create<TGameElement>(TGameElement instance) where TGameElement : GameElement
		{
			_elements.Add(instance);
			return instance;
		}

		public static void Remove(GameElement instance)
		{
			_elements.Remove(instance);
		}

		public static IEnumerable<GameElement> All
		{
            get
			{
				return _elements;
			}
		}
    }

	/// <summary>
	/// Provides static methods to interact with the instances of a specific type.
	/// </summary>
	/// <typeparam name="T">The type of GameObject.</typeparam>
	public static class Instance<T> where T : GameElement
	{
		/// <summary>
		/// Returns all instances of the specified type.
		/// </summary>
		public static IEnumerable<T> All
		{
			get { return Instance.All.OfType<T>(); }
		}

		/// <summary>
		/// Performs the action to each instance of the specified type.
		/// </summary>
		/// <param name="action">The action to perform</param>
		public static void Do(Action<T> action)
		{
			foreach (var obj in All)
				action.Invoke(obj);
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
