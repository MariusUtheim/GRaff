using System;


namespace GRaff.Synchronization
{

/*	public sealed class TweenEventArgs : EventArgs
	{
		public TweenEventArgs(int loopCount, double t)
		{
			this.LoopCount = loopCount;
			this.T = t;
		}

		public int LoopCount { get; private set; }

		public double T { get; private set; }
	}
	*/

	public class Tween : GameElement
	{
		private Action<double> _tweenAction;
		private Action _completeAction;

		public Tween(int duration, Func<double, double> tweeningFunction, Action<double> tweenAction)
			: this(duration, tweeningFunction, tweenAction, null)
		{ }

		public Tween(int duration, Func<double, double> tweeningFunction, Action<double> tweenAction, Action completeAction)
		{
			if (tweeningFunction == null) throw new ArgumentNullException("tweeningFunction"); /*C#6.0*/
			Duration = duration;
			TweeningFunction = tweeningFunction;
			_tweenAction = tweenAction;
			_completeAction = completeAction;
		}

		public static Tween Start(int duration, Func<double, double> tweeningFunction, Action<double> tweenAction)
		{
			return Instance.Create(new Tween(duration, tweeningFunction, tweenAction));
		}

		public static Tween Start(int duration, Func<double, double> tweeningFunction, Action<double> tweenAction, Action completeAction)
		{
			return Instance.Create(new Tween(duration, tweeningFunction, tweenAction, completeAction));
		}

		/*C#6.0*/
		public static Tween Linear(int duration, Action<double> tweenAction, Action completed = null)
		{
			return Instance.Create(new Tween(duration, t => t, tweenAction, completed));
		}

		public static Tween Cubic(int duration, Action<double> tweenAction, Action completeAction = null)
		{
			return Instance.Create(new Tween(duration, t => t * t * (2 - t), tweenAction, completeAction));
		}

		public static Tween Sine(int duration, Action<double> tweenAction, Action completeAction = null)
		{
			return Instance.Create(new Tween(duration, t => 0.5 * (1 - GMath.Cos(t * GMath.Pi)), tweenAction, completeAction));
		}

		#if !PUBLISH
		public Tween Inverse()
		{
			return new Tween(Duration, t => 1 - this.TweeningFunction(t), _tweenAction);
		}

		public Tween TwoSidedStep()
		{
			return new Tween(Duration, t => (t < 0.5) ? TweeningFunction(2 * t) / 2.0 : 1 - TweeningFunction(2 - 2 * t) / 2.0, _tweenAction);
		}

		public Tween TwoSidedInterpolate()
		{
			return new Tween(Duration, t => TweeningFunction(t) * (1 - t) + TweeningFunction(1 - t) * t, _tweenAction);
        }
		#endif

		public int Duration { get; private set; }

		public int Progress { get; private set; }

		public Func<double,double> TweeningFunction { get; private set; }

		public sealed override void OnStep()
		{
			Progress++;
			if (_tweenAction != null)  /*C#6.0*/
				_tweenAction.Invoke(TweeningFunction((double)Progress / Duration));
			if (Progress == Duration)
			{
				if (_completeAction != null)
					_completeAction.Invoke();
				Destroy();
			}
		}
	}
}
