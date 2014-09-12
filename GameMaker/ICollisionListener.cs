using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Represents a non-generic base class for the generic ICollisionListener<T>.
	/// This interface should not be implemented by other classes.
	/// </summary>
	public interface ICollisionListener
	{
	}

	/// <summary>
	/// Defines an action that is performed when the instance collides with another object of the specified type.
	/// </summary>
	/// <typeparam name="T">The type of object to collide with. This type parameter is covariant.</typeparam>
#warning TODO: Check that the covariance is okay.
	public interface ICollisionListener<in T>
		where T : GameObject
	{
		/// <summary>
		/// An action that is performed whenever this instance collides with another instance of the specified type.
		/// </summary>
		/// <param name="other">The instance that was collided with.</param>
		void OnCollision(T other);
	}
}
