using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff;
using GRaff.Synchronization;

namespace TestGame
{
	class CoroutineTest : GameElement, IKeyPressListener, IKeyReleaseListener
	{
		private Coroutine _routine = null;

		public CoroutineTest()
		{
		}

		public void OnKeyRelease(Key key)
		{
			if (_routine != null)
			{
				_routine.Wait();
				_routine = null;
			}
		}

		public void OnKeyPress(Key key)
		{
			if (_routine == null)
				_routine = Coroutine.Start(coroutine);
		}

		private IEnumerable<int> coroutine()
		{
			Window.Title = "1";
			yield return 10;
			Window.Title = "2";
			yield return 60;
			Window.Title = "3";
			yield return 120;
			Window.Title = "<< Done >>";
		}
	}
}
