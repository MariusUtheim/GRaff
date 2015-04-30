using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.GraphicTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Giraffe.Run(1024, 768, gameStart);
		}

		static void gameStart()
		{
			GlobalEvent.ExitOnEscape = true;
			TextureBuffers.LoadAll();

			Instance<SpriteTest>.Create();
		}
	}
}
