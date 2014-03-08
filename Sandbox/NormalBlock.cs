using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;
using GameMaker.Motions;

namespace Sandbox
{
	public class NormalBlock : Block
	{
		public NormalBlock(int x, int y)
		{
			Sprite = Sprites.Block;
			this.X = x;
			this.Y = y;
			this.Image.XScale = 2;
			this.Image.YScale = 2;
		}

		public override void Hit(Ball other)
		{
			this.Destroy();
		}

		public override void Step()
		{
			base.Step();

			if (Mask.ContainsPoint(Mouse.Location))
				Image.Blend = Color.Green;
			else
				Image.Blend = Color.White;
		}

		public override void OnDraw()
		{
			base.OnDraw();
			Mask.DrawOutline();
		}
	}
}
