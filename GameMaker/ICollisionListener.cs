using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public interface ICollisionListener
	{
	}

	public interface ICollisionListener<T> : ICollisionListener
		where T : GameObject
	{
		void OnCollision(T other);
	}
}
