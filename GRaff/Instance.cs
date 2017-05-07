using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
    public static class Instance
    {
		private static readonly InstanceList _elements = new InstanceList();

		internal static bool NeedsSort { get; set; }

		internal static void Sort()
		{
			_elements.Sort();
			NeedsSort = false;
		}

		public static TGameElement Create<TGameElement>(TGameElement instance) where TGameElement : IGameElement
		{
			_elements.Add(instance);
			return instance;
		}

		/// <summary>
		/// Destroys the instance of this GRaff.GameObject, removing it from the game.
		/// The instance will stop performing automatic actions such as Step and Draw,
		/// but the C# object is not garbage collected while it is still being referenced.
		/// </summary>
		public static void Destroy(this IGameElement element)
		{
			if (Remove(element))
				(element as IDestroyListener)?.OnDestroy();
		}


		internal static bool Remove(IGameElement instance)
		{
			return _elements.Remove(instance);
		}

		public static IEnumerable<IGameElement> All
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
	public static class Instance<T> where T : IGameElement
	{
		private static bool _isAbstract;
		private static Func<T> _parameterlessConstructor;
		private static Func<Point, T> _locationConstructor;
		private static Func<double, double, T> _xyConstructor;

		static Instance()
		{
			var type = typeof(T);

			_isAbstract = type.IsAbstract;

			var constructors = type.GetConstructors();
			var parameterTypes = constructors.Select(c => c.GetParameters().Select(p => p.ParameterType)).ToArray();

			var parameterlessMatch = constructors.FirstOrDefault(c => !c.GetParameters().Select(p => p.ParameterType).Any());
			var locationMatch = constructors.FirstOrDefault(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(Point) }));
			var xyMatch = constructors.FirstOrDefault(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(double), typeof(double) }));

			if (locationMatch == null && xyMatch == null)
			{
				if (parameterlessMatch == null)
					return;
				_parameterlessConstructor = () => (T)parameterlessMatch.Invoke(new object[0]);
				_locationConstructor = location => { var obj = (GameObject)parameterlessMatch.Invoke(new object[0]); obj.Location = location; return (T)(object)obj; };
				_xyConstructor = (x, y) => { var obj = (GameObject)parameterlessMatch.Invoke(new object[0]); obj.X = x; obj.Y = y; return (T)(object)obj; };
			}
			else
			{
				if (locationMatch != null)
					_locationConstructor = location => (T)locationMatch.Invoke(new object[] { location });

				if (xyMatch != null)
					_xyConstructor = (x, y) => (T)xyMatch.Invoke(new object[] { x, y });

				if (locationMatch == null)
					_locationConstructor = location => _xyConstructor(location.X, location.Y);
				if (xyMatch == null)
					_xyConstructor = (x, y) => _locationConstructor(new Point(x, y));

				if (parameterlessMatch != null)
					_parameterlessConstructor = () => (T)parameterlessMatch.Invoke(new object[0]);
				else if (locationMatch != null)
					_parameterlessConstructor = () => _xyConstructor(0, 0);
				else
					_parameterlessConstructor = () => _locationConstructor(Point.Zero);
			}
		}

		/// <summary>
		/// Creates a new instance of TGameObject.
		/// </summary>
		/// <returns>The created TGameObject.</returns>
		/// <remarks>
		/// If TGameObject has a parameterless constructor, this constructor is used.
		/// Otherwise, if it has a constructor accepting two System.Double structures, this constructor is called with (0.0, 0.0).
		/// Otherwise, if it has a constructor accepting a GRaff.Point, this constructor is called with GRaff.Point.Zero.
		/// If TGameObject has none of these constructors, a System.InvalidOperationException is thrown when Instance´1 is used.
		/// </remarks>
		public static T Create()
		{
			if (_isAbstract)
				throw new InvalidOperationException($"Unable to create instances of the abstract type {nameof(T)}");
            if (_parameterlessConstructor == null)
				throw new InvalidOperationException($"Unable to create instances through {nameof(Instance<T>)}: Type {nameof(T)} must specify a parameterless constructor, a constructor taking a GRaff.Point structure or a constructor taking two System.Double structures.");
			return Instance.Create(_parameterlessConstructor());
		}

		/// <summary>
		/// Creates a new instance of TGameObject, using the specified GRaff.Point as the argument.
		/// </summary>
		/// <param name="location">The argument of the called constructor, or the location where the TGameObject will be placed.</param>
		/// <returns>The created TGameObject.</returns>
		/// <remarks>
		/// If TGameObject has a constructor accepting a GRaff.Point, this constructor is called with location.
		/// Otherwise, if it has a constructor accepting two System.Double structures, this constructor is called with (location.X, location.Y).
		/// Otherwise, if it has a parameterless constructor, this constructor is called; then the Location property of the created object is set to location.
		/// If TGameObject has none of these constructors, a System.InvalidOperationException is thrown when Instance´1 is used.</remarks>
		/// </remarks>
		public static T Create(Point location)
		{
			if (_parameterlessConstructor == null)
				throw new InvalidOperationException($"Unable to create instances through {nameof(Instance<T>)}: Type {nameof(T)} must specify a parameterless constructor, a constructor taking a GRaff.Point structure or a constructor taking two System.Double structures.");
			return Instance.Create(_locationConstructor(location));
		}

		/// <summary>
		/// Creates a new instance of TGameObject, using the specified x- and y-coordinates as the arguments.
		/// </summary>
		/// <param name="x">The first argument of the called constructor, or the x-coordinate where the TGameObject will be placed.</param>
		/// <param name="y">The second argument of the called constructor, or the y-coordinate where the TGameObject will be placed.</param>
		/// <returns>The created TGameObject</returns>
		/// If TGameobject has a constructor accepting two System.Double structures, this constructor is called with (x, y).
		/// Otherwise, f TGameObject has a constructor accepting a GRaff.Point, this constructor is called with a new GRaff.Point using the specified (x, y) coordinates.
		/// Otherwise, if it has a parameterless constructor, this constructor is called; then the X property of the created object is set to x, and the Y property is set to y.
		/// If TGameObject has none of these constructors, a System.InvalidOperationException is thrown when Instance´1 is used.</remarks>
		/// </remarks>
		public static T Create(double x, double y)
		{
			if (_parameterlessConstructor == null)
				throw new InvalidOperationException(string.Format("Unable to create instances through {0}: Type {1} must specify a parameterless constructor, a constructor taking a GRaff.Point structure or a constructor taking two System.Double structures.", typeof(Instance<T>).Name, typeof(T).Name));
			return Instance.Create(_xyConstructor(x, y));
		}

		public static T _ => Enumerate().FirstOrDefault();

		/// <summary>
		/// Returns all instances of the specified type.
		/// </summary>
		public static IEnumerable<T> Enumerate() => Instance.All.OfType<T>();

		public static bool Any() => Enumerate().Any();
		public static bool Any(Func<T, bool> predicate) => Enumerate().Any(predicate);
		public static bool All(Func<T, bool> predicate) => Enumerate().All(predicate);
		public static IEnumerable<T> Where(Func<T, bool> predicate) => Enumerate().Where(predicate);
		public static IEnumerable<T> Where(Func<T, int, bool> predicate) => Enumerate().Where(predicate);

		/// <summary>
		/// Performs the action to each instance of the specified type.
		/// </summary>
		/// <param name="action">The action to perform</param>
		public static void Do(Action<T> action)
		{
			foreach (var obj in Enumerate())
				action.Invoke(obj);
		}

		public static bool DoOnce(Action<T> action)
		{
			foreach (var obj in Enumerate())
			{
				action.Invoke(obj);
				return true;
			}
			return false;
		}

		public static bool DoOnceIf(Func<T, bool> predicate, Action<T> action)
		{
			foreach (var obj in Where(predicate))
			{
				action.Invoke(obj);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns one instance of the specified type.
		/// </summary>
		public static T First() => Enumerate().First();
		public static T First(Func<T, bool> predicate) => Enumerate().First(predicate);
		public static T FirstOrDefault() => Enumerate().FirstOrDefault();
		public static T FirstOrDefault(Func<T, bool> predicate) => Enumerate().FirstOrDefault(predicate);
        public static T Single() => Enumerate().Single();
		public static T Single(Func<T, bool> predicate) => Enumerate().Single(predicate);
    }
}
