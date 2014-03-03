using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
	public class NormalBlock : Block
	{
		public NormalBlock(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public override void Hit(Ball other)
		{
			this.Destroy();
		}

		public override GameMaker.Sprite Sprite
		{
			get { return Sprites.Block; }
		}
	}
}
