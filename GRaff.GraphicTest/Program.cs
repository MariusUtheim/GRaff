using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics.Text;

namespace GRaff.GraphicTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Game.Run(1024, 768, gameStart);
		}

		static void gameStart()
		{
			GlobalEvent.ExitOnEscape = true;
			Textures.LoadAll();

			Instance.Create(new TestController());
		}

	}
}
