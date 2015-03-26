using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
	public static class Instance<T> where T : GameObject
	{
		private static Func<T> _parameterlessConstructor;
		private static Func<Point, T> _locationConstructor;
		private static Func<double, double, T> _xyConstructor;

		static Instance()
		{
			var type = typeof(T);
			var constructors = type.GetConstructors();
			var paremeterTypes = constructors.Select(c => c.GetParameters().Select(p => p.ParameterType)).ToArray();

			var parameterlessMatch = constructors.FirstOrDefault(c => !c.GetParameters().Select(p => p.ParameterType).Any());
			var locationMatch = constructors.FirstOrDefault(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(Point) }));
			var xyMatch = constructors.FirstOrDefault(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(double), typeof(double) }));
			
			if (locationMatch == null && xyMatch == null)
			{
				if (parameterlessMatch == null)
					return;
				_parameterlessConstructor = () => (T)parameterlessMatch.Invoke(new object[0]);
				_locationConstructor = location => { var obj = (T)parameterlessMatch.Invoke(new object[0]); (obj as GameObject).Location = location; return obj; };
				_xyConstructor = (x, y) => { var obj = (T)parameterlessMatch.Invoke(new object[0]); obj.X = x; obj.Y = y; return obj; };
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
			if (_parameterlessConstructor == null)
				throw new InvalidOperationException(string.Format("Unable to create instances through {0}: Type {1} must specify a parameterless constructor, a constructor taking a GRaff.Point structure or a constructor taking two System.Double structures.", typeof(Instance<T>).Name, typeof(T).Name));
			return _parameterlessConstructor();
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
				throw new InvalidOperationException(string.Format("Unable to create instances through {0}: Type {1} must specify a parameterless constructor, a constructor taking a GRaff.Point structure or a constructor taking two System.Double structures.", typeof(Instance<T>).Name, typeof(T).Name));
			return _locationConstructor(location);
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
			return _xyConstructor(x, y);
		}



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
		public static T First
		{
			get
			{
				return All.First();
			}
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
