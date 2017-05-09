using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GRaff.Synchronization
{

	public partial class Tween : GameElement
	{
		private event EventHandler<double> _step;
		private event EventHandler _complete;
		private Dictionary<int, List<Action>> _sentinels = new Dictionary<int, List<Action>>();

		public Tween(int duration, TweeningFunction tweeningFunction, Action<double> stepAction)
			: this(duration, tweeningFunction, stepAction, null)
		{ }

		public Tween(int duration, TweeningFunction tweeningFunction, Action<double> stepAction, Action completeAction)
		{
			Contract.Requires<ArgumentNullException>(tweeningFunction != null);
			Duration = duration;
			TweeningFunction = tweeningFunction;
			_step += (sender, e) => stepAction(e);
			_complete += (sender, e) => completeAction?.Invoke();
		}

		public static Tween Start(int duration, TweeningFunction tweeningFunction, Action<double> stepAction)
			=> Instance.Create(new Tween(duration, tweeningFunction, stepAction));

		public static Tween Start(int duration, TweeningFunction tweeningFunction, Action<double> stepAction, Action completeAction)
			=> Instance.Create(new Tween(duration, tweeningFunction, stepAction, completeAction));
		public event EventHandler<double> Step
		{
			add { _step += value; } 
			remove { _step -= value; }
		}

		public event EventHandler Complete
		{
			add { _complete += value; }
			remove { _complete -= value; }
		}

		public void AtStep(int step, Action action)
		{
			if (step < 0 || step >= Duration || action == null)
				return;

			List<Action> actions;
			if (_sentinels.TryGetValue(step, out actions))
				actions.Add(action);
			else
				_sentinels[step] = new List<Action>(new[] { action });
		}

		public int Duration { get; private set; }

		public int Progress { get; private set; }

		public TweeningFunction TweeningFunction { get; private set; }

		public sealed override void OnStep()
		{
			Progress++;
			if (Progress == Duration)
				_step?.Invoke(null, TweeningFunction(1));
			else
				_step?.Invoke(null, TweeningFunction((double)Progress / Duration));

			List<Action> sentinels;
			if (_sentinels.TryGetValue(Progress, out sentinels))
				foreach (var sentinel in sentinels)
					sentinel.Invoke();
			
			if (Progress == Duration)
			{
				_complete?.Invoke(null, null);
				Destroy();
			}
		}

	}
}
