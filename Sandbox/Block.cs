using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMaker;

namespace Sandbox
{
	public abstract class Block : GameObject
	{
		public abstract void Hit(Ball other);
	}
}
