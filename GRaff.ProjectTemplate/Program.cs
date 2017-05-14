using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRaff;

namespace GRaff.ProjectTemplate
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			Giraffe.Run(1280, 768, gameStart);
		}

		/// <summary>
		/// Initialize your game here.
		/// </summary>
		static void gameStart()
		{
			GlobalEvent.ExitOnEscape = true;
			Instance.Create(new Background { Color = Colors.LightGray });

			// Create more objects
		}
	}
}
