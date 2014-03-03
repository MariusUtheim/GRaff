using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker.Forms
{
	public static class Runner
	{
		public static void Run(Action gameStart)
		{
			Game.Run<FormsGraphicsEngine>(gameStart);
		}
	}
}
