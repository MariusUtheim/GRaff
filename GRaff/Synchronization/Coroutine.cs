using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	public class Coroutine : GameElement
	{
		private int _count;
		private readonly IEnumerator<int> _routine;

		public Coroutine(IEnumerator<int> routine)
		{
			Contract.Requires<ArgumentNullException>(routine != null);
			_count = 0;
			_routine = routine;
		}

		private static IEnumerable<int> _project(IEnumerable routine)
		{
			foreach (var x in routine)
				yield return 1;
		}

		public static Coroutine Start(IEnumerable routine)
		{
			Contract.Requires<ArgumentNullException>(routine != null);
			return Instance.Create(new Coroutine(_project(routine).GetEnumerator()));
		}

		public static Coroutine Start(IEnumerable<int> routine)
		{
			Contract.Requires<ArgumentNullException>(routine != null);
			return Instance.Create(new Coroutine(routine.GetEnumerator()));
		}

		public static Coroutine Start(Func<IEnumerable> routine)
		{
			Contract.Requires<ArgumentNullException>(routine != null);
			return Instance.Create(new Coroutine(_project(routine()).GetEnumerator()));
		}

		public static Coroutine Start(Func<IEnumerable<int>> routine)
		{
			Contract.Requires<ArgumentNullException>(routine != null);
			return Instance.Create(new Coroutine(routine().GetEnumerator()));
		}

		public void Wait()
		{
			while (_routine.MoveNext()) ;
		}

		public sealed override void OnStep()
		{
			if (--_count <= 0)
			{
				if (!_routine.MoveNext())
					Destroy();
				else
					_count = _routine.Current;
			}
		}
	}
}
