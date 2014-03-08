using GameMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boids
{
	static class Program
	{
		static void Main()
		{
			GameMaker.Forms.Runner.Run(gameStart);
		}

		static void gameStart()
		{
			Point origin = new Point(400, 300);

			for (var offset = new Vector(30, Angle.Zero); offset.Direction < GMath.Tau; offset.Direction += GMath.Tau / 100)
				Instance<Boid>.Create(origin + offset);
			
			Instance<Hoik>.Create(origin + new Vector(50, 0));

			GlobalEvent.MousePressed += button => { new Boid(Mouse.Location); };

			GlobalEvent.KeyPressed += key => { if (key == Key.Escape) Game.Quit(); };
		}
	}
}
